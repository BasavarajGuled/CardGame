using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource musicAudioSource;
    [SerializeField]
    private AudioSource clickGamePlayAudioSources;

    [SerializeField]
    internal Slider musicSlider;
    [SerializeField]
    internal Slider vfxSlider;
    void Awake()
    {
        AssignListener();
    }

    private void AssignListener()
    {
        musicSlider.onValueChanged.AddListener(delegate
        {
            OnMusicSliderChanged(musicSlider.value);
        });

        vfxSlider.onValueChanged.AddListener(delegate
        {
            OnVfxSliderChanged(vfxSlider.value);
        });
    }

    private void OnMusicSliderChanged(float value)
    {
        musicAudioSource.volume = value;

        GameSetting settings = new GameSetting();
        settings.Music = value;
        settings.VFX = clickGamePlayAudioSources.volume;

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(GameManager.audioSettingPath, json);

        Debug.Log("Saved: " + json);
    }

    private void OnVfxSliderChanged(float value)
    {

        clickGamePlayAudioSources.volume = value;

        GameSetting settings = new GameSetting();
        settings.VFX = value;
        settings.Music = musicAudioSource.volume;

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(GameManager.audioSettingPath, json);

        Debug.Log("Saved: " + json);
    }
}
