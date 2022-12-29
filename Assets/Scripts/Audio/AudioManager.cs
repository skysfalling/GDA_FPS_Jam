using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : UnitySingleton<AudioManager>
{
    [SerializeField] private GameObject audioOneShotPrefab;

    [Serializable]
    public struct SoundEffect
    {
        public string name;
        public List<AudioClip> clip;
    }

    private void Update()
    {
        
    }

    public List<SoundEffect> soundEffectList = new List<SoundEffect>();

    public void PlaySoundEffect(string effectName, Vector3 position, float volume, float duration)
    {
        foreach (SoundEffect SE in soundEffectList)
        {
            if (SE.name == effectName)
            {
                if (SE.clip.Count == 0 || SE.clip[0] == null) return;

                var oneShot = Instantiate(audioOneShotPrefab, position, Quaternion.identity);

                AudioClip selectedClip = SE.clip[UnityEngine.Random.Range(0, SE.clip.Count)];

                oneShot.GetComponent<AudioOneShot>().SetAudioStats(selectedClip, volume, duration);
                return;
            }
        }

        CheckAndPlaySFXFromResources(effectName, volume);

    }


    public void PlaySoundEffect(string effectName, float volume)
    {
        foreach (SoundEffect SE in soundEffectList)
        {
            if (SE.name == effectName)
            {
                if (SE.clip.Count == 0 || SE.clip[0] == null) return;
                
                var oneShot = Instantiate(audioOneShotPrefab, Camera.main.transform.position, Quaternion.identity);

                AudioClip selectedClip = SE.clip[UnityEngine.Random.Range(0, SE.clip.Count)];

                oneShot.GetComponent<AudioOneShot>().SetAudioStats(selectedClip, volume);
                return;
            }
        }

        CheckAndPlaySFXFromResources(effectName, volume);
    }


    void CheckAndPlaySFXFromResources(string id, float volume)
    {
        AudioClip clip = Resources.Load(id) as AudioClip;
        if (clip == null)
        {
            Debug.Log("sound effect not found! (" + id + ")");
            return;
        }
        Debug.Log("found sfx in resources! (" + id + ")");
        var oneShot = Instantiate(audioOneShotPrefab, Camera.main.transform.position, Quaternion.identity);

        oneShot.GetComponent<AudioOneShot>().SetAudioStats(clip, volume);

        
    }
}
