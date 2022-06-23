using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class M_Spawner : MonoBehaviour
{
    public static Action OnSpawnDice;
    public Dice[] DicePrefabs;
    public GameObject DiceContainer;
    public Dice CurrentDice1;
    public Dice CurrentDice2;
    
     public int SpawnDiceCount = 2;
     public int InstantiateDiceNumber = 6;
    public int InstantiateBoomDiceNumber = 1;
    [HideInInspector] public bool IsOneDice;
    public Vector3 DiceContainerOffset = new Vector3(0,1,-1);
    Vector3 fingerFirstPos;
    Vector3 fingerMovePos;
    bool canDiceRotate;
    GameObject pickedDiceContainer;
    bool canClick = true;

    private void OnEnable()
    {
        M_Observer.OnGameStart += GameStart;
        M_Observer.OnGameFail += GameFail;
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
        OnSpawnDice += SpawnDice;
    }
    private void OnDisable()
    {
        M_Observer.OnGameStart -= GameStart;
        M_Observer.OnGameFail -= GameFail;

        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
        OnSpawnDice -= SpawnDice;

    }

    private void GameFail()
    {
       canClick = false;
    }

    private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        if (fingerIndex == 0 && pickedDiceContainer != null && canClick)
        {
            if (canDiceRotate)
            {   canClick = false;
                DiceContainer.transform.DOLocalRotate(DiceContainer.transform.localEulerAngles - new Vector3(0,0,90) ,0.1f,RotateMode.FastBeyond360 ).SetEase(Ease.OutExpo).OnComplete(()=>canClick = true);
                if (IsOneDice)
                {
                    CurrentDice1.transform.DOLocalRotate(CurrentDice1.transform.localEulerAngles - new Vector3(0,0,90) , 0.1f , RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
                }
                else
                {
                    CurrentDice1.transform.DOLocalRotate(CurrentDice1.transform.localEulerAngles - new Vector3(0, 0, 90), 0.1f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
                    CurrentDice2.transform.DOLocalRotate(CurrentDice2.transform.localEulerAngles - new Vector3(0, 0, 90), 0.1f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);

                }
            }
            else
            {
                M_Grid.OnSetDice?.Invoke(CurrentDice1 , CurrentDice2 , DiceContainer , IsOneDice);
            }
        }
    }

    private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex == 0 && pickedDiceContainer!= null && canClick)
        {
            fingerMovePos = GetWorldPos(fingerPos);
            if (canDiceRotate)
            {
                float _distance = Vector3.Distance(fingerFirstPos, fingerMovePos);
                if (Mathf.Abs(_distance) >= 0.5f)
                {
                    canDiceRotate = false;
                }
            
            }
            else
            {
                pickedDiceContainer.transform.position = fingerMovePos + DiceContainerOffset;

            }
        }
    }

    private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex == 0 && canClick)
        {
            if (PickObject(fingerPos).CompareTag("DiceContainer"))
            {

                pickedDiceContainer = PickObject(fingerPos);
                if (pickedDiceContainer != null)
                {
                    canDiceRotate = true;
                    fingerFirstPos = GetWorldPos(fingerPos);
                }
            }
         

           

        }
    }

    void GameStart()
    {
        OnSpawnDice?.Invoke();
    }
    void SpawnDice()
    {
        DiceContainer.transform.localPosition = Vector3.zero;
        DiceContainer.transform.localEulerAngles = Vector3.zero;
        int _diceNumber1 = 0;
        int _diceNumber2 = 0;
        if (UnityEngine.Random.Range(0, SpawnDiceCount)%2 == 0)
        {
            int _diceNumber = UnityEngine.Random.Range(1, InstantiateDiceNumber);
            Dice _dice = Instantiate(DicePrefabs[_diceNumber], DiceContainer.transform);
            _dice.transform.localPosition = Vector3.zero;
            _dice.DiceNumber = _diceNumber;
            CurrentDice1 = _dice;
            IsOneDice = true;
        }
        else
        {
            while (_diceNumber1>=_diceNumber2)
            {
                _diceNumber1 = UnityEngine.Random.Range(InstantiateBoomDiceNumber,InstantiateDiceNumber);
                _diceNumber2 = UnityEngine.Random.Range(InstantiateBoomDiceNumber,InstantiateDiceNumber);
            }
            if (_diceNumber1 == 0)
            {
                Dice _dice1 = Instantiate(DicePrefabs[_diceNumber2], DiceContainer.transform);
                _dice1.transform.localPosition = new Vector3(0.525f, 0, 0);
                _dice1.DiceNumber = _diceNumber2;
                CurrentDice1 = _dice1;
                Dice _dice2 = Instantiate(DicePrefabs[_diceNumber1], DiceContainer.transform);
                _dice2.transform.localPosition = new Vector3(-0.525f, 0, 0);
                _dice2.DiceNumber = _diceNumber1;
                CurrentDice2 = _dice2;
                IsOneDice = false;
            }
            else
            {
                Dice _dice1 = Instantiate(DicePrefabs[_diceNumber1], DiceContainer.transform);
                _dice1.transform.localPosition = new Vector3(0.525f, 0, 0);
                _dice1.DiceNumber = _diceNumber1;
                CurrentDice1 = _dice1;
                Dice _dice2 = Instantiate(DicePrefabs[_diceNumber2], DiceContainer.transform);
                _dice2.transform.localPosition = new Vector3(-0.525f, 0, 0);
                _dice2.DiceNumber = _diceNumber2;
                CurrentDice2 = _dice2;
                IsOneDice = false;
            }
           
        }
    }
    //RAYCAST ÝLE OBJE YAKALAMA.
    GameObject PickObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    // RAYCAST ÝLE TAÞIMA POZÝSYONU.
    Vector3 GetWorldPos(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);

        // we solve for intersection with y = 0 plane
        float t = -ray.origin.z / ray.direction.z;

        return ray.GetPoint(t);
    }
    public static M_Spawner II;

    public static M_Spawner I
    {
        get
        {
            if (II == null)
            {
                GameObject _g = GameObject.Find("M_Spawner");
                if (_g != null)
                {
                    II = _g.GetComponent<M_Spawner>();
                }
            }

            return II;
        }
    }
}
