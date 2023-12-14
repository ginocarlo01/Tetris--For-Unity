using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private LevelGrid levelGrid;

    [SerializeField] private Snake snake;
    void Awake()
    {
        /*int number = 0;
        FunctionPeriodic.Create(() =>
        {
            CMDebug.TextPopupMouse("Ding! " + number);
            number++;
        }, .3f);*/

        levelGrid = new LevelGrid(10, 20, ((int)transform.position.x), ((int)transform.position.y));
        snake.Setup(levelGrid);
        levelGrid.Setup(snake);
    }
    void Update()
    {
        
    }
}
