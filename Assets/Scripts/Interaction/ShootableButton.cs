using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootableButton : HitInteractable
{
    bool _triggered = false;
    SpriteRenderer _spriteRenderer;

    [Header("Sprites")]
    [Space(10)]
    public Sprite spriteEnabled;
    public Sprite spriteDisabled;

    [Header ("Handle Button Input")]
    [Space(10)]
    public UnityEvent runOnEnable;
    public UnityEvent runOnDisable;

    public override void Start()
    {
        base.Start();
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Handle Hit Interaction
    public override void OnHitEffect()
    {

        // Is button Enabled?
        if (_triggered)
        {
            // If so disable button
            runOnDisable.Invoke();
            _spriteRenderer.sprite = spriteDisabled;
            _triggered = false;
        }
        else
        {
            // Otherwise enable button
            runOnEnable.Invoke();
            _spriteRenderer.sprite = spriteEnabled;
            _triggered = true;
        }
    }
}
