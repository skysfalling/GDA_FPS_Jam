using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Base class for all physical bullets in the game 
 * 
 * Inherit when making your own bullet for your gun
 */
public class BaseBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    public float damage = 1.0f;
    public float speed = 1.0f;
    public float lifespan = 6.0f;

    [SerializeField] private float hipSpread;
    [SerializeField] private float ADSspread;

    public Vector3 targetPosition;
    public bool hasTargetPosition;

    public virtual void Start()
    {
        ApplyForce();
        Destroy(gameObject, lifespan);
    }

    public virtual void ApplyForce()
    {
        Vector3 impulseDirection;
        if (hasTargetPosition)
        {
            impulseDirection = Vector3.Normalize(targetPosition - transform.position);
        }
        else
        {
            impulseDirection = transform.forward;
        }

        if (!FormController.Instance.isADS)
        {
            impulseDirection += new Vector3(Random.Range(-hipSpread, hipSpread), Random.Range(-hipSpread, hipSpread), Random.Range(-hipSpread, hipSpread));
        }
        else
        {
            impulseDirection += new Vector3(Random.Range(-ADSspread, ADSspread), Random.Range(-ADSspread, ADSspread), Random.Range(-ADSspread, ADSspread));
        }

        _rigidbody.AddForce(impulseDirection * speed, ForceMode.VelocityChange);
    }

    public void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        hasTargetPosition = true;
    }

    public void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    // Runs when bullet hits a gameObject
    public virtual void OnImpact(Collision collision) {}

    public virtual void OnCollisionEnter(Collision collision)
    {
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);
        }

        OnImpact(collision);

        Destroy(gameObject);
    }



}
