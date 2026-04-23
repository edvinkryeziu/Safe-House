using System;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class SettingsMenu : MonoBehaviour
{
    public SliderManager masterVolume;
    public SliderManager sfxVolume;
    public SliderManager musicVolume;
    public CustomDropdown resolutionDropDown;
    public CustomDropdown qualityDropDown;
    public Sprite resolutionDropDownSprite;
    public Sprite qualityDropDownSprite;
    public SwitchManager fullscreenSwitch;
    public AudioMixer audioMixer;
    public VolumeProfile[] volumeProfiles;

    private float masterVolumeFloat;
    private float sfxVolumeFloat;
    private float musicVolumeFloat;
    private bool isFullscreen;

    void Start()
    {
        // Set volume so it matches start slider
        float volume = Mathf.Max(masterVolume.mainSlider.value/100f,0.0001f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);

        // Fullscreen
        isFullscreen = true;
        Screen.fullScreen = isFullscreen;

        // Populate resolutions in drop down
        ResolutionPopulation();
        // Set highest resolution at start
        Screen.SetResolution(Screen.resolutions[Screen.resolutions.Length - 1].width, Screen.resolutions[Screen.resolutions.Length - 1].height, isFullscreen);

        // Populate Quality presets
        QualityPopulation();

        ChangeQuality(0);

    }

    public void OnMasterVolumeChange()
    {
        float volume = Mathf.Max(masterVolume.mainSlider.value/100f, 0.0001f);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        masterVolumeFloat = volume;
    }

    public void OnSFXVolumeChange()
    {
        float volume = Mathf.Max(sfxVolume.mainSlider.value/100f, 0.0001f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        sfxVolumeFloat = volume;
    }

    public void OnMusicVolumeChange()
    {
        float volume = Mathf.Max(musicVolume.mainSlider.value/100f, 0.0001f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        musicVolumeFloat = volume;
    }

    private void ResolutionPopulation()
    {
        foreach (Resolution resolution in Screen.resolutions)
        {
            resolutionDropDown.CreateNewItem($"{resolution.width} x {resolution.height}",resolutionDropDownSprite,false);
        }

        resolutionDropDown.SetupDropdown();

        resolutionDropDown.ChangeDropdownInfo(Screen.resolutions.Length - 1);

        resolutionDropDown.onValueChanged.AddListener(ChangeResolution);
        
    }

    private void ChangeResolution(int index)
    {
        Screen.SetResolution(Screen.resolutions[index].width,Screen.resolutions[index].height, isFullscreen);

        Debug.Log($"Changed resolution to: {Screen.resolutions[index].width} x {Screen.resolutions[index].height}");
    }

    private void QualityPopulation()
    {
        foreach (String name in QualitySettings.names)
        {
            qualityDropDown.CreateNewItem(name,qualityDropDownSprite,false);
        }

        qualityDropDown.SetupDropdown();

        qualityDropDown.onValueChanged.AddListener(ChangeQuality);
    }

    private void ChangeQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        Debug.Log($"Set quality to: {QualitySettings.names[index]}");
        PlayerPrefs.SetInt("QualityLevel", index);
    }

    public void OnSwitchFullscreen()
    {
        isFullscreen = !isFullscreen;
        Screen.fullScreen = isFullscreen;

        Debug.Log($"Fullscreen mode: {isFullscreen}");
    }
}
