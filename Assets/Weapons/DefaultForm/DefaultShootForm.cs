using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/DefaultShoot Form")]
public class DefaultShootForm : BaseForm
{
    [Header("Form Specific Data")]
    [SerializeField] private GameObject _bullet;
    [SerializeField] private LayerMask raycastCheckLayers;
 
    public override void FormAction(float context)
    {
        base.FormAction(-1);
        
        var bullet = Instantiate(_bullet,FormController.Instance.currentForm.barrelSpawn.position, Quaternion.identity);

        RaycastHit info;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out info, 100.0f, raycastCheckLayers))
        {
            var pos = info.point;
            bullet.GetComponent<BaseBullet>().SetTargetPosition(pos);
        }
        else {
            var dir = PlayerController.Instance._playerCamera.forward;
            bullet.GetComponent<BaseBullet>().SetDirection(dir);
        }


            
        
    }
}
