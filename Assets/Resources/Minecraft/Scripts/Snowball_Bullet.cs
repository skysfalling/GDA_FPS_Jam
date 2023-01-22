using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Base class for all physical bullets in the game 
 * 
 * Inherit when making your own bullet for your gun
 */
public class Snowball_Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;
    GameObject player;


    public float damage = 1.0f;
    public float speed = 1.0f;
    public float lifespan = 6.0f;

    [SerializeField] protected float hipSpread;
    [SerializeField] protected float ADSspread;

    public Vector3 targetPosition;
    public bool hasTargetPosition;

    [Header("Snowball Attributes")]
    public GameObject breakParticles;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        ApplyForce();
        Destroy(gameObject, lifespan);
    }

    public virtual void Update()
    {
        transform.LookAt(player.transform);
    }

    public virtual void ApplyForce()
    {
        Vector3 impulseDirection;

        //If the bullet has a target position, calculate the normalized vector pointing at that position and set the impulse direction to that vector.
        //If it doesn't have a target position, just set the impulse direction to the direction of the projectile.
        if (hasTargetPosition)
        {
            impulseDirection = Vector3.Normalize(targetPosition - transform.position);
        }
        else
        {
            impulseDirection = transform.forward;
        }

        float spreadValue = 0;

        //If the player is not aiming down sights, use the hip random spread, else use the ADS spread.
        if (!FormController.Instance.isADS)
        {
            spreadValue = hipSpread;
        }
        else
        {
            spreadValue = ADSspread;
        }

        //Add the random spread to the impulse direction calculated previously
        impulseDirection += new Vector3(Random.Range(-spreadValue, spreadValue), Random.Range(-spreadValue, spreadValue), Random.Range(-spreadValue, spreadValue));

        _rigidbody.AddForce(impulseDirection * speed, ForceMode.VelocityChange);
    }

    //Set target position for this bullet;
    public virtual void SetTargetPosition(Vector3 position)
    {
        targetPosition = position;
        hasTargetPosition = true;
    }

    public virtual void SetDirection(Vector3 direction)
    {
        transform.forward = direction;
    }

    // Runs when bullet hits a gameObject
    public virtual void OnImpact(Collision collision) {}

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") { return; }

        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);
        }

        OnImpact(collision);

        GameObject particles = Instantiate(breakParticles, transform.position, Quaternion.Euler(-90f, 0, 0));
        Destroy(particles, 1);

        Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        HitInteractable hitable = other.gameObject.GetComponent<HitInteractable>();

        // Check if target is a hitable
        if (hitable != null)
        {
            // If it does, damage the target
            hitable.ProcessHit();
        }

        Destroy(gameObject);
    }

}
