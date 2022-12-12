using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Abstract class for all gameObjects that have a response to being hit.
 * 
 * Examples include: Targets that flip on hit and UI that can be shot.
 * 
 * Use Damageable.cs for objects that take damage from shots
 */

public class HitInteractable : MonoBehaviour
{
    private CrosshairFlairController flairController;

    // Start is called before the first frame update
    public virtual void Start()
    {
        flairController = GameObject.Find("crosshair_flair").GetComponent<CrosshairFlairController>();
    }

    public virtual void ProcessHit()
    {
        flairController.Flair(15f);
        OnHitEffect();
    }

    // Runs every bullet hit
    public virtual void OnHitEffect() { }
}
