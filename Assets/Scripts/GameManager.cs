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

    internal GameState currentGameState { get; set; } = GameState.Easy;

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
}

public enum GameState
{
    Easy,
    Medium,
    Hard
}
