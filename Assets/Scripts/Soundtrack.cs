using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Soundtrack : MonoBehaviour
{
    public static Soundtrack instance;

    [SerializeField] AudioSource baseAudio;
    [SerializeField] AudioSource idleAudio;
    [SerializeField] AudioSource battleAudio;

    [SerializeField] AudioClip start;
    [SerializeField] AudioClip end;

    float startingVolumeValue;

    private void Awake()
    {
        if (FindObjectsOfType<Soundtrack>().Length > 1)
        {
            foreach (Soundtrack soundtrack in FindObjectsOfType<Soundtrack>())
            {
                if (soundtrack != this)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        ChangedSoundtrackVolume();
    }

    public void ChangedSoundtrackVolume()
    {
        baseAudio.volume = 1f * SettingsHolder.music;
    }

    public void GameStarted()
    {
        idleAudio.volume = 1f * SettingsHolder.music;
    }

    public void MenuScreen()
    {
        idleAudio.volume = 0f;
        battleAudio.volume = 0f;
    }

    public void BattleStart()
    {
        baseAudio.PlayOneShot(start);
        StartCoroutine(ChangeTheme(idleAudio, battleAudio));
    }

    public void BattleEnd()
    {
        baseAudio.PlayOneShot(end);
        StartCoroutine(ChangeTheme(battleAudio, idleAudio));
    }

    IEnumerator ChangeTheme(AudioSource from, AudioSource to)
    {
        float timePassed = 1f;
        while (timePassed > 0)
        {
            timePassed -= Time.unscaledDeltaTime;
            from.volume = SettingsHolder.music * timePassed;
            to.volume = SettingsHolder.music * (1 - timePassed);
            yield return null;
        }

        from.volume = 0f;
        to.volume = 1f;
    }
}
