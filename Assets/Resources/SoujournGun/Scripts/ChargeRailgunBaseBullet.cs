using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeRailgunBaseBullet : BaseBullet
{
    public FormObject hostForm;
    [SerializeField] private float energyPerHit;

    public override void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);

            if(hostForm != null)
            {
                hostForm._currentSecondaryEnergy += energyPerHit;
                hostForm._currentSecondaryEnergy = Mathf.Clamp(hostForm._currentSecondaryEnergy, 0, hostForm.secondaryForm.energyMax);
            }
        }

        OnImpact(collision);

        Destroy(gameObject);
    }


}
