using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SunflowerChargeController : MonoBehaviour
{
    // This feels a lot more hardcodey than I'd like it to be, but fuck it this is a game jam lmao
    // -Enrico

    public FormObject formObject;
    public Animator animator;
    public AudioSource chargingUpSoundSource;


    private void Update()
    {
        if (!FormController.Instance._isReloading &&
            FormController.Instance._currentPrimaryIsPressed &&
            formObject._currentPrimaryCooldown <= 0)
        {
            if (!chargingUpSoundSource.isPlaying)
            {
                chargingUpSoundSource.Play();
                // Probably play Charge animation here
            }

            // Formula for getting % of charge done
            // FormController.Instance._currentPrimaryHoldDuration / formObject.primaryForm.maxHoldDuration
        }
        else
        {
            if (chargingUpSoundSource.isPlaying)
            {
                chargingUpSoundSource.Stop();
                // And go back to Idle animation here
            }
        }
    }
}
