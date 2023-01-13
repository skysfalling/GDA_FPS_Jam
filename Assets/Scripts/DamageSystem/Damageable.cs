using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
 * Abstract class for all Damageable gameObjects
 * 
 * If you want something to take damage, you need a script on the gameObject that inherits this class
 */
public abstract class Damageable : MonoBehaviour
{
    [Header("Damageable Stats")]
    public float CurrentHealth;
    public float BaseHealth;
    public bool InfiniteHealth;

    [Header("Serialized Prefabs")]
    [SerializeField] private GameObject damagePop;

    private CrosshairFlairController flairController; 

    public virtual void Start()
    {
        CurrentHealth = BaseHealth;
        flairController = GameObject.Find("crosshair_flair").GetComponent<CrosshairFlairController>();
    }

    // Apply damage to our Damageable
    public virtual void ProcessDamage(float damage)
    {
        /// Create Damage hit marker
        
        // Find vector towards camera from origin
        Quaternion CameraAngle = Quaternion.LookRotation(this.transform.position - Camera.main.transform.position);
        CameraAngle = new Quaternion(0, CameraAngle.y, 0, CameraAngle.w);

        // Create Damage pop facing camera
        GameObject DamagePop = Instantiate(damagePop, this.transform.position, CameraAngle);
        SpawnedGarbageController.Instance.AddAsChild(DamagePop);
        DamagePop.GetComponent<DamagePopFade>().Setup(damage);

        /// Apply Damage if able
        if (!InfiniteHealth)
        {
            CurrentHealth -= damage;
        }

        /// Process damage
        if (CurrentHealth <= 0)
        {
            flairController.Flair(20f);
            OnHitEffect();
            OnDestroyEvent();
            Destroy(gameObject);
        }
        else
        {
            flairController.Flair(15f);
            OnHitEffect();
        }
    }
    
    // Runs every bullet hit
    public virtual void OnHitEffect() {}

    // Runs once Damageable reaches 0 Health and is destroyed
    public virtual void OnDestroyEvent() {}
}
