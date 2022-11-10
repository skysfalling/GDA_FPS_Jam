using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public float CurrentHealth
    {
        get;
        set;
    }
    public float BaseHealth
    {
        get;
        set;
    }

    public float health;
    public GameObject damagePop;

    void Start()
    {

    }

    public void ProcessDamage(float damage)
    {
        // Find vector towards camera from origin
        Quaternion CameraAngle = Quaternion.LookRotation(this.transform.position - Camera.main.transform.position);
        CameraAngle = new Quaternion(0, CameraAngle.y, 0, CameraAngle.w);

        // Create Damage pop facing camera
        GameObject DamagePop =  Instantiate(damagePop,this.transform.position, CameraAngle);
        DamagePop.GetComponent<DamagePopFade>().Setup(damage);

        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Destroy(gameObject, 4f);
        }
    }

}
