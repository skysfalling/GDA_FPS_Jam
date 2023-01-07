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
        var bullet = Instantiate(_bullet, GameController.Instance.ownedFormController.currentForm.barrelSpawn.position, Quaternion.identity);
        bullet.GetComponent<BaseHitscan>().damage = maximumDamage * ((GameController.Instance.ownedFormController.currentForm._currentSecondaryEnergy+1)/energyMax);
        GameController.Instance.ownedFormController.currentForm._currentSecondaryEnergy = 0;
        SpawnedGarbageController.Instance.AddAsChild(bullet);
        // Raycast into world from camera position + direction, if target found, set bullet target position to that point, else, bullet direction mimics player camera.
        // This allows us to shoot these projectile bullets from the gun rather than the center of the screen to get the desired appearance
        // If the weapon were hitscan, we could skip this and just add tracers from the gun to the desired destination

        bullet.GetComponent<BaseHitscan>().SetTargetDirection(Camera.main.transform.forward);
        

    }
}
