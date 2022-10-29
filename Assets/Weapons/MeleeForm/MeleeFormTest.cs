using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/Melee Form")]
public class MeleeFormTest : BaseForm
{
    [Header("Form Specific Data")]
    public GameObject meleeFX;
    public GameObject testCube;
    public override void FormAction(float context)
    {
        base.FormAction(-1);
        Instantiate(testCube, CursorController.Instance.cursorWorldPosition, Quaternion.identity);
    }
}
