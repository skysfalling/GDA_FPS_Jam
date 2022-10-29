using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Forms/DefaultShoot Form")]
public class DefaultShootForm : BaseForm
{
    [Header("Form Specific Data")]
    [SerializeField] private GameObject _bullet;
    public override void FormAction(float context)
    {
        base.FormAction(-1);
        
        var bullet = Instantiate(_bullet,FormController.Instance.spawnPivot.position, Quaternion.identity);
        bullet.GetComponent<BaseBullet>().SetDirection(PlayerController.Instance._playerCamera.forward);
    }
}
