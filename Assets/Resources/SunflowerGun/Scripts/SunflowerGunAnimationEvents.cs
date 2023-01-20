using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerGunAnimationEvents : MonoBehaviour
{
    [Header("Animator")]
    public SunflowerGunAnimator animator;


    [Header("Audio References")]
    public AudioClip FireMain;
    public AudioClip FireSteam;
    [Space(15)]
    public AudioClip ReloadOpen;
    public AudioClip ReloadOpenEjectMag;
    public AudioClip ReloadOpenCaseHitGround;
    [Space(15)]
    public AudioClip ReloadInsertMag;
    public AudioClip ReloadInsertProjectile;
    [Space(15)]
    public AudioClip ReloadLockMechanism;
    [Space(15)]
    public AudioClip ReloadBoltFirst;
    public AudioClip ReloadBoltSecond;


    public void PlayFireMainSound()
    {
        animator.PlayAudio(FireMain);
    }
    public void PlayFireSteamSound()
    {
        animator.PlayAudio(FireSteam);
    }

    
    public void PlayReloadOpenSound()
    {
        animator.PlayAudio(ReloadOpen);
    }
    public void PlayReloadOpenEjectMagSound()
    {
        animator.PlayAudio(ReloadOpenEjectMag);
    }
    public void PlayReloadOpenCaseHitGroundSound()
    {
        animator.PlayAudio(ReloadOpenCaseHitGround);
    }


    public void PlayReloadInsertMagSound()
    {
        animator.PlayAudio(ReloadInsertMag);
    }
    public void PlayReloadInsertProjectileSound()
    {
        animator.PlayAudio(ReloadInsertProjectile);
    }


    public void PlayReloadLockMechanismSound()
    {
        animator.PlayAudio(ReloadLockMechanism);
    }


    public void PlayReloadBoltFirstSound()
    {
        animator.PlayAudio(ReloadBoltFirst);
    }
    public void PlayReloadBoltSecondSound()
    {
        animator.PlayAudio(ReloadBoltSecond);
    }
}
