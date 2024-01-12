using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TetrisMobileInput : IMobileInputState
{
    Piece piece;
    private Vector2 startPosition;
    private Vector2 endPosition;

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

        MobileButtonsManager.instance.up.OnPointerDown(MobileInputUp);
        */

        MobileButtonsManager.instance.rotate.onClick.AddListener(RotatePiece);
        MobileButtonsManager.instance.up.onClick.AddListener(HardDropPiece);

        AddButtonListeners(MobileButtonsManager.instance.down, Directions.Down);
        AddButtonListeners(MobileButtonsManager.instance.left, Directions.Left);
        AddButtonListeners(MobileButtonsManager.instance.right, Directions.Right);
    }

    public void OnUpdate() 
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
            }
            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended)
            {

                endPosition = touch.position;

                float x = endPosition.x - startPosition.x;
                float y = endPosition.y - startPosition.y;

                if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                {
                    piece.shouldRotate = true;
                    piece.HorizontalInt = 0;
                    piece.VerticalInt = 0;

                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    piece.HorizontalInt = x > 0 ? 1 : -1;
                    //Debug.Log(direction);
                }
                else
                {
                    piece.VerticalInt = y > 0 ? 0 : -1;
                    //snake.GridMoveDirection = y > 0 ? Vector2Int.up : Vector2Int.down;
                    //Debug.Log(direction);
                }
                // Position the cube.
                //cube.position = position;
            }

            if(touch.phase == TouchPhase.Ended)
            {
                //piece.DisableMovement();
                piece.HorizontalInt = 0;
                piece.VerticalInt = 0;
            }
        }
        else
        {
            //piece.DisableMovement();
            piece.HorizontalInt = 0;
            piece.VerticalInt = 0;
        }
    }
    void HardDropPiece() { piece.shouldHardDrop = true; }

    void RotatePiece() { piece.shouldRotate = true;  }

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
        //Debug.Log(direction + " button released");

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
