using CodeMonkey;
using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Snake : MonoBehaviour
{
    [Header("Important game values")]
    [SerializeField]
    [Tooltip("Current position of the snake head in the grid")]
    private Vector2Int gridPosition;
    [SerializeField]
    [Tooltip("Where the grid starts according to the world position")]
    Vector2Int initGridPos;
    [SerializeField]
    [Tooltip("Where the snake will start in the grid")]
    Vector2Int initSnakePos;

    [Header("Movement Data")]
    [SerializeField]
    [Tooltip("Quantity of food that needs to be eaten to finish game")]
    private int maxSnakeBodySize = 4;
    [SerializeField]
    private float gridMoveTimerMax = 1f;
    SpriteRenderer spriteRenderer;
    private int snakeBodySize;
    private int qtyFoodEaten;
    private List<Vector2Int> snakeMovePositionList;
    [SerializeField]
    Joystick joystick;
    //States
    bool firstInputGiven = false;
    private bool canBeControlled = false;
    private bool snakeEaterMode = false;

    //Grid data:
    
    private Vector2Int gridMoveDirection;
    private Vector2Int nextMoveDirection;
    Vector2 move;
    private float gridMoveTimer;
    LevelGrid levelGrid;
    int width;
    int height;

    [Header("Signals")]
    [SerializeField]
    private Signal lostSnake;
    [SerializeField]
    private Signal winSnake;

    [Header("Debu")]
    [SerializeField]
    bool imortal;
    [SerializeField]
    bool noWin;

    public Vector2Int GridMoveDirection { get => gridMoveDirection; set => gridMoveDirection = value; }
    public bool FirstInputGiven { get => firstInputGiven; set => firstInputGiven = value; }

    public void Setup(LevelGrid levelGrid, int width, int height)
    {
        this.levelGrid = levelGrid;
        this.width = width;
        this.height = height;
    }

    public void ActivateSnake()
    {
        canBeControlled = true;
        spriteRenderer.enabled = true;
    }

    public void DisableSnake()
    {
        canBeControlled = false;
        spriteRenderer.enabled = false;

    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ActivateSnake();
        RestartSnake();

        //MobileButtonsManager.instance.CurrState = new SnakeMobileInput(this);

        //MobileButtonsManager.instance.CurrState.OnBeginState();
    }

    private void Start()
    {
        MobileButtonsManager.instance.CurrState = new SnakeMobileInput(this);

        MobileButtonsManager.instance.CurrState.OnBeginState();
    }
  
    public void RestartSnake()
    {
        gridPosition = new Vector2Int(initGridPos.x + initSnakePos.x, initGridPos.y + initSnakePos.y);
        gridMoveTimer = gridMoveTimerMax;
        nextMoveDirection = new Vector2Int(0, 0);
        GridMoveDirection = new Vector2Int(0, 0); //default movement: zero


        

        //snakeBodySize = 0;
        firstInputGiven = false;
        qtyFoodEaten = 0;
        snakeMovePositionList = new List<Vector2Int>();

    }

    public void ChangeInputToSnake()
    {
        MobileButtonsManager.instance.CurrState = new SnakeMobileInput(this);

        MobileButtonsManager.instance.CurrState.OnBeginState();
    }

    private void Update()
    {
        //for debug purposes:
        if (Input.GetKeyDown(KeyCode.R))
        {
            snakeBodySize = 0;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            snakeEaterMode = !snakeEaterMode;
        }

        //movement handling:
        if (!canBeControlled) { return; }
        HandleInput();
        HandleGridMovement();

    }

    private void HandleInput()
    {
#if UNITY_STANDALONE || UNITY_EDITOR

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        switch (v)
        {
            case 1:
                nextMoveDirection = Vector2Int.up;
                break;
            case -1:
                nextMoveDirection = Vector2Int.down;
                break;
        }

        switch (h)
        {
            case 1:
                nextMoveDirection = Vector2Int.right;
                break;
            case -1:
                nextMoveDirection = Vector2Int.left;
                break;
        }
#else
        float h = joystick.Horizontal;
        float v = joystick.Vertical;

        Debug.Log(h);
        Debug.Log(v);

        switch (v)
        {
            case 1:
                nextMoveDirection = Vector2Int.up;
                break;
            case -1:
                nextMoveDirection = Vector2Int.down;
                break;
        }

        switch (h)
        {
            case 1:
                nextMoveDirection = Vector2Int.right;
                break;
            case -1:
                nextMoveDirection = Vector2Int.left;
                break;
        }

#endif

        //Debug.Log("mine: " + move);
        /*if (Input.GetKeyDown(KeyCode.UpArrow))
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
        }*/

        if (!firstInputGiven)
        {
            if (nextMoveDirection != Vector2Int.zero)
            {
                firstInputGiven = true;
            }
        }

        //if(v == 0 )
    }

    private void HandleDirection()
    {
        if (nextMoveDirection == Vector2Int.up && GridMoveDirection != Vector2Int.down)
        {
            GridMoveDirection = Vector2Int.up;
        }
        else if (nextMoveDirection == Vector2Int.down && GridMoveDirection != Vector2Int.up)
        {
            GridMoveDirection = Vector2Int.down;
        }
        else if (nextMoveDirection == Vector2Int.right && GridMoveDirection != Vector2Int.left)
        {
            GridMoveDirection = Vector2Int.right;
        }
        else if (nextMoveDirection == Vector2Int.left && GridMoveDirection != Vector2Int.right)
        {
            GridMoveDirection = Vector2Int.left;
        }
        nextMoveDirection = new Vector2Int(0, 0);
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;



        if (gridMoveTimer > gridMoveTimerMax)
        {
            snakeMovePositionList.Insert(0, gridPosition);
            //snakeMovePositionList.Insert(0, move);

            HandleDirection();

            gridPosition += GridMoveDirection;
            gridMoveTimer = 0f;


            bool cellFull = Board.instance.IsCellFull(gridPosition.x, gridPosition.y - initGridPos.y);

            if (cellFull)
            {
                if (snakeEaterMode)
                {
                    Board.instance.CleanCell(gridPosition.x, gridPosition.y - initGridPos.y);
                }
                else
                {
                    Debug.Log("Cell was full");
                    HandleDeath();
                }

            }


            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            //collide with itself
            for (int i = 0; i < snakeMovePositionList.Count; i++)
            {
                if (snakeMovePositionList[i] == gridPosition && firstInputGiven)
                {

                    Debug.Log("Collided with itself");
                    HandleDeath();
                }

            }

            //out of limits
            if (gridPosition.x > initGridPos.x + width ||
                    gridPosition.x < initGridPos.x ||
                    gridPosition.y > initGridPos.y + height ||
                    gridPosition.y < initGridPos.y
                    )
            {

                Debug.Log("Out of limits!");
                HandleDeath();
            }

            //it does not matter the order (from beginning to end or from end to beginning)
            if (firstInputGiven)
            {
                for (int i = snakeMovePositionList.Count - 1; i >= 0; i--)
                {
                    Vector2Int snakeMovePosition = snakeMovePositionList[i];
                    World_Sprite world_sprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one, Color.white);
                    world_sprite.SetSprite(GameAssets.Instance.snakeHeadSprite);
                    //create the sprite of the snake's part
                    FunctionTimer.Create(world_sprite.DestroySelf, gridMoveTimerMax);
                    //every time we move, destroy the body parts
                }
            }

            //old set position
            transform.position = new Vector3(gridPosition.x, gridPosition.y, 0);
            //new set position
            //transform.position = new Vector3(move.x, move.y, 0);
            //transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                snakeBodySize++;
                qtyFoodEaten++;

                if (qtyFoodEaten >= maxSnakeBodySize)
                {
                    HandleVictory();
                }
            }
        }
    }

    public void HandleDeath()
    {

#if UNITY_EDITOR
        if (imortal)
            return;
#endif
        MobileButtonsManager.instance.CurrState = null;
        Debug.Log(MobileButtonsManager.instance.CurrState);
        RestartSnake();
        DisableSnake();
        lostSnake.Raise();

        
        
        //MobileButtonsManager.instance.CurrState.OnBeginState();
    }

    public void HandleVictory()
    {
        
#if UNITY_EDITOR
        if (noWin)
            return;
#endif
        RestartSnake();
        DisableSnake();
        winSnake.Raise();

        MobileButtonsManager.instance.CurrState = null;
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
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
