using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Options : MonoBehaviour
{
    [SerializeField] Toggle fullScreenToggle;
    [SerializeField] TextMeshProUGUI resolutionText;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] GameObject parent;
    [SerializeField] AudioMixer mixer;

    const string FULL_SCREEN_SAVE = "fullscreen";
    const string RESOLUTION_WIDTH_SAVE = "resolutionWidth";
    const string RESOLUTION_HEIGHT_SAVE = "resolutionHeight";
    const string REFRESH_RATE_SAVE = "refreshRate";
    const string SOUND_EFFECTS_SAVE = "refreshRate";
    const string MUSIC_SAVE = "refreshRate";

    List<Resolution> resolutions;
    Resolution selectedResolution;
    List<string> resolutionNames = new List<string>();
    int currentResolutionIndex;

    private void Start()
    {
        RemoveDuplicateResolutions();
        LoadSettings();
        CreateResolutionNames();

        fullScreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void RemoveDuplicateResolutions()
    {
        resolutions = new List<Resolution>();
        Resolution lastRes = new Resolution();
        foreach (Resolution res in Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct()) 
        {
            if (res.width != lastRes.width || res.height != lastRes.height)
            {
                resolutions.Add(res);
            }
        }
    }

    void LoadSettings()
    {
        selectedResolution = new Resolution();
        selectedResolution.width = PlayerPrefs.GetInt(RESOLUTION_WIDTH_SAVE, Screen.currentResolution.width);
        selectedResolution.height = PlayerPrefs.GetInt(RESOLUTION_HEIGHT_SAVE, Screen.currentResolution.height);
        selectedResolution.refreshRate = PlayerPrefs.GetInt(REFRESH_RATE_SAVE, Screen.currentResolution.refreshRate);

        float value = PlayerPrefs.GetFloat(SOUND_EFFECTS_SAVE, 1f);
        mixer.SetFloat("SoundEffect", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
        value = PlayerPrefs.GetFloat(MUSIC_SAVE, 1f);
        mixer.SetFloat("Soundtrack", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));

        fullScreenToggle.isOn = PlayerPrefs.GetInt(FULL_SCREEN_SAVE, Screen.fullScreen ? 1 : 0) > 0;

        Screen.SetResolution(
            selectedResolution.width,
            selectedResolution.height,
            fullScreenToggle.isOn
        );
    }

    void CreateResolutionNames()
    {
        for (int i = 0; i < resolutions.Count; i++)
        {
            resolutionNames.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (Mathf.Approximately(resolutions[i].width, selectedResolution.width) && Mathf.Approximately(resolutions[i].height, selectedResolution.height))
            {
                currentResolutionIndex = i;
            }
        }
        resolutionText.text = resolutionNames[currentResolutionIndex];
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(FULL_SCREEN_SAVE, isFullscreen ? 1 : 0);
        SoundsController.instance.PlayOneShot("Click");
    }

    public void IncreaseResolution()
    {
        currentResolutionIndex++;
        if(currentResolutionIndex == resolutions.Count)
        {
            currentResolutionIndex = 0;
        }
        resolutionText.text = resolutionNames[currentResolutionIndex];
        SetResolution(currentResolutionIndex);
        SoundsController.instance.PlayOneShot("Click");
    }

    public void DecreaseResolution()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = resolutions.Count - 1;
        }
        resolutionText.text = resolutionNames[currentResolutionIndex];
        SetResolution(currentResolutionIndex);
        SoundsController.instance.PlayOneShot("Click");
    }

    public void SetResolution(int resolutionIndex)
    {
        selectedResolution = resolutions[resolutionIndex];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(RESOLUTION_WIDTH_SAVE, selectedResolution.width);
        PlayerPrefs.SetInt(RESOLUTION_HEIGHT_SAVE, selectedResolution.height);
        PlayerPrefs.SetInt(REFRESH_RATE_SAVE, selectedResolution.refreshRate);
    }

    public void SoundChanged(float value)
    {
        PlayerPrefs.SetFloat(SOUND_EFFECTS_SAVE,value);
        mixer.SetFloat("SoundEffect", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
    }

    public void MusicChanged(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_SAVE, value);
        mixer.SetFloat("Soundtrack", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
    }

    public void OpenPopup()
    {
        parent.SetActive(true);
    }
}
