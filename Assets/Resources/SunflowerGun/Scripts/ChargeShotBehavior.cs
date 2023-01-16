using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargeShotBehavior : MonoBehaviour
{
    public FormObject formObject;
    public float chargeTime = 0.533f;


    private bool fireButtonPressed;
    private float chargeDuration;


    /// <summary>
    /// Calculates the percentage of charge at a given point in time
    /// </summary>
    public float GetChargePercentage()
    {
        return formObject._currentPrimaryCooldown / chargeTime;
    }
}
