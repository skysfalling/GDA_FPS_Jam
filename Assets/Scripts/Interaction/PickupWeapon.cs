using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : Interactable
{
    public Rigidbody _rb;
    public PlayerController playerController;
    public GameObject weaponPrefab;
    public Transform meshParent;

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
        Debug.Log("grabbed weapon");
        IsBeingGrabbed();
    }

    public void IsBeingGrabbed()
    {
        //PlayerController.Instance.SetCurrentGrabbable(this);
        GameController.Instance.ownedFormController.SetNewWeapon(weaponPrefab);
    }

    public void SetMesh(GameObject meshPrefab)
    {
        if(meshParent == null)
        {
            return;
        }

        Instantiate(meshPrefab, meshParent);
    }
   

    public Rigidbody GetRigidbody()
    {
        return _rb;
    }

}
