using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class M_Menu : MonoBehaviour
{
    public static Action OnSetScoreText;

    public Panel MainMenuPanelPrefab;
    public Panel GamePanelPrefab;
    public Panel PausePanelPrefab;
    public Panel CompletePanelPrefab;
    public Panel GameFailPanel;
    [HideInInspector] public Panel CurrentPanel;

    private void Awake()
    {

        CurrentPanel = Instantiate(MainMenuPanelPrefab, transform);


    }



   

    private void OnEnable()
    {
        M_Observer.OnGameCreate += GameCreate;
        M_Observer.OnGameReady += GameReady;
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGamePause += GamePause;
        M_Observer.OnGameFail += GameFail;
        M_Observer.OnGameComplete += GameComplete;
        M_Observer.OnGameRetry += GameRetry;
        M_Observer.OnGameContinue += GameContinue;
        M_Observer.OnGameNextLevel += GameNextLevel;
        OnSetScoreText += SetScoreText;

    }
    private void OnDisable()
    {
        M_Observer.OnGameCreate -= GameCreate;
        M_Observer.OnGameReady -= GameReady;
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGamePause -= GamePause;
        M_Observer.OnGameFail -= GameFail;
        M_Observer.OnGameComplete -= GameComplete;
        M_Observer.OnGameRetry -= GameRetry;
        M_Observer.OnGameContinue -= GameContinue;
        M_Observer.OnGameNextLevel -= GameNextLevel;
        OnSetScoreText -= SetScoreText;

    }

    

    private void GameCreate()
    {
       
    }

    private void GameReady()
    {
        //print("GameReady");
    }

    private void GameStart()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
        SetScoreText();

    }
    private void GamePause()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(PausePanelPrefab, transform);
        SetScoreText();
    }
    private void GameFail()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GameFailPanel, transform);
        SetScoreText();
    }

    private void GameComplete()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(CompletePanelPrefab, transform);
        SetScoreText();

    }

    private void GameRetry()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
        SetScoreText();

    }

    private void GameContinue()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
        SetScoreText();

    }

    private void GameNextLevel()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
        SetScoreText();

    }
    private void SetScoreText()
    {
        if (CurrentPanel.HighScoreText != null)
        {
            CurrentPanel.HighScoreText.text = M_Level.I.HighScore.ToString();
        }
        if (CurrentPanel.CurrentScoreText != null)
        {
            CurrentPanel.CurrentScoreText.text = M_Level.I.CurrentLevelScore.ToString();
        }
    }
    void DeleteCurrentPanel()
    {
        if (CurrentPanel != null)
        {
          
                Destroy(CurrentPanel.gameObject);
                CurrentPanel = null;
      



        }
    }
    public static M_Menu II;

    public static M_Menu I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Menu");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Menu>();
                }
            }

            return II;
        }
    }
}
