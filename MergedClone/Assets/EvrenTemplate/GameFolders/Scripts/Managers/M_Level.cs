using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Level : MonoBehaviour
{
    public static Action<int> OnSetScore;

    public Level[] Levels;

    [HideInInspector] public Level CurrentLevel;
   [HideInInspector] public int LevelIndex;
    [HideInInspector] public int HighScore ;
    [HideInInspector] public int CurrentLevelScore = 0;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("LevelIndex"))
        {
            LevelIndex = PlayerPrefs.GetInt("LevelIndex");
        }
        else
        {
            LevelIndex = 0;
        }
        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighScore = PlayerPrefs.GetInt("HighScore");
        }
        else
        {
            HighScore = 0;
        }
    }
   
   
    private void OnEnable()
    {
        OnSetScore += SetScore;

    }
    private void OnDisable()
    {
        OnSetScore -= SetScore;

    }
    private void Start()
    {
        CurrentLevel = Instantiate(Levels[LevelIndex] , transform);
        M_Menu.OnSetScoreText?.Invoke();
    }
    void SetScore(int scoreUp)
    {
        CurrentLevelScore += scoreUp;
        if (CurrentLevelScore > HighScore)
        {
            HighScore = CurrentLevelScore;
            PlayerPrefs.SetInt("HighScore", HighScore);

        }
        M_Menu.OnSetScoreText?.Invoke();
    }
    public static M_Level II;

    public static M_Level I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Level");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Level>();
                }
            }

            return II;
        }
    }
}
