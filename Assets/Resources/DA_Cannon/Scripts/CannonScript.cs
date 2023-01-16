using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField] PlayerController.PlayerStats statsWhileActive;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController.Instance.ReplacePlayerStats(statsWhileActive);   
    }
}
