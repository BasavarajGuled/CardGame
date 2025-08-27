using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGenerator : MonoBehaviour
{
    [SerializeField]
    private Card cardPrefab;
    [SerializeField]
    private GameObject cardHolder;

    [SerializeField]
    private List<Transform> cardTransforms;

    [SerializeField]
    private Transform[] cardParents;

    private Transform currentCardParent;

    public delegate void GenerateCardEvent(bool isActivate);
    public static event GenerateCardEvent generateCardEvent;

    public void GenerateCards()
    {
        if (GameManager.Instance.currentGameState == GameState.Easy)
        {
            EasyMode();
            generateCardEvent?.Invoke(false);
        }
        else if (GameManager.Instance.currentGameState == GameState.Medium)
        {
            MediumMode();
            generateCardEvent?.Invoke(false);
        }
        else if (GameManager.Instance.currentGameState == GameState.Hard)
        {
            HardMode();
            generateCardEvent?.Invoke(false);
        }
    }

    private void EasyMode()
    {
        // Implement Easy mode card generation
        SetCardParent(0);
        LoadCardParent(4);
        LoadCards();
    }

    private void MediumMode()
    {
        // Implement Medium mode card generation
        SetCardParent(1);
        LoadCardParent(6);
        LoadCards();
    }

    private void HardMode()
    {
        SetCardParent(2);
        LoadCardParent(30);
        LoadCards();
    }

    private void SetCardParent(int index)
    {
        foreach (Transform child in cardHolder.transform)
        {
            child.gameObject.SetActive(false);
        }
        cardParents[index].gameObject.SetActive(true);
        currentCardParent = cardParents[index];
    }

    private void LoadCardParent(int Count)
    {
        for (int i = 0; i < Count; i++)
        {
            Transform cardTransform = Instantiate(cardHolder, currentCardParent).transform;
            cardTransforms.Add(cardTransform);
        }
    }

    private void LoadCards()
    {
        List<int> availableIndices = new List<int>();
        for (int i = 0; i < cardTransforms.Count / 2; i++)
        {
            availableIndices.Add(i);
            availableIndices.Add(i);
        }

        // System.Random rng = new System.Random();
        // while (availableIndices.Count > 0)
        // {
        //     int randomIdx = rng.Next(availableIndices.Count);
        //     int cardTransformIndex = availableIndices[randomIdx];
        //     Card card = Instantiate(cardPrefab, cardTransforms[randomIdx]);
        //     card.CardIndex = cardTransformIndex;
        //     Debug.Log($"Card generated: {card.CardIndex}");
        //     availableIndices.RemoveAt(randomIdx);
        // }

        // Shuffle availableIndices
        for (int i = 0; i < availableIndices.Count; i++)
        {
            int rand = Random.Range(i, availableIndices.Count);
            int temp = availableIndices[i];
            availableIndices[i] = availableIndices[rand];
            availableIndices[rand] = temp;
        }

        // Instantiate cards
        for (int i = 0; i < availableIndices.Count; i++)
        {
            Card card = Instantiate(cardPrefab, cardTransforms[i]);
            card.CardIndex = availableIndices[i];
            card.transform.parent.name = availableIndices[i].ToString(); //debugging
        }
    }

}
