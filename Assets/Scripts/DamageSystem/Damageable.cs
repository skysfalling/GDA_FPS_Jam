using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [Header("Damageable Stats")]
    public float CurrentHealth;
    public float BaseHealth;

    [Header("Serialized Prefabs")]
    [SerializeField] private GameObject damagePop;

    public virtual void Start()
    {
        CurrentHealth = BaseHealth;
    }

    public virtual void ProcessDamage(float damage)
    {
        // Find vector towards camera from origin
        Quaternion CameraAngle = Quaternion.LookRotation(this.transform.position - Camera.main.transform.position);
        CameraAngle = new Quaternion(0, CameraAngle.y, 0, CameraAngle.w);

        // Create Damage pop facing camera
        GameObject DamagePop = Instantiate(damagePop, this.transform.position, CameraAngle);
        SpawnedGarbageController.Instance.AddAsChild(DamagePop);
        DamagePop.GetComponent<DamagePopFade>().Setup(damage);

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject, 4f);
        }
    }
}
