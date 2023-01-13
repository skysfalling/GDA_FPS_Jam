using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class BaseForm : ScriptableObject
{

    public enum EnergyUsage { Unlimited, CooldownMag};
    public enum EnergyRegenerationType { ConstantFullFill, ConstantIncremental, EmptyFullFill, ManualReload };
    public enum FireType { Auto, Semi, Hold };

    [Header("Base Form Data")]
    [Tooltip("An identification for this form. (This will not be displayed, it is purely for bugfixing purposes if " +
        "something goes wrong in officers creating combined submission testing range)")]
    public string id;
    [Tooltip("A description of what this form does. (This will not be displayed, it is purely for bugfixing purposes if " +
        "something goes wrong in officers creating combined submission testing range)")]
    public string description;
    [HideInInspector] public Sprite icon;
    [HideInInspector] public Transform pivot;
    [Tooltip("The firing type of this weapon - \n\n" +
        "Auto: This form, while held, continuously runs its FormAction() as long as the ActionCooldown has elapsed.\n\n" +
        "Semi: This form runs its FormAction() a single time on click, as long as the ActionCooldown has elapsed.\n\n" +
        "Hold: This form keeps track of how long the player has held the respective button, and upon release, runs its FormAction(), " +
        "sending the hold duration as additional context as a FormAction(float holdDuration). Clamped by the MaxHoldDuration state below.")]
    public FireType firingType;
    [Tooltip("The ActionCooldown is the cooldown between when this form will be able to run its FormAction(). To shoot two bullets per" +
        " second, the cooldown would be 0.5.")]
    public float actionCooldown;
    [ShowIf("ShouldShowHoldDuration")]
    [Tooltip("Maximum amount of time this form's input button can be held for while affecting the FormAction(float holdDuration) context.")]
    public float maxHoldDuration;

    private bool ShouldShowHoldDuration() { if (firingType != FireType.Hold) { return false; } return true; }
    private bool HasLimitedAmmo() { if (energyType != EnergyUsage.Unlimited) { return true; } return false; }

    private bool ShouldShowRegenRate() { if (HasLimitedAmmo() && energyRegenType != EnergyRegenerationType.ManualReload) { return true; } return false; }

    private bool ShouldShowAutoReload() { if (HasLimitedAmmo() && energyRegenType == EnergyRegenerationType.ManualReload) { return true; } return false; }

    private bool ShouldShowShakeDirection() { if (screenShakeImpulseMagnitude != 0f) { return true; }return false; }

    private bool ShouldShowAudioVolume() { if (audioClip != null) { return true; }return false; }

    [Header("Energy Data")]
    [Tooltip("How this form uses it's energy - \n\nUnlimited: This form does not use any energy to be used, only being affected by the ActionCooldown.\n\n" +
        "Cooldown Mag: This form is tied to a magazine amount, which is affected by a cooldown and regen type of your choosing.")]
    public EnergyUsage energyType;
    [ShowIf("HasLimitedAmmo")]
    [Tooltip("How this form regenerates energy - \n\nConstant Full Fill: This form constantly checks the delay " +
        "between your last FormAction() and the present, and if cooldown is reached, fully restores energy.\n\n" +
        "Constant Incremental: This form constantly checks the delay " +
        "between your last FormAction() and the present, and if cooldown is reached, begins regenerating energy based on EnergyRegenRate.\n\n" +
        "Empty Full Fill: This form, upon using all energy, waits for a duration based on EnergyRegenCooldown, and fills all energy.\n\n" +
        "Manual Reload: This allows the FormObject to be manually reloaded by using the R key, fully restoring all energy. Secondary forms using " +
        "the same option will use the primary form's regen rate as the reload timer as well.")]
    public EnergyRegenerationType energyRegenType;
    [ShowIf("HasLimitedAmmo")]
    [Tooltip("Cooldown until energy begins regenerating. (In the case of manual reload, this is your reload time)")]
    public float energyRegenCooldown;
    [ShowIf("ShouldShowRegenRate")]
    [Tooltip("Amount of energy regenerated over time. (Only relevant to energy regen types that actually regen over time)")]
    public float energyRegenRate;
    [ShowIf("HasLimitedAmmo")]
    [Tooltip("Maximum ammount of energy this form can hold.")]
    public float energyMax;
    [ShowIf("HasLimitedAmmo")]
    [Tooltip("Energy used upon FormAction() being run.")]
    public float energyCost;
    [ShowIf("HasLimitedAmmo")]
    [Tooltip("Toggles whether this form should share energy with a secondary form on host FormObject." +
        " (Will use energy stats of Primary Form)")]
    public bool shareEnergyWithOtherForm;
    [ShowIf("ShouldShowAutoReload")]
    [Tooltip("Toggles whether this form should trigger a manual reload on empty.")]
    public bool autoReloadOnEmpty;

    [Header("Screen Shake Impulse")]
    [Tooltip("Amount of shake. Leave at 0 for no shake.")]
    public float screenShakeImpulseMagnitude;
    [ShowIf("ShouldShowShakeDirection")]
    [Tooltip("Direction of camera shake. Leave at 0,0,0 for random direction. Sorry for not having something more complex for spray patterns and whatnot.")]
    public Vector3 screenShakeImpulseDirection;

    [Header("Audio")]
    public AudioClip audioClip;
    [ShowIf("ShouldShowAudioVolume")]
    [Tooltip("Volume of AudioClip played at FormAction().")]
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


