using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private LevelGrid levelGrid;

    [SerializeField] private Snake snake;

    [SerializeField]
    int width = 10;

    [SerializeField]
    int height = 20;
    void Awake()
    {
        /*int number = 0;
        FunctionPeriodic.Create(() =>
        {
            CMDebug.TextPopupMouse("Ding! " + number);
            number++;
        }, .3f);*/

        levelGrid = new LevelGrid(width, height, ((int)transform.position.x), ((int)transform.position.y) + 1);
        snake.Setup(levelGrid, width, height);
        levelGrid.Setup(snake);
    }
    public void DestroySnakeFood()
    {
        levelGrid.DestroyFood();
    }

    public void SpawnSnakeFood()
    {
        levelGrid.SpawnFood();
    }
}
