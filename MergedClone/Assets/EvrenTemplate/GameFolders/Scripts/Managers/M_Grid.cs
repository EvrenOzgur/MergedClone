using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Grid : MonoBehaviour
{
    public static Action<Dice , Dice , GameObject , bool> OnSetDice;
    public GridItem GridItemPrefab;

    [HideInInspector] public GridItem[,] GridArray;

    public int GridLenghtI = 5;
    public int GridLenghtJ = 5;
    private void Awake()
    {
        GridCreate();
    }
    private void OnEnable()
    {
        M_Observer.OnGameCreate += GameCreate;
        OnSetDice += SetDice;
    }
    private void OnDisable()
    {
        M_Observer.OnGameCreate -= GameCreate;
        OnSetDice -= SetDice;

    }

    private void SetDice(Dice dice1, Dice dice2, GameObject diceContainer, bool isOneDice)
    {
       
    }

    void GameCreate()
    {
       
    }
    void GridCreate()
    {
        GridArray = new GridItem[GridLenghtI , GridLenghtJ];
        for (int i = 0; i < GridLenghtI; i++)
        {
            for (int j = 0; j < GridLenghtJ; j++)
            {
                GridItem _gridItem = Instantiate(GridItemPrefab , transform);
                _gridItem.transform.position = new Vector3(i,j,0);
                _gridItem.IndexI = i;
                _gridItem.IndexJ = j;
                GridArray[i, j] = _gridItem;
            }
        }
    }
    public static M_Grid II;

    public static M_Grid I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Grid");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Grid>();
                }
            }

            return II;
        }
    }
}
