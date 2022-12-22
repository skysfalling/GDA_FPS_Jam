using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShootableButton : HitInteractable
{
    bool _triggered = false;
    SpriteRenderer _spriteRenderer;


    [Header("Properties")]
    [Space(10)]
    public bool canDisable = true; // If not, disable using a exterior call to DisableButton()
    public bool startOn = false;


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

        if (startOn)
        {
            runOnEnable.Invoke();
            _spriteRenderer.sprite = spriteEnabled;
            _triggered = true;
        }
    }

    // Handle Hit Interaction
    public override void OnHitEffect()
    {

        // Is button Disabled?
        if (!_triggered)
        {
            // If so enable button
            runOnEnable.Invoke();
            _spriteRenderer.sprite = spriteEnabled;
            _triggered = true;

        }
        else
        {
            if (canDisable)
            {
                // Otherwise, if we can, disable button
                runOnDisable.Invoke();
                _spriteRenderer.sprite = spriteDisabled;
                _triggered = false;
            }
        }
    }
     
    // Disable button
    public void DisableButton()
    {
        runOnDisable.Invoke();
        _spriteRenderer.sprite = spriteDisabled;
        _triggered = false;
    }
}
