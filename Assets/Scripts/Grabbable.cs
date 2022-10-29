using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : Interactable
{
    public Rigidbody _rb;
    public PlayerController playerController;
    public bool usesGravity;
    public bool dontDisableCollider;
    public bool dontAlterConstraints;
    public bool isLocked;
    public bool dontCollideWithPlayer;
    public float offset;
    public bool destroyable;

    public override void Start()
    {
        base.Start();
        if(GetComponent<Rigidbody>() != null)
        {
            _rb = GetComponent<Rigidbody>();
        }

    }

    public override void InteractAction()
    {
        base.InteractAction();
        IsBeingGrabbed();
    }

    public void IsBeingGrabbed()
    {
        PlayerController.Instance.SetCurrentGrabbable(this);
        if (!dontAlterConstraints)
        {
            _rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        
        if (usesGravity)
        {
            _rb.useGravity = false;
        }
        
        gameObject.layer = 2;
    }

    public void IsNoLongerBeingGrabbed()
    {
        if(PlayerController.Instance.currentGrabbable == this)
        {
            UnlockGrabbable();
            PlayerController.Instance.currentGrabbable = null;
            if (!dontAlterConstraints)
            {
                _rb.constraints = RigidbodyConstraints.None;
            }
            
            if (usesGravity)
            {
                _rb.useGravity = true;
            }

            if (dontCollideWithPlayer)
            {
                gameObject.layer = 11;
            }
            else
            {
                gameObject.layer = 9;
            }
        }
    }

    public void LockGrabbable()
    {
        isLocked = true;
        _rb.isKinematic = true;
        if (!dontDisableCollider)
        {
            GetComponent<Collider>().enabled = false;
        }
        
    }

    public void UnlockGrabbable()
    {
        if (!isLocked)
        {
            return;
        }

        isLocked = false;
        _rb.isKinematic = false;
        transform.parent = null;
        if (!dontDisableCollider)
        {
            GetComponent<Collider>().enabled = true;
        }

    }

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

}
