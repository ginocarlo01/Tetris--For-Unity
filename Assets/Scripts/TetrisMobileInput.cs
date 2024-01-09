using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TetrisMobileInput : IMobileInputState
{
    Piece piece;
    public TetrisMobileInput(Piece piece)
    {
        this.piece = piece;
    }
    public void OnBeginState()
    {
#if UNITY_EDITOR
        Debug.Log("Tetris input");
#endif
        MobileButtonsManager.instance.Reset();
        /*
        MobileButtonsManager.instance.up.onClick.AddListener(MobileInputUp);
        MobileButtonsManager.instance.down.onClick.AddListener(MobileInputDown);
        MobileButtonsManager.instance.left.onClick.AddListener(MobileInputLeft);
        MobileButtonsManager.instance.right.onClick.AddListener(MobileInputRight);

        
        */
        //MobileButtonsManager.instance.up.OnPointerDown(MobileInputUp);

        AddButtonListeners(MobileButtonsManager.instance.up, Directions.Up);
        AddButtonListeners(MobileButtonsManager.instance.down, Directions.Down);
        AddButtonListeners(MobileButtonsManager.instance.left, Directions.Left);
        AddButtonListeners(MobileButtonsManager.instance.right, Directions.Right);
    }

    void MobileInputUp() => piece.VerticalInt = 1;
    void MobileInputDown() => piece.VerticalInt = -1;
    void MobileInputLeft() { piece.HorizontalInt = -1;  }
    void MobileInputRight() { piece.HorizontalInt = 1;  }

    void AddButtonListeners(Button button, Directions direction)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry pressEntry = new EventTrigger.Entry();
        pressEntry.eventID = EventTriggerType.PointerDown;
        pressEntry.callback.AddListener((data) => { OnButtonPress(direction); });

        EventTrigger.Entry releaseEntry = new EventTrigger.Entry();
        releaseEntry.eventID = EventTriggerType.PointerUp;
        releaseEntry.callback.AddListener((data) => { OnButtonRelease(direction); });

        trigger.triggers.Add(pressEntry);
        trigger.triggers.Add(releaseEntry);
    }

    void OnButtonPress(Directions direction)
    {
        Debug.Log(direction + " button pressed");

        // Apply logic based on the pressed direction
        switch (direction)
        {
            case Directions.Right:
                piece.HorizontalInt = 1;
                break;
            case Directions.Left:
                piece.HorizontalInt = -1;
                break;
            case Directions.Up:
                piece.VerticalInt = 1;
                break;
            case Directions.Down:
                piece.VerticalInt = -1;
                break;
        }
    }

    void OnButtonRelease(Directions direction)
    {
        Debug.Log(direction + " button released");

        // Reset the corresponding input when the button is released
        if (direction == Directions.Right || direction == Directions.Left)
        {
            piece.HorizontalInt = 0;
        }
        else if (direction == Directions.Up || direction == Directions.Down)
        {
            piece.VerticalInt = 0;
        }
    }
}
