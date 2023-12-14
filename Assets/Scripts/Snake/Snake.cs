using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;

    private Vector2Int gridPosition;

    private float gridMoveTimer;

    [SerializeField]
    private float gridMoveTimerMax = 1f;

    LevelGrid levelGrid;

    [SerializeField]
    private int snakeBodySize;

    private List<Vector2Int> snakeMovePositionList;

    [SerializeField]
    Vector2Int initGridPos;

    public void Setup( LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(initGridPos.x, initGridPos.y + 0); //middle of the grid
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1,0); //default movement: right

        snakeBodySize = 0;
        snakeMovePositionList = new List<Vector2Int>();
    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //we can only go up if we are not going down
            if (gridMoveDirection != Vector2Int.down)
            {
                gridMoveDirection = Vector2Int.up;
            }

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Vector2Int.up)
            {
                gridMoveDirection = Vector2Int.down;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Vector2Int.left)
            {
                gridMoveDirection = Vector2Int.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Vector2Int.right)
            {
                gridMoveDirection = Vector2Int.left;
            }
        }
    }

    private void HandleGridMovement()
    { 
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer > gridMoveTimerMax)
        {
            

            snakeMovePositionList.Insert(0, gridPosition);

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                
                Debug.Log(snakeMovePositionList[i]);
            }
            Debug.Log("---------------------------------------");

            gridPosition += gridMoveDirection;
            gridMoveTimer = 0f;

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                if (snakeMovePositionList[i] == gridPosition)
                {
                    CMDebug.TextPopup("Game over", transform.position);
                }
            }

            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite world_sprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one, Color.white);
                world_sprite.SetSprite(GameAssets.Instance.snakeHeadSprite);
                //create the sprite of the snake's part
                FunctionTimer.Create(world_sprite.DestroySelf, gridMoveTimerMax);
                //every time we move, destroy the body part
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
            //transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                snakeBodySize++;
            }
        }

    
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2 (dir.x, dir.y) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPosition()
    {
        List<Vector2Int> plusHead = new List<Vector2Int>() { gridPosition };
        plusHead.AddRange(snakeMovePositionList); //add a list to another

        return plusHead;
    }
}
