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
    private Slider musicSlider;
    [SerializeField]
    private Slider vfxSlider;

    private string savePath;

    void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "settings.json");
        AssignListener();
        LoadScore();
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
        File.WriteAllText(savePath, json);

        Debug.Log("Saved: " + json);
    }

    private void OnVfxSliderChanged(float value)
    {

        clickGamePlayAudioSources.volume = value;

        GameSetting settings = new GameSetting();
        settings.VFX = value;
        settings.Music = musicAudioSource.volume;

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved: " + json);
    }

    public void LoadScore()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameSetting data = JsonUtility.FromJson<GameSetting>(json);
            Debug.Log("Loaded: " + json);
            musicSlider.value = data.Music;
            vfxSlider.value = data.VFX;
        }
        else
        {
            Debug.LogWarning("No save file found, returning 0");
            musicSlider.value = 0.5f;
            vfxSlider.value = 0.5f;
        }
    }
}

[Serializable]
public class GameSetting
{
    public float Music;
    public float VFX;
}
