using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundsController : MonoSingleton<SoundsController>
{
    [SerializeField] List<SoundClip> allSounds;
    [SerializeField] AudioSource mainSource;

    [Serializable]
    struct SoundClip
    {
        public string soundName;
        public AudioClip clip;
    }

    public void PlayOnce(string audioSourceName)
    {
        AudioClip audioClip = GetAudioClip(audioSourceName);
        if (audioClip != null)
        {
            if (!mainSource.isPlaying)
            {
                mainSource.clip = audioClip;
                mainSource.Play();
            }
        }
    }

    public void PlayOneShot(string audioSourceName)
    {
        AudioClip audioClip = GetAudioClip(audioSourceName);
        if (audioClip != null)
        {
            mainSource.PlayOneShot(audioClip);
        }
    }

    public AudioClip GetAudioClip(string audioName)
    {
        foreach (SoundClip audioClip in allSounds)
        {
            if (audioClip.soundName == audioName)
            {
                return audioClip.clip;
            }
        }
        return null;
    }

    public void StopPlayOnce(string audioSourceName)
    {
        AudioClip audioClip = GetAudioClip(audioSourceName);
        if(audioClip != null)
        {
            if (mainSource.isPlaying)
            {
                if(mainSource.clip == audioClip)
                {
                    mainSource.Stop();
                }
            }
        }
    }
}
