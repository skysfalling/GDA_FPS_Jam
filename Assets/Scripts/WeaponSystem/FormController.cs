using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FormController : UnitySingleton<FormController>
{

    public FormObject currentForm;
    public int currentFormIndex = 0;
    public List<FormObject> formList;
    public Transform spawnPivot;
    [SerializeField] private GameObject _formParent;
    public Transform ADSPosition;

    [Header("Primary Status")]
    public bool _currentPrimaryIsPressed = false;
    public bool _currentPrimaryIsReady = true;
    public float _currentPrimaryHoldDuration = 0;

    [Header("Secondary Status")]
    public bool _currentSecondaryIsPressed = false;
    public bool _currentSecondaryIsReady = true;
    public float _currentSecondaryHoldDuration = 0;

    [Header("ADS Status")]
    public bool isADS;

    // Start is called before the first frame update
    void Start()
    {
        GrabForms();
        SetForm(0);
        ResetController();
    }

    // Update is called once per frame
    void Update()
    {
        UseActions();
    }

    public void SetForm(int index)
    {
        currentFormIndex = index;
        currentForm = formList[currentFormIndex];
    }

    public void SetNewWeapon(GameObject newWeapon)
    {
        ClearCurrentWeapon();
        var newGun = Instantiate(newWeapon, _formParent.transform);
        newGun.transform.localPosition = Vector3.zero;
        FormObject newForm = newGun.transform.GetComponent<FormObject>();
        currentForm = newForm;
    }

    public void ClearCurrentWeapon()
    {
        currentForm = null;
        Destroy(_formParent.transform.GetChild(0).gameObject);
    }

    public void SetForm(FormObject formObj)
    {
        currentForm = formObj;
    }

    public void SwitchForm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentFormIndex++;
            if (currentFormIndex >= formList.Count)
            {
                SetForm(0);
            }
            else
            {
                SetForm(currentFormIndex);
            }
        }
        
    }

    void GrabForms()
    {
        formList.Clear();
        FormObject[] forms = _formParent.GetComponentsInChildren<FormObject>();
        foreach (FormObject form in forms)
        {
            formList.Add(form);
        }
    }

    void ResetController()
    {
        _currentPrimaryIsReady = true;
        _currentSecondaryIsReady = true;
        _currentPrimaryIsPressed = false;
        _currentSecondaryIsPressed = false;
        _currentPrimaryHoldDuration = 0;
        _currentSecondaryHoldDuration = 0;
    }


    void UseActions()
    {
        
        CheckPrimaryActions();
        CheckSecondaryActions();

    }

    void CheckPrimaryActions()
    {
        if (!_currentPrimaryIsPressed || currentForm.primaryForm == null)
        {
            return;
        }

        if (currentForm.primaryForm.firingType == BaseForm.FireType.Auto)
        {
            currentForm.UsePrimaryAction(-1);
        }
        else if (currentForm.primaryForm.firingType == BaseForm.FireType.Semi && _currentPrimaryIsReady)
        {
            _currentPrimaryIsReady = false;
            currentForm.UsePrimaryAction(-1);
        }
        else if (currentForm.primaryForm.firingType == BaseForm.FireType.Hold)
        {
            if (currentForm._currentPrimaryCooldown <= 0 && _currentPrimaryHoldDuration < currentForm.primaryForm.maxHoldDuration)
            {
                _currentPrimaryHoldDuration += Time.deltaTime;
                Mathf.Clamp(_currentPrimaryHoldDuration, 0, currentForm.primaryForm.maxHoldDuration);
            }


        }
    }

    void CheckSecondaryActions()
    {
        if (!_currentSecondaryIsPressed || currentForm.secondaryForm == null)
        {
            return;
        }

        if (currentForm.secondaryForm.firingType == BaseForm.FireType.Auto)
        {
            currentForm.UseSecondaryAction(-1);
        }
        else if (currentForm.secondaryForm.firingType == BaseForm.FireType.Semi && _currentSecondaryIsReady)
        {
            _currentSecondaryIsReady = false;
            currentForm.UseSecondaryAction(-1);
        }
        else if (currentForm.secondaryForm.firingType == BaseForm.FireType.Hold)
        {
            if (currentForm._currentSecondaryCooldown <= 0 && _currentSecondaryHoldDuration < currentForm.secondaryForm.maxHoldDuration)
            {
                _currentSecondaryHoldDuration += Time.deltaTime;
                Mathf.Clamp(_currentSecondaryHoldDuration, 0, currentForm.secondaryForm.maxHoldDuration);
            }


        }
    }

    private void LateUpdate()
    {

    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (currentForm.primaryForm == null)
        {
            return;
        }

        if (context.started)
        {
            _currentPrimaryIsReady = true;
            
        }

        if (context.canceled)
        {
            if (currentForm.primaryForm.firingType == BaseForm.FireType.Hold && currentForm._currentPrimaryCooldown <= 0)
            {
                currentForm.UsePrimaryAction(_currentPrimaryHoldDuration);
                _currentPrimaryHoldDuration = 0;
            }
        }

        _currentPrimaryIsPressed = context.ReadValueAsButton();
    }

    public void AltFire(InputAction.CallbackContext context)
    {
        if(currentForm.secondaryForm == null)
        {
            return;
        }

        if (context.started)
        {
            _currentSecondaryIsReady = true;

        }
        if (context.canceled)
        {
            if (currentForm.secondaryForm.firingType == BaseForm.FireType.Hold && currentForm._currentSecondaryCooldown <= 0)
            {
                currentForm.UseSecondaryAction(_currentSecondaryHoldDuration);
                _currentSecondaryHoldDuration = 0;
            }
        }

        _currentSecondaryIsPressed = context.ReadValueAsButton();
    }

    public void AimDownSights(InputAction.CallbackContext context)
    {
        //If player starts holding down ADS
        if (context.started)
        {
            isADS = true;
            ToggleADSCamera();
            //Move the weapon to the ADS position to fix bullet spawn position
            LeanTween.move(currentForm.gameObject, ADSPosition, 0.15f).setEase(LeanTweenType.easeOutQuad);
            //Update Max player movement speed
            PlayerController.Instance.UpdateMaximumInputSpeed();

        }

        //If player stops holding down ADS
        if (context.canceled)
        {
            isADS = false;
            ToggleADSCamera();
            //Move the weapon back to the formParent/weaponParent position to fix bullet spawn position
            LeanTween.move(currentForm.gameObject, _formParent.transform, 0.15f).setEase(LeanTweenType.easeOutQuad);
            //Update Max player movement speed
            PlayerController.Instance.UpdateMaximumInputSpeed();

        }

    }

    //Enables and disables the camera attached to this Form/Weapon
    public void ToggleADSCamera()
    {
        if (isADS)
        {
            currentForm.virtualCamera.enabled = true;
            PlayerController.Instance.isPressingSprint = false;
        }
        else
        {
            currentForm.virtualCamera.enabled = false;
        }
    }


}
