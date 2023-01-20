using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerGunAnimator : MonoBehaviour
{
    // TODO: Putting this here cuz I know for a fact you will see this lmao
    //       Be sure to go into weapon_info.json and update the model path
    //       to point to the one you're gonna add
    //
    //       Also be sure to set up a transform at the end of the barrel
    //       for the bullet spawn

    [Header("Animation References")]


    [Space(15)]
    public ParticleSystem steamBarrel;
    public ParticleSystem steamReloadEjection;


    [Header("Audio References")]
    public AudioClip FireMainSound;
    public AudioClip FireSteam;

    public AudioClip ReloadOpen;
    public AudioClip ReloadOpenEjectMag;
    public AudioClip ReloadOpenCaseHitGround;

    public AudioClip ReloadInsertMag;
    public AudioClip ReloadInsertProjectile;

    public AudioClip ReloadLockMechanism;

    public AudioClip ReloadBoltFirst;
    public AudioClip ReloadBoltSecond;


    public void PlayAudio(AudioClip toPlay, float volume = 1f)
    {
        AudioManager.Instance.PlaySoundEffect(toPlay, volume);
    }

    public void PlaySteamEmitter(ParticleSystem toPlay)
    {
        toPlay.Play();
    }
}
