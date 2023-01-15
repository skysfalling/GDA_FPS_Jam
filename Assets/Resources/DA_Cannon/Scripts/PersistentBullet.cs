using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentBullet : BaseBullet
{
    public override void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);
        }

        OnImpact(collision);
    }
}
