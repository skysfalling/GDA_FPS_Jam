using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Animation brain unique for each weapon
// Decouples animation state logic from FormController
// Editable by Jam Participants, to alter animation triggers

// Place script on your weapon model, same as your animator

public class AnimatorBrain : MonoBehaviour
{
    FormController currentPlayerStatus;
    Animator animController;

    bool reloadingAnimationPlayed = false;

    private void Start()
    {
        currentPlayerStatus = GameObject.Find("Player").GetComponent<FormController>();
        animController = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check animation states
        // This is the only place where these animation parameters are altered
        // Feel free to remove these and set the animation up how you want
 
        animController.SetBool("Fire", isFiring() && !isReloading());

        if (isReloading() && !reloadingAnimationPlayed)
        {
            animController.SetTrigger("Reload");
            reloadingAnimationPlayed = true;
        }

        if (!isReloading())
        {
            reloadingAnimationPlayed = false;
        }
    }

    // Helper Methods
    // Note: currentPlayerStatus has access to more information
    // To see check the script FormController.cs, attached to the Player gameObject
    public bool isADS(){ return currentPlayerStatus.isADS; }

    public bool isReloading() { return currentPlayerStatus._isReloading; }

    public bool isFiring() { return currentPlayerStatus.FiredGun; }

    public bool isHoldingDownPrimary() { return (currentPlayerStatus._currentPrimaryHoldDuration > 0); }

    public bool isHoldingDownSecondary() { return (currentPlayerStatus._currentSecondaryHoldDuration > 0); }
}
