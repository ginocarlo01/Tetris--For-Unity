using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMobileInput : IMobileInputState
{
    Snake snake;
    //ButtonsManager buttonsManager;
    public SnakeMobileInput(Snake snake=null)
    {
        this.snake = snake;
       
    }

    public void OnBeginState()
    {
#if UNITY_EDITOR
        Debug.Log("Snake input");
#endif
        MobileButtonsManager.instance.Reset();

        MobileButtonsManager.instance.up.onClick.AddListener(MobileInputUp);
        MobileButtonsManager.instance.down.onClick.AddListener(MobileInputDown);
        MobileButtonsManager.instance.left.onClick.AddListener(MobileInputLeft);
        MobileButtonsManager.instance.right.onClick.AddListener(MobileInputRight);
    }

    void MobileInputUp() => snake.GridMoveDirection = Vector2Int.up;
    void MobileInputDown() => snake.GridMoveDirection = Vector2Int.down;
    void MobileInputLeft() => snake.GridMoveDirection = Vector2Int.left;
    void MobileInputRight() => snake.GridMoveDirection = Vector2Int.right;
}
