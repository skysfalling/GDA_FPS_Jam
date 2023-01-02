using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseForm : ScriptableObject
{

    public enum EnergyUsage { Unlimited, CooldownMag, SingleMag };
    public enum EnergyRegenerationType { ConstantFullFill, ConstantIncremental, EmptyFullFill };
    public enum FireType { Auto, Semi, Hold };

    [Header("Base Form Data")]
    public string id;
    public string description;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public Transform pivot;
    public FireType firingType;
    public float actionCooldown;
    public float maxHoldDuration;

    [Header("Energy Data")]
    public EnergyUsage energyType;
    public EnergyRegenerationType energyRegenType;
    public float energyRegenCooldown;
    public float energyRegenRate;
    public float energyMax;
    public float energyCost;
    public bool shareEnergyWithOtherForm;

    [Header("Screen Shake Impulse")]
    [Tooltip("Amount of shake. Leave at 0 for no shake.")]
    public float screenShakeImpulseMagnitude;
    [Tooltip("Leave at 0,0,0 for random direction")]
    public Vector3 screenShakeImpulseDirection;

    [Header("Audio")]
    public AudioClip audioClip;
    public float volume = 1.0f;

    public virtual void FormAction(float context)
    {
        PlayFormAudio();
    }

    public virtual void InitializeForm(Transform input)
    {
        pivot = input;
    }

    public virtual void PlayFormAudio()
    {
        if (audioClip != null)
        {
            AudioManager.Instance.PlaySoundEffect(audioClip, volume);
        }
            
    }



}


