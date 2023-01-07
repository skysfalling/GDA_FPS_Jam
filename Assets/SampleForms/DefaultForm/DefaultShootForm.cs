using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[CreateAssetMenu(menuName = "Forms/DefaultShoot Form")]
public class DefaultShootForm : BaseForm
{
    [Header("Form Specific Data")]
    public GameObject _bullet;
    public LayerMask raycastCheckLayers;
 
    //FormAction() is called each time the form "shoots".
    public override void FormAction(float context)
    {
        base.FormAction(-1);

        //Spawn bullet prefab at weapon's barrel position
        //var bullet = Instantiate(_bullet, GameController.Instance.ownedFormController.currentForm.barrelSpawn.position, Quaternion.identity);
        //bullet.GetComponent<NetworkObject>().Spawn();
        //SpawnedGarbageController.Instance.AddAsChild(bullet);
        GameController.Instance.ownedFormController.projectileBuffer = _bullet;
        GameController.Instance.ownedFormController.SpawnShootServerRpc();
    }

}
