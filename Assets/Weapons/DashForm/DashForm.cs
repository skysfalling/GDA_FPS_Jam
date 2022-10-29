using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create UnityEditor UI option to create form
[CreateAssetMenu(menuName = "Forms/Dash Form")]
public class DashForm : BaseForm
{
    [Header("Form Specific Data")]
    [SerializeField] private float forceStrength;
    public override void FormAction(float context)
    {
        base.FormAction(-1);
        //PlayerController.Instance.TempRemoveSpeedLimit(0.2f);
        //PlayerController.Instance.TempRemovePlayerControl(context/2f);
        //PlayerController.Instance.ApplyForceTowardsOfPosition(CursorController.Instance.cursorWorldPosition, context/maxHoldDuration * forceStrength, true);
        Debug.Log("Performed Dash Action");
    }
}
