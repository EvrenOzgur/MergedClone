using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Level : MonoBehaviour
{
    public Level[] Levels;

    [HideInInspector] public Level CurrentLevel;
   [HideInInspector] public int LevelIndex;
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
    }
    private void Start()
    {
        CurrentLevel = Instantiate(Levels[LevelIndex] , transform);
    }
}
