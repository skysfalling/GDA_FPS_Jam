using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public float health = 10f;
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

        health -= damage;

        if (health <= 0)
        {
            Destroy(gameObject, 4f);
        }
    }

}
