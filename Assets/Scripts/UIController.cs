using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private Toggle easyModeToggle;
    [SerializeField]
    private Toggle mediumModeToggle;
    [SerializeField]
    private Toggle hardModeToggle;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private CanvasGroup uiCanvasGroup;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        easyModeToggle.onValueChanged.AddListener(OnEasyModeToggleChanged);
        mediumModeToggle.onValueChanged.AddListener(OnMediumModeToggleChanged);
        hardModeToggle.onValueChanged.AddListener(OnHardModeToggleChanged);

        easyModeToggle.isOn = true; // Default to Easy mode

        SetUICanvasState(true);
        CardGenerator.generateCardEvent += SetUICanvasState;
    }

    void OnDestroy()
    {
        CardGenerator.generateCardEvent -= SetUICanvasState;
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.cardGenerator.GenerateCards();
    }

    private void OnEasyModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Easy;
    }

    private void OnMediumModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Medium;
    }

    private void OnHardModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Hard;
    }

    private void SetUICanvasState(bool isActivate)
    {
        uiCanvasGroup.alpha = isActivate ? 1 : 0;
        uiCanvasGroup.interactable = isActivate;
        uiCanvasGroup.blocksRaycasts = isActivate;
    }
}
