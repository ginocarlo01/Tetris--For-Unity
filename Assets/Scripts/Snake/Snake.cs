using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    //grid
    private Vector2Int gridMoveDirection;
    private Vector2Int nextMoveDirection;
    [SerializeField]
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    [SerializeField]
    private float gridMoveTimerMax = 1f;
    LevelGrid levelGrid;
    [SerializeField]
    Vector2Int initGridPos;
    int width;
    int height;

    //snake
    SpriteRenderer spriteRenderer;
    [SerializeField]
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    [SerializeField]
    Vector2Int initSnakePos;
    private bool canBeControlled = false;
    [SerializeField]
    private Signal lostSnake;
    [SerializeField]
    private Signal winSnake;
    [SerializeField]
    private int maxSnakeBodySize = 4;

    public void Setup(LevelGrid levelGrid,int width, int height)
    {
        this.levelGrid = levelGrid;
        this.width = width;
        this.height= height;
    }

    public void ActivateSnake(){
        canBeControlled = true;
        spriteRenderer.enabled = true;
    }

    public void DisableSnake(){
        canBeControlled = false;
        spriteRenderer.enabled = false;
        
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ActivateSnake();
        RestartSnake();
    }


    public void RestartSnake(){
        gridPosition = new Vector2Int(initGridPos.x + initSnakePos.x, initGridPos.y + initSnakePos.y); 
        gridMoveTimer = gridMoveTimerMax;
        nextMoveDirection = new Vector2Int(0, 0);
        gridMoveDirection = new Vector2Int(0,0); //default movement: zero

        snakeBodySize = 0;
        snakeMovePositionList = new List<Vector2Int>();
    }

    private void Update()
    {
        if(!canBeControlled){ return; }
        HandleInput();
        HandleGridMovement();
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextMoveDirection = Vector2Int.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextMoveDirection = Vector2Int.down;   
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextMoveDirection = Vector2Int.right;  
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            nextMoveDirection = Vector2Int.left;   
        }
    }

    private void HandleDirection(){
        if (nextMoveDirection ==  Vector2Int.up && gridMoveDirection != Vector2Int.down)
        {
            gridMoveDirection = Vector2Int.up;
        }
        else if (nextMoveDirection ==  Vector2Int.down && gridMoveDirection != Vector2Int.up)
        {
            gridMoveDirection = Vector2Int.down;
        }
        else if (nextMoveDirection ==  Vector2Int.right && gridMoveDirection != Vector2Int.left)
        {
            gridMoveDirection = Vector2Int.right;
        }
        else if (nextMoveDirection ==  Vector2Int.left && gridMoveDirection != Vector2Int.right)
        {
            gridMoveDirection = Vector2Int.left;
        }
    }

    private void HandleGridMovement()
    { 
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer > gridMoveTimerMax)
        {
            snakeMovePositionList.Insert(0, gridPosition);

            HandleDirection();
            
            gridPosition += gridMoveDirection;
            gridMoveTimer = 0f;

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            //collide with itself
            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                if (snakeMovePositionList[i] == gridPosition)
                {
                    HandleDeath();
                }

            }

            if(gridPosition.x > initGridPos.x + width ||
                    gridPosition.x < initGridPos.x ||
                    gridPosition.y > initGridPos.y + height ||
                    gridPosition.y < initGridPos.y
                    ){
                        HandleDeath();
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

                if(snakeBodySize >= maxSnakeBodySize){
                    HandleVictory();
                    //HandleDeath();
                }
            }
        }
    }

    public void HandleDeath(){
        RestartSnake();
        DisableSnake();
        lostSnake.Raise();
    }

    public void HandleVictory()
    {
        RestartSnake();
        DisableSnake();
        winSnake.Raise();
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
