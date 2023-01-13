using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnBoxController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        collider.transform.position = new Vector3(0, 4, 0);
    }
}
