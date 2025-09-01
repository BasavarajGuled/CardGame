using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    /// <summary>
    /// Event triggered when cards are generated or the UI state needs to change.
    /// </summary>
    public static event GenerateCardEvent generateCardEvent;

    public int totalMatchCount;

    [SerializeField]
    private List<Card> cards;
    private Coroutine animateCardsCoroutine;

    /// <summary>
    /// Generates cards based on the current game state.
    /// </summary>
    public void GenerateCards()
    {
        if (GameManager.Instance.currentGameState == GameState.Easy)
        {
            EasyMode();
            generateCardEvent?.Invoke(false);
            animateCardsCoroutine = StartCoroutine(AnimateCards(0.5f));
        }
        else if (GameManager.Instance.currentGameState == GameState.Medium)
        {
            MediumMode();
            generateCardEvent?.Invoke(false);
            animateCardsCoroutine = StartCoroutine(AnimateCards(0.5f));
        }
        else if (GameManager.Instance.currentGameState == GameState.Hard)
        {
            HardMode();
            generateCardEvent?.Invoke(false);
            animateCardsCoroutine = StartCoroutine(AnimateCards());
        }

    }

    /// <summary>
    /// Handles the Easy mode card generation.
    /// </summary>
    private void EasyMode()
    {
        // Implement Easy mode card generation
        SetCardParent(0);
        LoadCardParent(4);
        LoadCards();
    }

    /// <summary>
    /// Handles the Medium mode card generation.
    /// </summary>
    private void MediumMode()
    {
        // Implement Medium mode card generation
        SetCardParent(1);
        LoadCardParent(6);
        LoadCards();
    }

    /// <summary>
    /// Handles the Hard mode card generation.
    /// </summary>
    private void HardMode()
    {
        SetCardParent(2);
        LoadCardParent(30);
        LoadCards();
        // SetCardParent(0);
        // LoadCardParent(4);
        // LoadCards();
    }

    /// <summary>
    /// Sets the active card parent based on the provided index.
    /// </summary>
    /// <param name="index"></param>
    private void SetCardParent(int index)
    {
        for (int i = 0; i < cardParents.Length; i++)
        {
            cardParents[i].gameObject.SetActive(false);
        }
        cardParents[index].gameObject.SetActive(true);
        currentCardParent = cardParents[index];
    }

    /// <summary>
    /// Loads card parent transforms and sets the total match count.
    /// </summary>
    /// <param name="Count"></param>
    private void LoadCardParent(int Count)
    {
        totalMatchCount = Count / 2;
        for (int i = 0; i < Count; i++)
        {
            Transform cardTransform = Instantiate(cardHolder, currentCardParent).transform;
            cardTransform.gameObject.name = i.ToString();
            cardTransforms.Add(cardTransform);
        }
    }

    /// <summary>
    /// Loads and instantiates cards, assigning them random indices for matching.
    /// </summary>
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
            cards.Add(card);
            card.SetCardIndex(availableIndices[i]);
            card.gameObject.name = availableIndices[i].ToString();
            //card.transform.parent.name = availableIndices[i].ToString() + "_" + i; //debugging
        }
    }

    /// <summary>
    /// Animates the cards by flipping them for a specified delay.
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    private IEnumerator AnimateCards(float delay = 2.0f)
    {
        yield return new WaitForSeconds(0.5f);
        foreach (var card in cards)
        {
            card.IsFliped = true;
        }
        yield return new WaitForSeconds(delay);
        foreach (var card in cards)
        {
            card.IsFliped = false;
        }
        if (animateCardsCoroutine != null)
            StopCoroutine(animateCardsCoroutine);
    }

    /// <summary>
    /// Resets the cards by clearing the current card parent and deactivating all card parents.
    /// </summary>
    public void ResetCards()
    {
        cards.Clear();
        if (currentCardParent == null) return;
        for (int i = 0; i < cardParents.Length; i++)
        {
            cardParents[i].gameObject.SetActive(false);
        }
        foreach (Transform child in currentCardParent.transform)
        {
            Destroy(child.gameObject);
        }
        cardTransforms.Clear();
    }

    internal void SaveCards()
    {
        SavedLevelCards savedLevelCards = new SavedLevelCards();

        savedLevelCards.matchCount = GameManager.Instance.cardManager.matchCounter;
        savedLevelCards.turnCount = GameManager.Instance.cardManager.turnCounter;
        savedLevelCards.gameState = GameManager.Instance.currentGameState;

        savedLevelCards.savedCard = new List<SavedCard>();

        int i = 0;
        foreach (Transform child in cardTransforms)
        {
            SavedCard card = new SavedCard();
            card.holderIndex = int.Parse(child.gameObject.name);
            if (child.childCount > 0)
                card.cardIndex = int.Parse(child.GetChild(0).gameObject.name);
            else
                card.cardIndex = -1;
            savedLevelCards.savedCard.Add(card);

            i++;
        }

        string json = JsonUtility.ToJson(savedLevelCards, true);
        File.WriteAllText(GameManager.savedCardsPath, json);

        Debug.Log("Saved: " + json);
    }

    public void GenerateSavedCards()
    {
        if (GameManager.Instance.currentGameState == GameState.Easy)
        {
            SetCardParent(0);
            LoadCardParent(4);
            generateCardEvent?.Invoke(false);
        }
        else if (GameManager.Instance.currentGameState == GameState.Medium)
        {
            SetCardParent(1);
            LoadCardParent(6);
            generateCardEvent?.Invoke(false);
        }
        else if (GameManager.Instance.currentGameState == GameState.Hard)
        {
            SetCardParent(2);
            LoadCardParent(30);
            generateCardEvent?.Invoke(false);
        }
        LoadSavedCards();
    }

    private void LoadSavedCards()
    {
        string json = File.ReadAllText(GameManager.savedCardsPath);
        SavedLevelCards data = JsonUtility.FromJson<SavedLevelCards>(json);

        GameManager.Instance.cardManager.matchCounter = data.matchCount;
        GameManager.Instance.cardManager.turnCounter = data.turnCount;

        GameManager.Instance.cardManager.SetMatchTurnCount();

        for (int i = 0; i < cardTransforms.Count; i++)
        {
            if (data.savedCard[i].cardIndex != -1)
            {
                Card card = Instantiate(cardPrefab, cardTransforms[i]);
                cards.Add(card);
                card.SetCardIndex(data.savedCard[i].cardIndex);
                card.gameObject.name = data.savedCard[i].cardIndex.ToString();
                //card.transform.parent.name = availableIndices[i].ToString() + "_" + i; //debugging
            }
        }
    }

}
