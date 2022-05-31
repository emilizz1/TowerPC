using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoSingleton<SoundsController>
{
    [SerializeField] List<AudioClip> allSounds;
    [SerializeField] AudioSource mainSource;
    [SerializeField] List<AudioClip> workerSelectedSounds;
    [SerializeField] string startSound;

    private void Start()
    {
        PlayOneShot(startSound);
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

    AudioClip GetAudioClip(string audioName)
    {
        foreach (AudioClip audioClip in allSounds)
        {
            if (audioClip.name == audioName)
            {
                return audioClip;
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

    public void PlayWorkerSelected()
    {
        mainSource.PlayOneShot(workerSelectedSounds[Random.Range(0, workerSelectedSounds.Count)]);
    }
}
