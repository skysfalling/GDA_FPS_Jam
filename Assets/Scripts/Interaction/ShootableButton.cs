using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableButton : HitInteractable
{
    bool _triggered = false;
    SpriteRenderer _spriteRenderer;
    public Sprite spriteEnabled;
    public Sprite spriteDisabled;

    public override void Start()
    {
        base.Start();
        _spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnHitEffect()
    {
        if (_triggered)
        {
            _spriteRenderer.sprite = spriteDisabled;
            _triggered = false;
        }
        else
        {
            _spriteRenderer.sprite = spriteEnabled;
            _triggered = true;
        }
    }
}
