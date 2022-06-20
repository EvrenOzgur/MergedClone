using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Grid : MonoBehaviour
{
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
    }
    private void OnDisable()
    {
        M_Observer.OnGameCreate -= GameCreate;

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
}
