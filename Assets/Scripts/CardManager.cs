using System;
using System.Collections.Generic;
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

    public void PlayClip(int clipIndex)
    {
        if (clipIndex >= 0 && clipIndex < clips.Count)
        {
            audioSource.clip = clips[clipIndex];
            audioSource.Play();
        }
    }

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

    public void NoMatchFound()
    {
        PlayClip(1); // Play a sound for no match found
        TurnCounter();
        foreach (var card in cards)
        {
            card.ResetCard();
        }
    }

    internal void CardFliped()
    {
        PlayClip(2);
    }

    public void AddCard(Card card)
    {
        if (!cards.Contains(card) && cards.Count < 2)
        {
            cards.Add(card);
        }

        if (cards.Count == 2)
        {
            // Check for a match
            if (cards[0].CardIndex == cards[1].CardIndex)
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

    private void MatchCounter()
    {
        matchCounter++;
        matchCount.text = matchCounter.ToString();
    }

    private void TurnCounter()
    {
        turnCounter++;
        turnCount.text = turnCounter.ToString();
    }

}
