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
    [SerializeField]
    private int cardIndex;
    public int CardIndex => cardIndex;

    private bool isFliped = false;
    public bool IsFliped
    {
        get { return isFliped; }
        set
        {
            isFliped = value;
            if (isFliped)
                cardButton.interactable = false;
            else
                cardButton.interactable = true;
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
            flipingCard.localScale = Vector3.one;
            IsFliped = true;
            cardManager.CardFliped();
            addToManagerCoroutine = StartCoroutine(AddToManager());
        }
        else
        {
            flipingCard.localScale = Vector3.zero;
            IsFliped = false;
        }
    }

    internal void ResetCard()
    {
        flipingCard.localScale = Vector3.zero;
        IsFliped = false;
    }

    private IEnumerator AddToManager()
    {
        yield return new WaitForSeconds(0.5f);
        cardManager.AddCard(this);
        if (addToManagerCoroutine != null)
        {
            StopCoroutine(addToManagerCoroutine);
        }
    }

}
