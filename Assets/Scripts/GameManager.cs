using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private CardManager cardManager;
    [SerializeField]
    private UIController uiController;
    public UIController UIController => uiController;
    [SerializeField]
    internal CardGenerator cardGenerator;

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
        cardGenerator.GenerateCards();
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
}

public enum GameState
{
    Easy,
    Medium,
    Hard
}
