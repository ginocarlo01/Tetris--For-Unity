using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

[Serializable]
public enum Directions { Up, Down, Left, Right }

public class MobileButtonsManager : MonoBehaviour
{
    public Button up, down, left, right;
    public static MobileButtonsManager instance;
    public EventTrigger test;

    [SerializeField]
    public IMobileInputState curState;
    public IMobileInputState CurrState { get => curState; set => curState = value; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        // Add listeners for button press and release
        

        Debug.Log(curState);
    }

    

    public void Reset()
    {

        RemoveButtonListeners(up);
        RemoveButtonListeners(down);
        RemoveButtonListeners(left);
        RemoveButtonListeners(right);

        up.onClick.RemoveAllListeners();
        down.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();
        right.onClick.RemoveAllListeners();
    }

    void RemoveButtonListeners(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            Destroy(trigger);
        }
    }
}
