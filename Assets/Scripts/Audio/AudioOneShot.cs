using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOneShot : MonoBehaviour
{

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        if(audioSource.clip == null)
        {
            StartCoroutine(DestroyAudio(0));
        }
        else
        {
            float audioClipLength = audioSource.clip.length;
            StartCoroutine(DestroyAudio(audioClipLength));
        }
        
    }

    public void SetAudioStats(AudioClip clip, float vol, float duration)
    {
        audioSource.clip = clip;
        audioSource.volume = vol;
        if(duration > 0)
        {
            StartCoroutine(DestroyAudio(duration));
        }
        
    }

    public void SetAudioStats(AudioClip clip, float vol)
    {
        audioSource.clip = clip;
        audioSource.volume = vol;
        StartCoroutine(DestroyAudio(clip.length));

    }

    public IEnumerator DestroyAudio(float audioClipLength)
    {
        yield return new WaitForSecondsRealtime(audioClipLength);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
