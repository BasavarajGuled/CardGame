using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI matchCount;
    private int matchCounter = 0;
    [SerializeField]
    private TMPro.TextMeshProUGUI turnCount;
    private int turnCounter = 0;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> clips;
    public List<Card> cards;

    private Coroutine showScreenCoroutine;

    #region Debugging
    // public GraphicRaycaster raycaster;   // Assign your Canvas's GraphicRaycaster
    // public EventSystem eventSystem;      // Assign your EventSystem
    // private PointerEventData pointerEventData;
    // private List<RaycastResult> results = new List<RaycastResult>();
    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0)) // Left click
    //     {
    //         // Create new PointerEventData
    //         pointerEventData = new PointerEventData(eventSystem)
    //         {
    //             position = Input.mousePosition
    //         };

    //         // Clear old results
    //         results.Clear();

    //         // Raycast into UI
    //         raycaster.Raycast(pointerEventData, results);

    //         // Check what was hit
    //         foreach (RaycastResult result in results)
    //         {
    //             Debug.Log("Clicked on: " + result.gameObject.name);
    //             // 👉 You can also check tags, components, etc.
    //         }
    //     }
    // }
    #endregion

    /// <summary>
    /// Plays an audio clip based on the provided index.
    /// </summary>
    /// <param name="clipIndex"></param>
    public void PlayClip(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < clips.Count)
        {
            audioSource.clip = clips[clipIndex];
            audioSource.Play();
        }
    }

    /// <summary>
    /// Handles the event when a match is found between two cards.
    /// </summary>
    public void MatchFound()
    {
        PlayClip(0); // Play a sound for a match found\
        MatchCounter();
        TurnCounter();
        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
    }

    /// <summary>
    /// Handles the event when no match is found between two cards.
    /// </summary>
    public void NoMatchFound()
    {
        PlayClip(1); // Play a sound for no match found
        TurnCounter();
        foreach (var card in cards)
        {
            card.ResetCard();
        }
    }

    /// <summary>
    /// Handles the event when a card is flipped.
    /// </summary>
    internal void CardFliped()
    {
        PlayClip(2);
    }

    /// <summary>
    /// Adds a card to the current selection and checks for matches if two cards are selected.
    /// </summary>
    /// <param name="card"></param>
    public void AddCard(Card card)
    {
        if (!cards.Contains(card) && cards.Count < 2)
        {
            cards.Add(card);
        }

        if (cards.Count == 2)
        {
            // Check for a match
            if (cards[0].cardIndex == cards[1].cardIndex)
            {
                MatchFound();
            }
            else
            {
                NoMatchFound();
            }

            // Clear the selected cards
            cards.Clear();
        }
    }

    /// <summary>
    /// Increments the match counter and checks if the game state needs to be updated. If all matches are found, it triggers the appropriate screen based on the current game state.
    /// </summary>
    private void MatchCounter()
    {
        matchCounter++;
        matchCount.text = matchCounter.ToString();

        if (matchCounter == GameManager.Instance.cardGenerator.totalMatchCount)
        {
            if (GameManager.Instance.currentGameState == GameState.Easy)
            {
                GameManager.Instance.currentGameState = GameState.Medium;
                showScreenCoroutine = StartCoroutine(ShowScreenAfterDelay(1.0f, false, true, false));
            }
            else if (GameManager.Instance.currentGameState == GameState.Medium)
            {
                GameManager.Instance.currentGameState = GameState.Hard;
                showScreenCoroutine = StartCoroutine(ShowScreenAfterDelay(1.0f, false, true, false));
            }
            else if (GameManager.Instance.currentGameState == GameState.Hard)
            {
                GameManager.Instance.currentGameState = GameState.Easy;
                showScreenCoroutine = StartCoroutine(ShowScreenAfterDelay(1.0f, false, false, true));
            }
        }
    }

    /// <summary>
    /// Increments the turn counter and updates the UI.
    /// </summary>
    private void TurnCounter()
    {
        turnCounter++;
        turnCount.text = turnCounter.ToString();
    }

    /// <summary>
    /// Shows the specified screen after a delay.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="showHome"></param>
    /// <param name="showNext"></param>
    /// <param name="showGameOver"></param>
    /// <returns></returns>
    private IEnumerator ShowScreenAfterDelay(float delay, bool showHome, bool showNext, bool showGameOver)
    {
        yield return new WaitForSeconds(delay);
        GameManager.Instance.UIController.ShowScreen(showHome, showNext, showGameOver);
        if (showScreenCoroutine != null)
        {
            StopCoroutine(showScreenCoroutine);
        }
    }

    /// <summary>
    /// Resets the game state, including match and turn counters, and clears the list of cards.
    /// </summary>
    public void ResetCards()
    {
        matchCounter = 0;
        turnCounter = 0;
        matchCount.text = matchCounter.ToString();
        turnCount.text = turnCounter.ToString();
        cards.Clear();
    }
}
