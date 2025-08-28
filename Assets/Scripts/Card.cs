using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [SerializeField]
    private Transform flipingCard;
    [SerializeField]
    private Button cardButton;
    public int cardIndex;

    [SerializeField]
    private Image cardImage;

    private bool isFliped = false;

    /// <summary>
    /// Indicates whether the card is flipped. When set, it updates the visual state and interactivity of the card.
    /// </summary>
    public bool IsFliped
    {
        get { return isFliped; }
        set
        {
            isFliped = value;
            if (isFliped)
            {
                flipingCard.localScale = Vector3.one;
                cardButton.interactable = false;
            }
            else
            {
                flipingCard.localScale = Vector3.zero;
                cardButton.interactable = true;
            }
        }
    }

    private CardManager cardManager;
    private Coroutine addToManagerCoroutine;

    private void Awake()
    {
        cardButton.onClick.AddListener(OnCardClicked);
        IsFliped = false;
        cardManager = FindObjectOfType<CardManager>();

    }

    private void OnCardClicked()
    {
        if (!isFliped)
        {
            IsFliped = true;
            cardManager.CardFliped();
            addToManagerCoroutine = StartCoroutine(AddToManager());
        }
        else
        {

            IsFliped = false;
        }
    }

    /// <summary>
    /// Resets the card to its initial unflipped state.
    /// </summary>
    internal void ResetCard()
    {
        IsFliped = false;
    }

    /// <summary>
    /// Adds the card to the CardManager after a short delay to ensure the flip animation is visible.
    /// </summary>
    /// <returns></returns>
    private IEnumerator AddToManager()
    {
        yield return new WaitForSeconds(0.5f);
        cardManager.AddCard(this);
        if (addToManagerCoroutine != null)
        {
            StopCoroutine(addToManagerCoroutine);
        }
    }

    public void SetCardIndex(int index)
    {
        cardIndex = index;
        cardImage.sprite = GameManager.Instance.cardSprites[cardIndex];
    }

}
