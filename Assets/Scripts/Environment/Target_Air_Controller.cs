using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * Example Damagable
 * 
 * This is a flip target that flips 180 degrees when hit
 */

public class Target_Air_Controller : HitInteractable
{
    bool flip;
    float current_rotation = 1;
    Vector3 start_rotation;
    float rate_of_rot = 200;
    bool destroy = false;


    public UnityEvent RunOnHit;



    public override void Start()
    {
        base.Start();
        start_rotation = transform.eulerAngles;
    }

    private void FixedUpdate()
    {
        // If target isn't flipped over...
        if (flip && current_rotation < 180)
        {
            // Increment current rotation, working towards 180
            current_rotation = Mathf.Min(180, current_rotation + (rate_of_rot * Time.fixedDeltaTime));
            rate_of_rot *= 1.25f;

            // Set our current rotation to our original + our above increment
            Vector3 next_rot = start_rotation;
            next_rot.z += current_rotation;
            transform.eulerAngles = next_rot;
        }
        else { 

            // At 180 degrees, done flipping over
            flip = false; 
        }

        // Check if target is being destroyed
        if (destroy)
        {
            // While Scale is larger than 0
            if (transform.localScale.y > 0)
            {
                // Scale down
                transform.localScale -= new Vector3(5f, 150f, 150f) * Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public override void OnHitEffect()
    {
        // If target isn't flipping we can flip again
        if (!flip && !destroy)
        {
            RunOnHit.Invoke();

            // Set default stats, localized to current rotation
            flip = true;
            start_rotation = transform.eulerAngles;
            rate_of_rot = 200;
            current_rotation = 1;
        }
    }

    public void DestoryTarget()
    {
        destroy = true;
    }
}
