using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    public UnityEvent enterEvent;
    public UnityEvent exitEvent;
    [SerializeField] private string _tagCheck;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.CompareTo(_tagCheck) == 0)
        {
            enterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.CompareTo(_tagCheck) == 0)
        {
            exitEvent.Invoke();
        }
    }
}
