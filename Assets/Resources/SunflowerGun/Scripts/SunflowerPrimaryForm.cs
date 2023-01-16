using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/Sunflower Primary Form")]
public class SunflowerPrimaryForm : BaseForm
{
    [Header("Form Specific Data")]
    public GameObject projectile;
    public LayerMask raycastCheckLayers;


    public override void FormAction(float context)
    {
        // If charge time wasn't long enough, do nothing
        if (FormController.Instance._currentPrimaryHoldDuration < maxHoldDuration)
        {
            return;
        }

        Debug.Log("shot");
        base.FormAction(context);

        //Spawn bullet prefab at weapon's barrel position
        GameObject bullet = Instantiate(projectile, FormController.Instance.currentForm.barrelSpawn.position, Quaternion.identity);
        SpawnedGarbageController.Instance.AddAsChild(bullet);
        RaycastHit info;


        // Raycast into world from camera position + direction, if target found, set bullet target position to that point, else, bullet direction mimics player camera.
        // This allows us to shoot these projectile bullets from the gun rather than the center of the screen to get the desired appearance
        // If the weapon were hitscan, we could skip this and just add tracers from the gun to the desired destination
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, 999f, raycastCheckLayers))
        {
            Vector3 pos = info.point;
            bullet.GetComponent<BaseBullet>().SetTargetPosition(pos);
        }
        else
        {
            Vector3 dir = PlayerController.Instance._playerCamera.forward;
            bullet.GetComponent<BaseBullet>().SetDirection(dir);
        }
    }
}
