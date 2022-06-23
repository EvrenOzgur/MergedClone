using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class M_Grid : MonoBehaviour
{
    public static Action<Dice, Dice, GameObject, bool> OnSetDice;
    public GridItem GridItemPrefab;

    [HideInInspector] public GridItem[,] GridArray;

    public int GridLenghtI = 5;
    public int GridLenghtJ = 5;

    List<GridItem> foundGridItemList;
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
        Dice _dice1 = dice1;
        Dice _dice2 = dice2;
        GameObject _diceContainer = diceContainer;
        bool _isOneDice = isOneDice;


        if (_isOneDice)
        {
            int _controlX = Mathf.RoundToInt(_dice1.transform.position.x);
            int _controlY = Mathf.RoundToInt(_dice1.transform.position.y);
            if (InGridControl(_controlX, _controlY))
            {
                if (GridArray[_controlX, _controlY].IsFull == false)
                {
                    _dice1.transform.SetParent(GridArray[_controlX, _controlY].transform);
                    _dice1.transform.localPosition = Vector3.zero;
                    _dice1.CurrentGridItem = GridArray[_controlX, _controlY];
                    GridArray[_controlX, _controlY].CurrentDice = _dice1;
                    GridArray[_controlX, _controlY].IsFull = true;
                    List<GridItem> _placedGridItemList = new List<GridItem>();
                    _placedGridItemList.Add(GridArray[_controlX, _controlY]);
                    RecursiveSucceedControl(_placedGridItemList);
                    M_Spawner.I.CurrentDice1 = null;
                    GameContinueControl();

                }
                else
                {
                    _diceContainer.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                _diceContainer.transform.localPosition = Vector3.zero;
            }
        }
        else
        {

            int _controlX1 = Mathf.RoundToInt(_dice1.transform.position.x);
            int _controlY1 = Mathf.RoundToInt(_dice1.transform.position.y);
            int _controlX2 = Mathf.RoundToInt(_dice2.transform.position.x);
            int _controlY2 = Mathf.RoundToInt(_dice2.transform.position.y);
            if (InGridControl(_controlX1, _controlY1) && InGridControl(_controlX2, _controlY2))
            {
                if (GridArray[_controlX1, _controlY1].IsFull == false && GridArray[_controlX2, _controlY2].IsFull == false)
                {
                    _dice1.transform.SetParent(GridArray[_controlX1, _controlY1].transform);
                    _dice1.transform.localPosition = Vector3.zero;
                    _dice1.CurrentGridItem = GridArray[_controlX1, _controlY1];
                    GridArray[_controlX1, _controlY1].CurrentDice = _dice1;
                    GridArray[_controlX1, _controlY1].IsFull = true;
                    M_Spawner.I.CurrentDice1 = null;
                    _dice2.transform.SetParent(GridArray[_controlX2, _controlY2].transform);
                    _dice2.transform.localPosition = Vector3.zero;
                    _dice2.CurrentGridItem = GridArray[_controlX2, _controlY2];
                    GridArray[_controlX2, _controlY2].CurrentDice = _dice2;
                    GridArray[_controlX2, _controlY2].IsFull = true;
                    List<GridItem> _placedGridItemList = new List<GridItem>();
                    _placedGridItemList.Add(GridArray[_controlX1, _controlY1]);
                    _placedGridItemList.Add(GridArray[_controlX2, _controlY2]);

                    RecursiveSucceedControl(_placedGridItemList);
                    M_Spawner.I.CurrentDice2 = null;
                    GameContinueControl();
                }
                else
                {
                    _diceContainer.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                _diceContainer.transform.localPosition = Vector3.zero;
            }
        }
    }
    void RecursiveSucceedControl(List<GridItem> placedGridItemList)
    {
        if (placedGridItemList != null)
        {
            List<GridItem> _succeedList = new List<GridItem>();
            float _delay = 0;
            for (int i = 0; i < placedGridItemList.Count; i++)
            {
                GridItem _placedGridItem = placedGridItemList[i];
                foundGridItemList = new List<GridItem>();
                foundGridItemList.Add(_placedGridItem);
                RecursiveFoundNearGrids(placedGridItemList, i);
                if (foundGridItemList.Count > 0)
                {
                    for (int j = 0; j < foundGridItemList.Count; j++)
                    {
                        foundGridItemList[j].IsCheck = false;
                    }
                    if (foundGridItemList.Count > 2)
                    {
                        int _diceNumber = foundGridItemList[0].CurrentDice.DiceNumber;
                        for (int k = 0; k < foundGridItemList.Count; k++)
                        {

                            GridItem _foundGridItem = foundGridItemList[k];
                            List<Vector3> _foundDicePathList = new List<Vector3>();
                            GridItem _founderGridItem = _foundGridItem.FounderGridItem;
                            _foundDicePathList.Add(_foundGridItem.transform.position + Vector3.back);
                            float _diceSpeed = 2;
                            while (_founderGridItem != null)
                            {
                                _foundDicePathList.Add(_founderGridItem.transform.position + Vector3.back);
                                _founderGridItem = _founderGridItem.FounderGridItem;
                            }
                            _foundGridItem.CurrentDice.transform.localEulerAngles = new Vector3(0, 180, 0);
                            _foundGridItem.CurrentDice.transform.DOPath(_foundDicePathList.ToArray(), _diceSpeed).SetSpeedBased();
                            float _pathTime = CalculateDicePathDistance(_foundDicePathList);
                            if (_delay < _pathTime)
                            {
                                _delay = _pathTime;
                            }
                            Destroy(_foundGridItem.CurrentDice.gameObject, _delay + 0.1f);
                            _foundGridItem.CurrentDice = null;
                            _foundGridItem.IsFull = false;


                        }
                        for (int k = 0; k < foundGridItemList.Count; k++)
                        {
                            foundGridItemList[k].FounderGridItem = null;
                        }
                        if (_diceNumber != 0)
                        {
                            int _instantiateDiceNumber = (_diceNumber + 1) % (M_Spawner.I.DicePrefabs.Length);
                            _placedGridItem.CurrentDice = Instantiate(M_Spawner.I.DicePrefabs[_instantiateDiceNumber], _placedGridItem.transform);
                            _placedGridItem.CurrentDice.DiceNumber = _instantiateDiceNumber;
                            _placedGridItem.IsFull = true;
                            if (M_Spawner.I.InstantiateDiceNumber < _instantiateDiceNumber + 1)
                            {
                                M_Spawner.I.InstantiateDiceNumber++;
                                if (M_Spawner.I.InstantiateDiceNumber >= M_Spawner.I.DicePrefabs.Length)
                                {
                                    M_Spawner.I.InstantiateBoomDiceNumber = 0;
                                }
                            }
                            _placedGridItem.CurrentDice.transform.localPosition = -Vector3.forward / 2;
                            _placedGridItem.CurrentDice.transform.localEulerAngles = new Vector3(0, 180, 0);
                            _placedGridItem.CurrentDice.transform.DOLocalRotate(Vector3.zero, 0.25f).SetDelay(_delay - 0.25f);
                            _placedGridItem.CurrentDice.transform.DOShakeRotation(_delay - 0.25f, 30, 30);
                            _succeedList.Add(_placedGridItem);
                        }
                        else
                        {
                            _placedGridItem.CurrentDice = Instantiate(M_Spawner.I.DicePrefabs[0], _placedGridItem.transform);
                            _placedGridItem.CurrentDice.transform.localScale = Vector3.zero;
                            _placedGridItem.CurrentDice.transform.DOPunchScale(new Vector3(3, 3, 3), 2, 30);
                            BoomDice(_placedGridItem, _delay);

                        }
                    }
                }
            }
            for (int i = 0; i < _succeedList.Count; i++)
            {
                StartCoroutine(RecursiveSucceedControlIE(new List<GridItem>() { _succeedList[i] }, _delay));
            }
        }
    }
    IEnumerator RecursiveSucceedControlIE(List<GridItem> placedGridItemList, float delay)
    {
        yield return new WaitForSeconds(delay);
        RecursiveSucceedControl(placedGridItemList);
    }
    void RecursiveFoundNearGrids(List<GridItem> placedGridItemList, int placedGridItemNumber)
    {
        GridItem _placedGridItem = placedGridItemList[placedGridItemNumber];
        _placedGridItem.IsCheck = true;
        List<GridItem> _foundGridItems = new List<GridItem>();
        GridItem _controlGridItem = null;
        if (InGridControl(_placedGridItem.IndexI - 1, _placedGridItem.IndexJ))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI - 1, _placedGridItem.IndexJ];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItemList.Add(_controlGridItem);
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
                foundGridItemList.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        if (InGridControl(_placedGridItem.IndexI, _placedGridItem.IndexJ - 1))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI, _placedGridItem.IndexJ - 1];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItemList.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        if (InGridControl(_placedGridItem.IndexI, _placedGridItem.IndexJ + 1))
        {
            _controlGridItem = GridArray[_placedGridItem.IndexI, _placedGridItem.IndexJ + 1];
            if (NearGridControl(_placedGridItem, _controlGridItem, placedGridItemList))
            {
                _controlGridItem.IsCheck = true;
                _controlGridItem.FounderGridItem = _placedGridItem;
                foundGridItemList.Add(_controlGridItem);
                _foundGridItems.Add(_controlGridItem);

            }
        }
        for (int i = 0; i < _foundGridItems.Count; i++)
        {
            RecursiveFoundNearGrids(_foundGridItems, i);
        }
    }
    bool InGridControl(int controlX, int controlY)
    {
        if (controlX >= 0 &&
            controlX < GridLenghtI &&
            controlY >= 0 &&
            controlY < GridLenghtJ)
        {
            return true;
        }
        return false;
    }
    float CalculateDicePathDistance(List<Vector3> dicePathList)
    {
        float _pathDistance = 0;
        for (int i = 0; i < dicePathList.Count - 1; i++)
        {
            _pathDistance += Vector3.Distance(dicePathList[i], dicePathList[i + 1]);
        }
        return _pathDistance;
    }
    void BoomDice(GridItem boomGridItem, float delayTime)
    {
        List<GridItem> _boomGridItemList = new List<GridItem>();
        _boomGridItemList.Add(boomGridItem);
        for (int i = boomGridItem.IndexI - 1; i < boomGridItem.IndexI + 2; i++)
        {
            for (int j = boomGridItem.IndexJ - 1; j < boomGridItem.IndexJ + 2; j++)
            {
                if (InGridControl(i, j))
                {
                    if (GridArray[i, j].IsFull)
                    {
                        _boomGridItemList.Add(GridArray[i, j]);
                    }
                }
            }
        }
        _boomGridItemList = _boomGridItemList.OrderBy(qq => Vector3.Distance(qq.transform.position, boomGridItem.transform.position)).ToList();
        for (int i = 0; i < _boomGridItemList.Count; i++)
        {
            if (_boomGridItemList[i].IsFull)
            {
                GridItem _boomGridItem = _boomGridItemList[i];
                _boomGridItem.CurrentDice.transform.DOScale(Vector3.zero, delayTime);
                _boomGridItem.CurrentDice.transform.DOLocalRotate(new Vector3(0, 720, 0), delayTime, RotateMode.FastBeyond360);
                Destroy(_boomGridItem.CurrentDice.gameObject);
                _boomGridItem.IsFull = false;
                _boomGridItem.CurrentDice = null;
            }

        }
    }
    bool NearGridControl(GridItem gridItem, GridItem controlGridItem, List<GridItem> gridItemList)
    {
        if (controlGridItem.IsCheck == false &&
            controlGridItem.IsFull &&
            controlGridItem.CurrentDice != null &&
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
    void GameContinueControl()
    {
        int _counter = 0;
        for (int i = 0; i < GridLenghtI; i++)
        {
            for (int j = 0; j < GridLenghtJ; j++)
            {

                if (!GridArray[i, j].IsFull)
                {
                    _counter = 1;
                    if (InGridControl(i + 1, j))
                    {
                        if (!GridArray[i + 1, j].IsFull)
                        {
                            _counter = 2;
                        }
                    }
                }

                if (_counter == 2) break;
            }
            if (_counter == 2) break;
        }
        if (_counter!=2)
        {
            for (int i = 0; i < GridLenghtI; i++)
            {
                for (int j = 0; j < GridLenghtJ; j++)
                {
                    if (!GridArray[i, j].IsFull)
                    {
                        _counter = 1;
                        if (InGridControl(i, j + 1))
                        {
                            if (!GridArray[i, j + 1].IsFull)
                            {
                                _counter = 2;
                            }
                        }
                    }
                    if (_counter == 2) break;
                }
                if (_counter == 2) break;
            }
        }
        if (_counter == 0)
        {
            M_Observer.OnGameFail?.Invoke();
        }
        else
        {
            if (_counter == 1)
            {
                M_Spawner.I.SpawnDiceCount = 1;
                M_Spawner.OnSpawnDice?.Invoke();

            }
            else
            {
                M_Spawner.I.SpawnDiceCount = 2;
                M_Spawner.OnSpawnDice?.Invoke();

            }
        }
    }
    void GridCreate()
    {
        GridArray = new GridItem[GridLenghtI, GridLenghtJ];
        for (int i = 0; i < GridLenghtI; i++)
        {
            for (int j = 0; j < GridLenghtJ; j++)
            {
                GridItem _gridItem = Instantiate(GridItemPrefab, transform);
                _gridItem.transform.position = new Vector3(i, j, 0);
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
