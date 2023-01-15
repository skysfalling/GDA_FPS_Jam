using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingFish_PrimaryProjectile : BaseBullet
{
    public float homingStrength = 0;
    public float homingRate = 1;
    public float maximumHomingSpeed = 5;
    public Transform homingTarget;

    public override void Start()
    {
        base.Start();
        homingTarget = FormController.Instance.currentForm.GetComponent<HomingFishWeaponController>().homingTarget;
    }

    private void Update()
    {

        if(homingStrength < 1)
        {
            homingStrength += homingRate * Time.deltaTime;
        }

    }

    private void FixedUpdate()
    {
        if(homingTarget == null)
        {
            return;
        }

        ApplyHomingForce();
    }

    void ApplyHomingForce()
    {
        Vector3 homingDirection = (homingTarget.position - transform.position).normalized;


        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity.normalized, homingDirection, homingStrength) * _rigidbody.velocity.magnitude;

        //_rigidbody.AddForce(homingDirection * homingStrength, ForceMode.Impulse);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, maximumHomingSpeed);
        transform.forward = _rigidbody.velocity.normalized;

    }


}
