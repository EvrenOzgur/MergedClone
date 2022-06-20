using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class M_Menu : MonoBehaviour
{
    public Panel MainMenuPanelPrefab;
    public Panel GamePanelPrefab;
    public Panel PausePanelPrefab;
    public Panel CompletePanelPrefab;
    public Panel GameFailPanel;
    [HideInInspector] public Panel CurrentPanel;

    private void Awake()
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
        CurrentPanel = Instantiate(MainMenuPanelPrefab, transform);


    }



    private void OnDestroy()
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
       

    }
    private void GamePause()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(PausePanelPrefab, transform);
       
    }
    private void GameFail()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GameFailPanel, transform);
    }

    private void GameComplete()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(CompletePanelPrefab, transform);
       
    }

    private void GameRetry()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
       
    }

    private void GameContinue()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
      
    }

    private void GameNextLevel()
    {
        DeleteCurrentPanel();
        CurrentPanel = Instantiate(GamePanelPrefab, transform);
       
    }

    void DeleteCurrentPanel()
    {
        if (CurrentPanel != null)
        {
          
                Destroy(CurrentPanel.gameObject);
                CurrentPanel = null;
      



        }
    }
}
