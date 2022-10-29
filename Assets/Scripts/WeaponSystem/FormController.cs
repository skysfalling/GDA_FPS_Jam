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

    [Header("Primary Status")]
    public bool _currentPrimaryIsPressed = false;
    public bool _currentPrimaryIsReady = true;

    [Header("Secondary Status")]
    public bool _currentSecondaryIsPressed = false;
    public bool _currentSecondaryIsReady = true;

    public float _currentPrimaryHoldDuration = 0;
    public float _currentSecondaryHoldDuration = 0;

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


}
