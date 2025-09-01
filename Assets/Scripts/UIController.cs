using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    internal Toggle easyModeToggle;
    [SerializeField]
    internal Toggle mediumModeToggle;
    [SerializeField]
    internal Toggle hardModeToggle;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button nextButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Button settingButton;
    [SerializeField]
    private Button settingBackButton;
    private bool isSetting = false;

    [SerializeField]
    private Button reload;
    [SerializeField]
    private Button save;

    [SerializeField]
    private CanvasGroup uiCanvasGroup;
    [SerializeField]
    private GameObject homeScreenPanel;
    [SerializeField]
    private GameObject nextScreenPanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject settingPanel;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        easyModeToggle.onValueChanged.AddListener(OnEasyModeToggleChanged);
        mediumModeToggle.onValueChanged.AddListener(OnMediumModeToggleChanged);
        hardModeToggle.onValueChanged.AddListener(OnHardModeToggleChanged);

        nextButton.onClick.AddListener(OnNextScreenButtonClicked);
        homeButton.onClick.AddListener(() =>
        {
            ShowScreen(true, false, false);
            GameManager.Instance.PlayButtonClickSound(); // Play button click sound
            SetToggleState();
            GameManager.Instance.ResetGame();
            GameManager.Instance.PlayMusic(0); // Play game over music
        });
        settingButton.onClick.AddListener(() => OnSettingClick());
        settingBackButton.onClick.AddListener(() => OnSettingClick());
        reload.onClick.AddListener(() => OnReloadClick());
        save.onClick.AddListener(() => OnSaveClick());

        ShowScreen(true, false, false);
        CardGenerator.generateCardEvent += SetUICanvasState;
    }

    void OnDestroy()
    {
        CardGenerator.generateCardEvent -= SetUICanvasState;
    }

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.LoadGame();
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
    }

    private void OnEasyModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Easy;
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
    }

    private void OnMediumModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Medium;
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
    }

    private void OnHardModeToggleChanged(bool arg0)
    {
        GameManager.Instance.currentGameState = GameState.Hard;
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
    }

    /// <summary>
    /// Sets the UI canvas state (visible or hidden).
    /// </summary>
    /// <param name="isActivate"></param>
    private void SetUICanvasState(bool isActivate)
    {
        uiCanvasGroup.alpha = isActivate ? 1 : 0;
        uiCanvasGroup.interactable = isActivate;
        uiCanvasGroup.blocksRaycasts = isActivate;
    }

    /// <summary>
    /// Handles the Next button click event to reset the game and hide the UI.
    /// </summary>
    private void OnNextScreenButtonClicked()
    {
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
        GameManager.Instance.ResetGame();
        GameManager.Instance.LoadGame();
        SetUICanvasState(false);
    }

    /// <summary>
    /// Shows the specified screen (home, next, or game over).
    /// </summary>
    /// <param name="isHome"></param>
    /// <param name="isNext"></param>
    /// <param name="isGameOver"></param>
    public void ShowScreen(bool isHome, bool isNext, bool isGameOver, bool isSetting = false)
    {
        //SetToggleState();
        homeScreenPanel.SetActive(isHome);
        nextScreenPanel.SetActive(isNext);
        gameOverPanel.SetActive(isGameOver);
        settingPanel.SetActive(isSetting);
        SetUICanvasState(true);
        if (isGameOver)
        {
            GameManager.Instance.PlayMusic(1); // Play game over music
        }
    }

    /// <summary>
    /// Sets the toggle state based on the current game state.
    /// </summary>
    private void SetToggleState()
    {
        GameManager.isAutoClicking = true;
        if (GameManager.Instance.currentGameState == GameState.Easy)
            easyModeToggle.isOn = true;
        else if (GameManager.Instance.currentGameState == GameState.Medium)
            mediumModeToggle.isOn = true;
        else if (GameManager.Instance.currentGameState == GameState.Hard)
            hardModeToggle.isOn = true;
        GameManager.isAutoClicking = false;
    }

    /// <summary>
    /// sets the setting panel active
    /// </summary>
    private void OnSettingClick()
    {
        GameManager.Instance.PlayButtonClickSound(); // Play button click sound
        isSetting = !isSetting;
        if (isSetting)
            ShowScreen(false, false, false, true);
        else
            ShowScreen(true, false, false);
    }

    private void OnReloadClick()
    {
        GameManager.Instance.ResetGame();
        GameManager.Instance.LoadGame();
    }

    private void OnSaveClick()
    {
        GameManager.Instance.SaveLevel();
        GameManager.Instance.SaveCards();
    }


}
