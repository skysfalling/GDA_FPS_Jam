using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeShotBehavior : MonoBehaviour
{
    public FormObject formObject;
    public bool useSecondaryFireInstead = false;

    private float chargeTime = 0.533f;


    private void Start()
    {
        if (!useSecondaryFireInstead)
        {
            chargeTime = formObject.primaryForm.maxHoldDuration;
        }
        else
        {
            if (formObject.secondaryForm)
            {
                chargeTime = formObject.secondaryForm.maxHoldDuration;
            }
        }
    }


    private void Update()
    {
        // If fully charged, automatically shoot
        if (!useSecondaryFireInstead)
        {
            if (FormController.Instance._currentPrimaryIsPressed)
            {
                if (FormController.Instance._currentPrimaryHoldDuration >= chargeTime)
                {
                    formObject.UsePrimaryAction(-1f);
                }
            }
        }
        else
        {
            if (FormController.Instance._currentSecondaryIsPressed)
            {
                if (FormController.Instance._currentSecondaryHoldDuration >= chargeTime)
                {
                    Debug.Log("Fully charged");
                    formObject.UseSecondaryAction(-1f);
                }
            }
        }
    }


    /// <summary>
    /// Calculates the percentage of charge at a given point in time
    /// </summary>
    public float GetChargePercentage()
    {
        return formObject._currentPrimaryCooldown / chargeTime;
    }
}
