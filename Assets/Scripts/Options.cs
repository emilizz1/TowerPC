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
        selectedResolution.width = SavedData.savesData.resolutionWidth;
        selectedResolution.height = SavedData.savesData.resolutionHeight;
        selectedResolution.refreshRate = SavedData.savesData.refreshRate;

        float value = SavedData.savesData.soundEffects;
        mixer.SetFloat("SoundEffect", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
        soundSlider.value = value;
        value = SavedData.savesData.music;
        mixer.SetFloat("Soundtrack", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
        musicSlider.value = value;

        fullScreenToggle.isOn = (SavedData.savesData.fullscreen > 0);

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
        SavedData.savesData.fullscreen = isFullscreen ? 1 : 0;
        SavedData.Save();
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

        SavedData.savesData.resolutionWidth = selectedResolution.width;
        SavedData.savesData.resolutionHeight = selectedResolution.height;
        SavedData.savesData.refreshRate = selectedResolution.refreshRate;
        SavedData.Save();
    }

    public void SoundChanged(float value)
    {
        SavedData.savesData.soundEffects = value;
        SavedData.Save();
        mixer.SetFloat("SoundEffect", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
    }

    public void MusicChanged(float value)
    {
        SavedData.savesData.music = value;
        SavedData.Save();
        mixer.SetFloat("Soundtrack", value > 0.5f ? Mathf.Lerp(-40f, 0f, value) : Mathf.Lerp(-80f, 40f, value));
    }

    public void OpenPopup()
    {
        Time.timeScale = 0f;
        parent.SetActive(true);
    }

    public void ClosePopup()
    {
        Time.timeScale = SpeedButton.instance.currentSpeed;
        parent.SetActive(false);        
    }
}
