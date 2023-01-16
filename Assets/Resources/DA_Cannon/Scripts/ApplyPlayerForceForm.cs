using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Forms/ApplyPlayerForce Form")]
public class ApplyPlayerForceForm : BaseForm
{
    [Header("Form Specific Data")]
    public LayerMask raycastCheckLayers;

    [Header("Player Force")]
    public float playerForceMagnitude;
    public float forceDuration;
    public bool constantDirection = false;
    private bool ShouldShowDir() { if (constantDirection) { return true; } return false; }
    [ShowIf("ShouldShowDir")] public Vector3 playerForceDirection;

    //[SerializeField] PlayerController.PlayerStats statsWhileActive;

    // ======================================================================================================================================

    // Brainstorming:
    // Three guns, two of these could use this.
    // Cannon: 
    //      Secondary-  Rocket jump. Applies a force on the player opposite the direction they're looking. Is more accurate when aiming.
    // Vacuum:
    //      Primary-    Sucks. Applies a force on an object the player is looking at (within a range) towards the player. The longer they
    //                  suck, the more energy they build up for the secondary, up to a cap.
    //      Secondary-  Blows, dual purpose. If the player is not sucking, this just applies an outward force on the object the player is
    //                  looking at. If the player is sucking, the object they've been sucking is sent flying outwards. If it makes contact
    //                  with a valid target, the target takes damage proportional to the amount of time they've been sucking.

    // ======================================================================================================================================

    //FormAction() is called each time the form "shoots".
    public override void FormAction(float context)
    {
        base.FormAction(-1);

        Vector3 dir;
        
        if (!constantDirection){ dir = -PlayerController.Instance._playerCamera.transform.forward; }
        else { dir = playerForceDirection.normalized; }

        Vector3 forceVector = (playerForceMagnitude*dir);
        
        float elapsed = 0f;
        PlayerController.Instance._rigidbody.velocity += (new Vector3 (0, forceVector.y, 0));

        while (elapsed < forceDuration)
        {
            PlayerController.Instance._rigidbody.velocity += (new Vector3 (forceVector.x*20f, 0, forceVector.z*20f));
            elapsed += Time.deltaTime;
        }
    }
}