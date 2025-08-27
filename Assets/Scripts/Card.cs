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
    public int CardIndex;

    private bool isFliped = false;
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

    internal void ResetCard()
    {
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
