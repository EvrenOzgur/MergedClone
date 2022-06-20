using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Spawner : MonoBehaviour
{
    public Dice[] DicePrefabs;
    public Transform DiceContainer;
    public Dice CurrentDice1;
    public Dice CurrentDice2;
    
     public int SpawnDiceCount = 2;
     public int InstantiateDiceNumber = 6;
    [HideInInspector] public bool IsOneDice;


    private void OnEnable()
    {
        M_Observer.OnGameStart += GameStart;
    }
    private void OnDisable()
    {
        M_Observer.OnGameStart -= GameStart;

    }
    void GameStart()
    {
        SpawnDice();
    }
    void SpawnDice()
    {
        int a = 0;
        int b = 0;
        int _random = Random.Range(0, SpawnDiceCount);
        print(_random);
        if ( _random == 0)
        {
            int _diceNumber = Random.Range(1, InstantiateDiceNumber);
            Dice _dice = Instantiate(DicePrefabs[_diceNumber], DiceContainer);
            _dice.transform.localPosition = Vector3.zero;
            CurrentDice1 = _dice;
            IsOneDice = true;
        }
        else
        {
            while (a>=b)
            {
                a = Random.Range(1,InstantiateDiceNumber);
                b = Random.Range(1,InstantiateDiceNumber);
            }
            Dice _dice1 = Instantiate(DicePrefabs[a], DiceContainer);
            _dice1.transform.localPosition = new Vector3(0.5f, 0, 0);
            CurrentDice1 = _dice1;
            Dice _dice2 = Instantiate(DicePrefabs[b] , DiceContainer);
            _dice2.transform.localPosition = new Vector3(-0.5f,0,0);
            CurrentDice2 = _dice2;
            IsOneDice = false;
        }
    }
}
