using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/ChargeRailgun_SecondaryForm")]
public class ChargeRailgunSecondaryForm : BaseForm
{
    [Header("Form Specific Data")]
    public GameObject _bullet;
    public float maximumDamage;

    //FormAction() is called each time the form "shoots".
    public override void FormAction(float context)
    {
        base.FormAction(-1);

        //Spawn bullet prefab at weapon's barrel position
        var bullet = Instantiate(_bullet, GetBarrelTransform().position, Quaternion.identity);

        //Modify damage of hitscan laser based on current secondary energy of current weapon.
        bullet.GetComponent<BaseHitscan>().damage = maximumDamage * ((GetCurrentFormObject()._currentSecondaryEnergy+1)/energyMax);

        //Forcefully reset secondary energy.
        GetCurrentFormObject()._currentSecondaryEnergy = 0;

        SpawnedGarbageController.Instance.AddAsChild(bullet);
        bullet.GetComponent<BaseHitscan>().SetTargetDirection(Camera.main.transform.forward);
        

    }
}
