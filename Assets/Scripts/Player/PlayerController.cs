using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : UnitySingleton<PlayerController>
{
    [Header("Serialized Variables")]
    public Rigidbody _rigidbody;
    public Transform _playerCamera;
    [SerializeField] private CinemachineVirtualCamera _vcam;
    [SerializeField] private Volume m_Volume;
    private Vignette m_Vignette;


    [Header("Current Player State")]
    public Vector3 moveDirection;
    public Vector2 lookDirection;
    public bool isInspecting;
    public bool isGrounded;
    public bool isSprinting;
    public bool isPressingSprint;
    private Vector3 _currentVelocity;
    private float xRotation = 0f;
    public float scrollDirection;
    public float scrollModifier;
    public bool canControlMovement = true;

    [HideInInspector]
    [Header("Interaction System")]
    [SerializeField] private Transform _grabPivot;
    [HideInInspector]
    public Grabbable currentGrabbable;
    [HideInInspector]
    public float grabbableForce;
    [HideInInspector]
    public float baseInteractableDistance;
    [HideInInspector]
    public float maxInteractableDistance;
    [HideInInspector]
    public float minInteractableDistance;
    [HideInInspector]
    public float interactableDistanceChangeRate;
    [HideInInspector]
    public float interactableDistance;
    [HideInInspector]
    public LayerMask interactableLayers;
    [HideInInspector]
    public float inspectMinimumSensitivity;

    [System.Serializable]
    public struct PlayerStats
    {
        public float maximumInputSpeed;
        public float sprintModifier;
        public float ADSModifier;
        public float movementAcceleration;
        public float movementDrag;
        public float gravity;
        public float jumpForce;
    }

    [Header("Base Movement Stats")]

    public PlayerStats basePlayerStats;

    [Header("Current Movement Stats [Reset to Base on Play or Swap Weapons]")]
    [Tooltip("Maximum speed of the player (minus falling).")]
    [SerializeField] private float _maximumInputSpeed;
    private float _currentMaximumInputSpeed;
    [Tooltip("Scalar modifier to movement speed while sprinting.")]
    [SerializeField] private float _sprintModifier;
    [Tooltip("Scalar modifier to movement speed while aiming down sights.")]
    [SerializeField] private float _ADSModifier;
    [Tooltip("Movement acceleration of the player.")]
    [SerializeField] private float _movementAcceleration;
    [Tooltip("Movement drag of the player.")]
    [SerializeField] private float _movementDrag;
    [Tooltip("Force of gravity of the player.")]
    [SerializeField] private float _gravity;
    [Tooltip("Amount of jump force the player has.")]
    [SerializeField] private float _jumpForce;

    [Header("UI/Camera Related Stats")]
    [Tooltip("Field of view of the player.")]
    [SerializeField] private float _FOV;
    [Tooltip("Field of view while the player is sprinting.")]
    [SerializeField] private float _sprintFOV;
    [Tooltip("Sensitivity of the player cursor.")]
    [SerializeField] private float _lookSensitivity = 1.0f;
    [Tooltip("Current state (0-1) of the VFX.")]
    [SerializeField] private float currentVFXState;
    [SerializeField] private float targetFOV = 90;
    [SerializeField] private float currentVFXStateRate;

    public override void Awake()
    {
        base.Awake();
        _vcam.m_Lens.FieldOfView = _FOV;
        _currentMaximumInputSpeed = _maximumInputSpeed;
        m_Volume.profile.TryGet<Vignette>(out m_Vignette);
    }

    //Modify the current FOV of the player camera
    public void ModifyFOV(float fov)
    {
        _vcam.m_Lens.FieldOfView = fov;
    }

    //Updates the base FOV of the player
    public void UpdateBaseFOV(float newFOV)
    {
        _FOV = newFOV;
        _sprintFOV = newFOV - 10;
        ModifyFOV(_FOV);
    }

    //Replace the players stats with a new set of player stats.
    public void ReplacePlayerStats(PlayerStats newPlayerStats)
    {
        _maximumInputSpeed = newPlayerStats.maximumInputSpeed;
        _sprintModifier = newPlayerStats.sprintModifier;
        _ADSModifier = newPlayerStats.ADSModifier;
        _movementAcceleration = newPlayerStats.movementAcceleration;
        _movementDrag = newPlayerStats.movementDrag;
        _gravity = newPlayerStats.gravity;
        _jumpForce = newPlayerStats.jumpForce;
    }

    //Resets the players stats to the base values
    public void ResetBasePlayerStats()
    {
        ReplacePlayerStats(basePlayerStats);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeRigidbody();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canControlMovement)
        {
            return;
        }

        LimitMovement();
        RaycastGrabbablePivot();
        ApplyGrab();
        UpdateSprintVFX();

        if (isInspecting)
        {
            ApplyInspect();
        }
        else
        {
            ApplyLook();
        }

    }

    private void FixedUpdate()
    {
        if (!canControlMovement)
        {
            return;
        }

        ApplyMovement();
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        // If grounded, stop, else apply force of gravity downwards.
        if (isGrounded)
        {
            return;
        }

        _rigidbody.AddForce(Vector3.down * _gravity, ForceMode.Acceleration);
    }

    private void ApplyLook()
    {
        // Grab MouseX and MouseY axis for use in rotating camera + player.
        // (I know this uses the old input system but this way solves some inconsistencies I was having with the new one)
        float lookX = Input.GetAxis("Mouse X") * _lookSensitivity;
        float lookY = Input.GetAxis("Mouse Y") * _lookSensitivity;

        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * lookX);
        _playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);


    }

    private void ApplyInspect()
    {
        if (currentGrabbable == null)
        {
            return;
        }

        Vector2 looking = GetPlayerLook();

        float lookX = 0;
        float lookY = 0;

        if (Mathf.Abs(looking.x) > inspectMinimumSensitivity)
        {
            lookX = looking.x * _lookSensitivity * Time.deltaTime;
        }

        if (Mathf.Abs(looking.y) > inspectMinimumSensitivity)
        {
            lookY = looking.y * _lookSensitivity * Time.deltaTime;
        }


        currentGrabbable.transform.Rotate((Vector3.up * lookX) + (Camera.main.transform.right * lookY), Space.World);

    }


    void InitializeRigidbody()
    {
        _rigidbody.drag = _movementDrag;
    }

    void ApplyMovement()
    {
        Vector2 movement = GetPlayerMovement();
        Vector3 move = (transform.right * movement.x) + (transform.forward * movement.y);

        _rigidbody.AddForce(move * _movementAcceleration, ForceMode.Acceleration);
    }

    void LimitMovement()
    {
        _rigidbody.velocity = Vector3.ClampMagnitude(new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z), _currentMaximumInputSpeed) + new Vector3(0, _rigidbody.velocity.y, 0);
    }

    public Vector2 GetPlayerMovement()
    {
        return moveDirection;
    }

    public Vector2 GetPlayerLook()
    {
        return lookDirection;
    }

    public void SetSensitivity(float value)
    {
        _lookSensitivity = value;
    }

    public void Look(InputAction.CallbackContext context)
    {
        lookDirection = context.ReadValue<Vector2>();

    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        UpdateMaximumInputSpeed();

    }

    public void SetGroundedState(bool state)
    {
        isGrounded = state;
    }

    void TryInteract()
    {
        RaycastHit info;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, 2, interactableLayers))
        {
            if (LayerMask.LayerToName(info.transform.gameObject.layer) == "Interactable" || LayerMask.LayerToName(info.transform.gameObject.layer) == "InteractableNoPlayerCollide")
            {
                info.transform.GetComponent<Interactable>().InteractAction();
            }
        }

    }

    void ApplyGrab()
    {
        if (currentGrabbable != null)
        {
            float grabbableOffset = Vector3.Distance(currentGrabbable.transform.position, _grabPivot.transform.position);
            Vector3 forceDirection = (_grabPivot.position - currentGrabbable.transform.position).normalized * grabbableForce * grabbableOffset;
            if (!currentGrabbable.isLocked)
            {
                currentGrabbable.GetRigidbody().AddForce(forceDirection, ForceMode.Acceleration);
                if (grabbableOffset < 0.5f)
                {
                    currentGrabbable.transform.position = _grabPivot.position;
                    currentGrabbable.LockGrabbable();
                    currentGrabbable.transform.SetParent(_grabPivot);
                }
            }

        }
    }

    void ApplyJump()
    {
        if (!isGrounded)
        {
            return;
        }

        _rigidbody.AddForce(_jumpForce * Vector3.up, ForceMode.Impulse);
    }

    void RaycastGrabbablePivot()
    {
        RaycastHit info;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        float offset = interactableDistance;

        if (currentGrabbable != null)
        {
            offset += currentGrabbable.offset;
        }


        if (Physics.Raycast(ray, out info, offset, interactableLayers))
        {
            float newDistance = info.distance;
            if (currentGrabbable != null)
            {
                newDistance -= currentGrabbable.offset;
            }


            _grabPivot.position = ray.GetPoint(newDistance);
        }
        else
        {
            _grabPivot.position = ray.GetPoint(interactableDistance);
        }
    }

    public void SetCurrentGrabbable(Grabbable grabbable)
    {
        currentGrabbable = grabbable;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        interactableDistance = baseInteractableDistance;
        if (currentGrabbable == null)
        {
            TryInteract();
        }
        else
        {
            currentGrabbable.IsNoLongerBeingGrabbed();
        }


    }

    public void OnInspect(InputAction.CallbackContext context)
    {
        if (currentGrabbable == null)
        {
            return;
        }
        if (context.started)
        {
            isInspecting = true;

        }
        if (context.canceled)
        {
            isInspecting = false;
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        scrollDirection = context.ReadValue<float>();
        interactableDistance += interactableDistanceChangeRate * scrollDirection;

        interactableDistance = Mathf.Clamp(interactableDistance, minInteractableDistance, maxInteractableDistance);


    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ApplyJump();
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started && !FormController.Instance.isADS)
        {
            isPressingSprint = true;
            UpdateMaximumInputSpeed();

        }

        if (context.canceled)
        {
            isPressingSprint = false;
            UpdateMaximumInputSpeed();
        }
    }

    public void UpdateMaximumInputSpeed()
    {
        if (FormController.Instance.isADS)
        {
            _currentMaximumInputSpeed = _maximumInputSpeed * _ADSModifier;
            return;
        }

        if (isPressingSprint && moveDirection.y > 0)
        {
            _currentMaximumInputSpeed = _maximumInputSpeed * _sprintModifier;
            isSprinting = true;
        }
        else
        {
            _currentMaximumInputSpeed = _maximumInputSpeed;
            isSprinting = false;
        }

    }

    //kinda gross but dw about it
    void UpdateSprintVFX()
    {
        if ((isSprinting || FormController.Instance.isADS) && currentVFXState < 1)
        {
            currentVFXState += Time.deltaTime * currentVFXStateRate;
        }
        else if (currentVFXState > 0 && !(isSprinting || FormController.Instance.isADS))
        {
            currentVFXState -= Time.deltaTime * currentVFXStateRate;
        }

        currentVFXState = Mathf.Clamp(currentVFXState, 0, 1);

        if (isSprinting)
        {
            targetFOV = _sprintFOV;
        }
        else if (FormController.Instance.isADS)
        {
            if (targetFOV == _sprintFOV)
            {
                currentVFXState = (_FOV / FormController.Instance.currentForm.ADSZoomModifier) / (currentVFXState * _sprintFOV);
            }

            targetFOV = _FOV / FormController.Instance.currentForm.ADSZoomModifier;
        }

        ModifyFOV(Mathf.Lerp(_FOV, targetFOV, currentVFXState));

        m_Vignette.intensity.value = Mathf.Lerp(0f, 0.2f, currentVFXState);
    }




}
