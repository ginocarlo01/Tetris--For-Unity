using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public enum Directions { Up, Down, Left, Right }

//[CreateAssetMenu]
public class MobileButtonsManager : MonoBehaviour
{
    public Button up, down, left, right;
    public static MobileButtonsManager instance;

    [SerializeField]
    public IMobileInputState curState;
    public IMobileInputState CurrState { get => curState; set => curState = value; }
    
    //public Directions directions;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        Debug.Log(curState);
    }
    public void Reset()
    {
        up.onClick.RemoveAllListeners();
        down.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();
        right.onClick.RemoveAllListeners();
    }

}
