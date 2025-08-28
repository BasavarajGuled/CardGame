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
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> clips;

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
    }

    /// <summary>
    /// Resets the game by resetting the card manager and generating new cards.
    /// </summary>
    public void ResetGame()
    {
        cardManager.ResetCards();
        cardGenerator.GenerateCards();
    }

    internal void PlayMusic(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < clips.Count)
        {
            audioSource.clip = clips[clipIndex];
            audioSource.Play();
        }
    }
}

public enum GameState
{
    Easy,
    Medium,
    Hard
}
