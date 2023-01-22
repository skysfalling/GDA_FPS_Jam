using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/Snowball Form")]
public class SnowballForm : BaseForm
{
    [Header("Form Specific Data")]
    public GameObject _bullet;
    public LayerMask raycastCheckLayers;
 
    //FormAction() is called each time the form "shoots".
    public override void FormAction(float context)
    {
        base.FormAction(-1);
        
        //Spawn bullet prefab at weapon's barrel position
        var bullet = Instantiate(_bullet,FormController.Instance.currentForm.barrelSpawn.position, Quaternion.identity);
        SpawnedGarbageController.Instance.AddAsChild(bullet);
        RaycastHit info;

        // Raycast into world from camera position + direction, if target found, set bullet target position to that point, else, bullet direction mimics player camera.
        // This allows us to shoot these projectile bullets from the gun rather than the center of the screen to get the desired appearance
        // If the weapon were hitscan, we could skip this and just add tracers from the gun to the desired destination
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, 200.0f, raycastCheckLayers))
        {
            var pos = info.point;
            bullet.GetComponent<Snowball_Bullet>().SetTargetPosition(pos);
        }
        else {
            var dir = PlayerController.Instance._playerCamera.forward;
            bullet.GetComponent<Snowball_Bullet>().SetDirection(dir);
        }
    }
}
