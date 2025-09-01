using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    internal CardManager cardManager;
    [SerializeField]
    internal UIController uiController;
    [SerializeField]
    internal CardGenerator cardGenerator;
    [SerializeField]
    internal AudioController audioController;

    /// <summary>
    /// Current game state (difficulty level).
    /// </summary>
    internal GameState currentGameState { get; set; } = GameState.Easy;

    [SerializeField]
    internal Sprite[] cardSprites;

    [SerializeField]
    private AudioSource audioSourceMusic;
    [SerializeField]
    private AudioSource audioSourceButtonClick;
    [SerializeField]
    private List<AudioClip> clips;

    public static bool isAutoClicking = false;

    public static string audioSettingPath;
    public static string levelSettingPath;
    public static string savedCardsPath;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        audioSettingPath = Path.Combine(Application.persistentDataPath, "settings.json");
        levelSettingPath = Path.Combine(Application.persistentDataPath, "level.json");
        savedCardsPath = Path.Combine(Application.persistentDataPath, "savedcards.json");
    }

    void Start()
    {
        LoadAudioSetting();
        LoadLevelSetting();
        PlayMusic(0); // Play background music

    }

    /// <summary>
    /// Resets the game by resetting the card manager and generating new cards.
    /// </summary>
    internal void ResetGame()
    {
        cardManager.ResetCounters();
        cardGenerator.ResetCards();
    }

    internal void LoadGame()
    {
        try
        {
            string json = File.ReadAllText(savedCardsPath);
            SavedLevelCards data = JsonUtility.FromJson<SavedLevelCards>(json);

            if (data.gameState == currentGameState && data.savedCard.Count > 0)
                cardGenerator.GenerateSavedCards();
            else
                cardGenerator.GenerateCards();
        }
        catch (Exception e)
        {
            cardGenerator.GenerateCards();
        }
    }

    internal void PlayMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < clips.Count)
        {
            audioSourceMusic.clip = clips[clipIndex];
            audioSourceMusic.Play();
        }
    }

    internal void PlayButtonClickSound()
    {
        if (!isAutoClicking)
            audioSourceButtonClick.PlayOneShot(clips[2]);
    }

    public void SaveLevel()
    {
        SavedLevel level = new SavedLevel();
        level.state = GameManager.Instance.currentGameState;

        string json = JsonUtility.ToJson(level, true);
        File.WriteAllText(GameManager.levelSettingPath, json);
    }

    public void SaveCards()
    {
        cardGenerator.SaveCards();
    }

    public void LoadAudioSetting()
    {
        if (File.Exists(audioSettingPath))
        {
            string json = File.ReadAllText(audioSettingPath);
            GameSetting data = JsonUtility.FromJson<GameSetting>(json);
            Debug.Log("Loaded: " + json);
            audioController.musicSlider.value = data.Music;
            audioController.vfxSlider.value = data.VFX;
        }
        else
        {
            Debug.LogWarning("No save file found, returning 0");
            audioController.musicSlider.value = 0.5f;
            audioController.vfxSlider.value = 0.5f;
        }
    }

    public void LoadLevelSetting()
    {
        if (File.Exists(levelSettingPath))
        {
            string json = File.ReadAllText(levelSettingPath);
            SavedLevel data = JsonUtility.FromJson<SavedLevel>(json);
            Debug.Log("Loaded: " + json);

            isAutoClicking = true;
            if (data.state == GameState.Easy)
                uiController.easyModeToggle.isOn = true;
            else if (data.state == GameState.Medium)
                uiController.mediumModeToggle.isOn = true;
            else if (data.state == GameState.Hard)
                uiController.hardModeToggle.isOn = true;
            isAutoClicking = false;
        }
        else
        {
            Debug.LogWarning("No save file found, returning 0");
            isAutoClicking = true;
            uiController.easyModeToggle.isOn = true;
            isAutoClicking = false;
        }
    }

    public void LoadSavedCards()
    {
        if (File.Exists(savedCardsPath))
        {
            string json = File.ReadAllText(savedCardsPath);
            GameSetting data = JsonUtility.FromJson<GameSetting>(json);
            Debug.Log("Loaded: " + json);

        }
        else
        {
            Debug.LogWarning("No save file found, returning 0");

        }
    }
}

public enum GameState
{
    Easy,
    Medium,
    Hard
}

[Serializable]
public class GameSetting
{
    public float Music;
    public float VFX;
}

[Serializable]
public class SavedLevel
{
    public GameState state;
}

[Serializable]
public class SavedLevelCards
{
    public GameState gameState;
    public int matchCount;
    public int turnCount;
    public List<SavedCard> savedCard;
}

[Serializable]
public class SavedCard
{
    public int holderIndex;
    public int cardIndex;
}


