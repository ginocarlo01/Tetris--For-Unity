using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SnakeMobileInput : IMobileInputState
{
    Snake snake;
    float width, height;
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
            Touch touch = Input.GetTouch(0);

            // Move the cube if the screen has the finger moving.
            if (touch.phase == TouchPhase.Moved)
            {

                Vector2 pos = touch.position;
                pos.x = (pos.x - width) / width;
                pos.y = (pos.y - height) / height;

                int h;
                int v;

                //position = new Vector3(-pos.x, pos.y, 0.0f);
                if (pos.x > 0)
                {
                    h = 1;
                    MobileInputRight();
                    //touch.a
                    //Debug.Log("h: " + h);
                }
                if (pos.x < 0)
                {
                    h = -1;
                    MobileInputLeft();
                    Debug.Log("h: " + h);
                }
                if (pos.y > 0)
                {
                    v = 1;
                    MobileInputUp();
                    Debug.Log("v: " + v);
                }
                if (pos.y < 0)
                {
                    v = -1;
                    MobileInputDown();
                    Debug.Log("v: " + v);
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
