using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class M_Spawner : MonoBehaviour
{
    public Dice[] DicePrefabs;
    public GameObject DiceContainer;
    public Dice CurrentDice1;
    public Dice CurrentDice2;
    
     public int SpawnDiceCount = 2;
     public int InstantiateDiceNumber = 6;
    [HideInInspector] public bool IsOneDice;
    public Vector3 DiceContainerOffset;
    Vector3 fingerFirstPos;
    Vector3 fingerMovePos;
    bool canDiceRotate;
    GameObject pickedDiceContainer;
   

    private void OnEnable()
    {
        M_Observer.OnGameStart += GameStart;
        FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;

    }
    private void OnDisable()
    {
        M_Observer.OnGameStart -= GameStart;
        FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
        FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
        FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;

    }

    private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
    {
        if (fingerIndex == 0 && pickedDiceContainer != null)
        {
            if (canDiceRotate)
            {
                DiceContainer.transform.DOLocalRotate(DiceContainer.transform.localEulerAngles - new Vector3(0,0,90) ,0.1f,RotateMode.FastBeyond360 ).SetEase(Ease.OutExpo);
            }
            else
            {
                M_Grid.OnSetDice.Invoke(CurrentDice1 , CurrentDice2 , DiceContainer , IsOneDice);
            }
        }
    }

    private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
    {
        if (fingerIndex == 0 && pickedDiceContainer!= null)
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
        if (fingerIndex == 0)
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
        SpawnDice();
    }
    void SpawnDice()
    {
        int a = 0;
        int b = 0;
        if (Random.Range(0, SpawnDiceCount)%2 == 0)
        {
            int _diceNumber = Random.Range(1, InstantiateDiceNumber);
            Dice _dice = Instantiate(DicePrefabs[_diceNumber], DiceContainer.transform);
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
            Dice _dice1 = Instantiate(DicePrefabs[a], DiceContainer.transform);
            _dice1.transform.localPosition = new Vector3(0.5f, 0, 0);
            CurrentDice1 = _dice1;
            Dice _dice2 = Instantiate(DicePrefabs[b] , DiceContainer.transform);
            _dice2.transform.localPosition = new Vector3(-0.5f,0,0);
            CurrentDice2 = _dice2;
            IsOneDice = false;
        }
    }
    //RAYCAST �LE OBJE YAKALAMA.
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

    // RAYCAST �LE TA�IMA POZ�SYONU.
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
