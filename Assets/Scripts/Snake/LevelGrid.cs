using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using UnityEditor;
public class LevelGrid 
{
    
    private Vector2Int foodGridPosition;

    int width;
    int height;

    int initPosX;
    int initPosY;

    GameObject foodGameObject;

    Snake snake;


    public LevelGrid(int width, int height,int initPosX, int initPosY)
    {
        this.width = width;
        this.height= height;
        this.initPosX = initPosX;
        this.initPosY = initPosY;

        //FunctionPeriodic.Create(SpawnFood, 1f);
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }

    public void SpawnFood()
    {
        int[] listOfRowsOccupied = Board.instance.GetListOfRowsOccupied();

        if (listOfRowsOccupied[listOfRowsOccupied.Length - 2] == 1) { return; }
        
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(initPosX, initPosX + width), Random.Range(initPosY, initPosY + height));
        }
        while (snake.GetFullSnakeGridPosition().IndexOf(foodGridPosition) != -1 || Board.instance.GetListOfRowsOccupied()[foodGridPosition.y - initPosY] == 1); //gonna keep trying to find a new position while it is the same as the snake
        

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));

        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.Instance.foodSprite;

        foodGameObject.transform.position = new Vector3( foodGridPosition.x,foodGridPosition.y, 0);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DestroyFood(){
        
        if(foodGameObject != null){
            Object.Destroy(foodGameObject);
        }
        
    }
}
