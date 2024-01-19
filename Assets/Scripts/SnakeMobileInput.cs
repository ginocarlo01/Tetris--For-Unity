using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeMobileInput : IMobileInputState
{
    Snake snake;
    float width, height;
    Vector2 startPosition, endPosition;
    //ButtonsManager buttonsManager;
    public SnakeMobileInput(Snake snake=null)
    {
        this.snake = snake;
       
    }

    public void OnBeginState()
    {
        width = MobileButtonsManager.instance.Width;
        height = MobileButtonsManager.instance.Height;
#if UNITY_EDITOR
        Debug.Log("Snake input");
#endif
        MobileButtonsManager.instance.Reset();

        MobileButtonsManager.instance.up.onClick.AddListener(MobileInputUp);
        MobileButtonsManager.instance.down.onClick.AddListener(MobileInputDown);
        MobileButtonsManager.instance.left.onClick.AddListener(MobileInputLeft);
        MobileButtonsManager.instance.right.onClick.AddListener(MobileInputRight);
    }

    public void OnUpdate()
    {
        if (Input.touchCount > 0)
        {
           snake.FirstInputGiven = true;
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
                    //direction = "Tappad";
                    snake.GridMoveDirection = Vector2Int.zero;

                }
                else if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    snake.GridMoveDirection = x > 0 ? Vector2Int.right : Vector2Int.left;
                    //Debug.Log(direction);
                }
                else
                {
                    snake.GridMoveDirection = y > 0 ? Vector2Int.up : Vector2Int.down;
                    //Debug.Log(direction);
                }
                // Position the cube.
                //cube.position = position;
            }
        }
    }

    void MobileInputUp() => snake.GridMoveDirection = Vector2Int.up;
   // void exemplo() { snake.GridMoveDirection = Vector2Int.up; }
    void MobileInputDown() => snake.GridMoveDirection = Vector2Int.down;
    void MobileInputLeft() => snake.GridMoveDirection = Vector2Int.left;
    void MobileInputRight() => snake.GridMoveDirection = Vector2Int.right;
}
