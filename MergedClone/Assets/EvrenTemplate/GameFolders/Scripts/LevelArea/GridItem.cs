using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
   [HideInInspector] public int IndexI;
   [HideInInspector] public int IndexJ;
    [HideInInspector] public Dice CurrentDice;
    [HideInInspector] public bool IsFull;
    [HideInInspector] public bool IsCheck;
    [HideInInspector] public GridItem FounderGridItem;
}
