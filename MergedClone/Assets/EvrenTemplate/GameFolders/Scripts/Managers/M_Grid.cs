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

    List<GridItem> foundGridItems;
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
        if (isOneDice)
        {
            int _controlX =Mathf.RoundToInt( dice1.transform.position.x);
            int _controlY =Mathf.RoundToInt( dice1.transform.position.y);
            if (InGridControl(_controlX, _controlY))
            {
                if (GridArray[_controlX, _controlY].IsFull == false)
                {
                    dice1.transform.SetParent(GridArray[_controlX, _controlY].transform);
                    dice1.transform.localPosition = Vector3.zero;
                    dice1.CurrentGridItem = GridArray[_controlX, _controlY];
                    GridArray[_controlX, _controlY].CurrentDice = dice1;
                    GridArray[_controlX, _controlY].IsFull = true;
                    M_Spawner.I.CurrentDice1 = null;
                    M_Spawner.OnSpawnDice?.Invoke();
                }
                else
                {
                    diceContainer.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                diceContainer.transform.localPosition = Vector3.zero;
            }
        }
        else
        {
            int _controlX1 = Mathf.RoundToInt(dice1.transform.position.x);
            int _controlY1 = Mathf.RoundToInt(dice1.transform.position.y);
            int _controlX2 = Mathf.RoundToInt(dice2.transform.position.x);
            int _controlY2= Mathf.RoundToInt(dice2.transform.position.y);
            if (InGridControl(_controlX1,_controlY1) && InGridControl(_controlX2,_controlY2))
            {
                if (GridArray[_controlX1, _controlY1].IsFull == false && GridArray[_controlX2, _controlY2].IsFull == false)
                {
                    dice1.transform.SetParent(GridArray[_controlX1, _controlY1].transform);
                    dice1.transform.localPosition = Vector3.zero;
                    dice1.CurrentGridItem = GridArray[_controlX1, _controlY1];
                    GridArray[_controlX1, _controlY1].CurrentDice = dice1;
                    GridArray[_controlX1, _controlY1].IsFull = true;
                    M_Spawner.I.CurrentDice1 = null;
                    dice2.transform.SetParent(GridArray[_controlX2, _controlY2].transform);
                    dice2.transform.localPosition = Vector3.zero;
                    dice2.CurrentGridItem = GridArray[_controlX2, _controlY2];
                    GridArray[_controlX2, _controlY2].CurrentDice = dice2;
                    GridArray[_controlX2, _controlY2].IsFull = true;
                    M_Spawner.I.CurrentDice2 = null;
                    M_Spawner.OnSpawnDice?.Invoke();

                }
                else
                {
                    diceContainer.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                diceContainer.transform.localPosition = Vector3.zero;
            }
        }
    }
    void RecursiveSucceedControl()
    {

    }
    void RecursiveFoundNearGrids(List<GridItem> placedGridItemList , int placedGridItemNumber )
    {
        GridItem _placedGridItem = placedGridItemList[placedGridItemNumber];
        _placedGridItem.IsCheck = true;
        List<GridItem> _foundGridItems = new List<GridItem>();
        GridItem _controlGridItem ;
        if (InGridControl(_placedGridItem.IndexI -1, _placedGridItem.IndexJ))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI - 1, _placedGridItem.IndexJ];
            if (NearGridControl(_placedGridItem , _controlGridItem , placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItems.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        if (InGridControl(_placedGridItem.IndexI + 1, _placedGridItem.IndexJ))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI + 1, _placedGridItem.IndexJ];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItems.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        if (InGridControl(_placedGridItem.IndexI , _placedGridItem.IndexJ-1))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI , _placedGridItem.IndexJ-1];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItems.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        if (InGridControl(_placedGridItem.IndexI , _placedGridItem.IndexJ+1))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI , _placedGridItem.IndexJ+1];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItems.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        for (int i = 0; i < _foundGridItems.Count; i++)
        {
            RecursiveFoundNearGrids(_foundGridItems , i);
        }
    }
    bool InGridControl(int controlX , int controlY)
    {
        if (controlX>= 0 && 
            controlX<= GridLenghtI && 
            controlY >=0 &&
            controlY<= GridLenghtJ)
        {
            return true;
        }
        return false;
    }
    bool NearGridControl(GridItem gridItem ,GridItem controlGridItem , List<GridItem> gridItemList)
    {
        if (controlGridItem.IsCheck == false &&
            controlGridItem.IsFull &&
            controlGridItem.CurrentDice.DiceNumber == gridItem.CurrentDice.DiceNumber &&
            gridItemList.Contains(controlGridItem) == false)
        {
            return true;
        }
        return false;
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
