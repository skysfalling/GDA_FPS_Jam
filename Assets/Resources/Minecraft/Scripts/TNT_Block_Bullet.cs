using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Base class for all physical bullets in the game 
 * 
 * Inherit when making your own bullet for your gun
 */
public class TNT_Block_Bullet : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;

    public float damage = 1.0f;
    public float blast_damage = 50;
    public float speed = 1.0f;
    public float lifespan = 6.0f;

    [SerializeField] protected float hipSpread;
    [SerializeField] protected float ADSspread;

    public Vector3 targetPosition;
    public bool hasTargetPosition;

    [Header("TNT Attributes")]
    public float flashDelay = 0.5f;

    public Material tnt_material;
    public Material flash_material;

    public GameObject explodeParticles;

    public float explosionForce = 10.0f;
    public float explosionRadius = 5.0f;
    public LayerMask affectedLayers;

    public virtual void Start()
    {
        ApplyForce();

        InvokeRepeating("FlashToggle", 0, flashDelay);

        Invoke("Explode", lifespan);
    }

    // toggle the flashing of the tnt block
    public void FlashToggle()
    {

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        
        if (meshRenderer.sharedMaterials[0] == tnt_material)
        {
            meshRenderer.material = flash_material;
        }
        else
        {
            meshRenderer.material = tnt_material;
        }

    }

    public void Explode()
    {
        // Get all colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        // Apply explosion force to each collider
        foreach (Collider col in colliders)
        {
            // explosion force
            if (col.GetComponent<Rigidbody>())
            {
                Rigidbody rb = col.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            // damage
            if (col.GetComponent<Damageable>())
            {
                // If it does, damage the target
                col.GetComponent<Damageable>().ProcessDamage(blast_damage);
            }
        }

        GameObject particles = Instantiate(explodeParticles, transform.position, Quaternion.identity);
        Destroy(particles, 2);

        Destroy(gameObject);
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
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        // Check if target is a Damageable
        if (damageable != null)
        {
            // If it does, damage the target
            damageable.ProcessDamage(damage);
        }

        OnImpact(collision);

        //Destroy(gameObject);
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

        //Destroy(gameObject);
    }

}
