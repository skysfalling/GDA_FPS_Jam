using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [Header("User Stats")]
    public Vector2 moveDirection = Vector2.zero;
    public Vector2 lookDirection = Vector2.zero;
    public Vector3 currentRotation;
    public float xRotation = 0f;
    float mouseX, mouseY;
    public Vector2 sensitivity;
    public float movementSpeed;
    
    [Header("References")]
    public PlayerInputs playerControls;
    [SerializeField] private Transform _cameraParent;
    private InputAction _movement;
    private InputAction _look;
    private Rigidbody _rb;
    //private CharacterController _characterController;


    private void OnEnable()
    {
        playerControls = new PlayerInputs();

        _movement = playerControls.Player.Move;
        _look = playerControls.Player.Look;

        _movement.Enable();
        _look.Enable();

        _rb = GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        _movement.Disable();
        _look.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RecieveInput();
        LookMove();
    }

    void RecieveInput()
    {
        lookDirection = _look.ReadValue<Vector2>();
        mouseX = lookDirection.x * sensitivity.x;
        mouseY = lookDirection.y * sensitivity.y;
    }

    void LookMove()
    {
        moveDirection = _movement.ReadValue<Vector2>();

        transform.Rotate(new Vector3(0, mouseX * Time.deltaTime, 0));

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = xRotation;

        _cameraParent.eulerAngles = targetRotation;
        
        //_cameraParent.localEulerAngles = new Vector3(currentRotation.x,0,0);

        _rb.velocity = (transform.forward * moveDirection.y * movementSpeed) + (transform.right * moveDirection.x * movementSpeed);

        //_characterController.Move(new Vector3(moveDirection.x, 0, moveDirection.y) * Time.deltaTime * 2);

    }


}
