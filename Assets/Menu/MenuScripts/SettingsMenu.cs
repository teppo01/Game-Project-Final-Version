using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using System;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    Resolution[] screenResolutions;

    [Header("Sliders")]
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider effectsVolumeSlider;
    public Slider sensSlider;

    [Header("Other")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown ResolutionDropdown;
    void Start()
    {
        screenResolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> resolutionOptions = new();
        for (int i=0; i < screenResolutions.Length; i++)
        {
            string option = screenResolutions[i].width + "x" + screenResolutions[i].height + "@" + screenResolutions[i].refreshRate + "hz";
            resolutionOptions.Add(option);
        }
        ResolutionDropdown.AddOptions(resolutionOptions);

        mainVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        effectsVolumeSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
        sensSlider.value = PlayerPrefs.GetFloat("mouseSensitivity");

        int resolution = PlayerPrefs.GetInt("resolution");
        SetResolution(resolution);
        ResolutionDropdown.value = resolution;

        int quality = PlayerPrefs.GetInt("qualityLevel");
        SetQuality(quality);
        qualityDropdown.value = quality;

        // tää pitää testata buildissa kuten muutki settingsit, toimii oudosti editorissa
        bool fullscreen = Convert.ToBoolean(PlayerPrefs.GetInt("fullscreen"));
        SetFullscreen(fullscreen);
        fullscreenToggle.isOn = fullscreen;
    }

    public void SetResolution (int resolution)
    {
        PlayerPrefs.SetInt("resolution", resolution);
        Resolution res = screenResolutions[resolution];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMasterVolume (float volume)
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        audioMixer.SetFloat("MasterVolume",  Mathf.Log10(volume)*20);
    }
    public void SetMusicVolume (float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        audioMixer.SetFloat("MusicVolume",  Mathf.Log10(volume)*20);
    }
    public void SetEffectsVolume (float volume)
    {
        PlayerPrefs.SetFloat("EffectsVolume", volume);
        audioMixer.SetFloat("EffectsVolume",  Mathf.Log10(volume)*20);
    }

    public void SetQuality (int quality)
    {
        PlayerPrefs.SetInt("qualityLevel", quality);
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullscreen (bool fullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", fullscreen ? 1 : 0);
        Screen.fullScreen = fullscreen;
    }

    public void SetSensitivity (float sens)
    {
        // tarvii jonkinlaisen laskukaavan hiiren nopeuteen
        PlayerPrefs.SetFloat("mouseSensitivity", sens);
        PlayerCamera.mouseSensitivity = sens;
    }

}
