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
    public Button up, down, left, right, rotate;
    public static MobileButtonsManager instance;
    public EventTrigger test;
    private float width;
    private float height;
    [SerializeField]
    public IMobileInputState curState;
    public IMobileInputState CurrState { get => curState; set => curState = value; }
    public float Width { get => width; }
    public float Height { get => height; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        // Add listeners for button press and release

        width = (float)Screen.width / 2.0f;
        height = (float)Screen.height / 2.0f;
        Debug.Log(curState);
    }

    private void Update()
    {
        if(curState!=null)
        curState.OnUpdate();
    }

    public void Reset()
    {

        RemoveButtonListeners(up);
        RemoveButtonListeners(down);
        RemoveButtonListeners(left);
        RemoveButtonListeners(rotate);
        RemoveButtonListeners(right);

        rotate.onClick.RemoveAllListeners();
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
