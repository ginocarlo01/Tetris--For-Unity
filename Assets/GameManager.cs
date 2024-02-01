using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum GameState
{
    None,
    Pause,
    Play,
    GameOver,
    Menu
}

public class GameManager : MonoBehaviour
{
    Snake snake;
    //[SerializeField]
    //GameObject gameOverScreen;
    ScreensManager screensManager;
    public static GameManager instance;
    public GameState state;
    [SerializeField]
    InitialGame initial;
    [SerializeField]
    bool fals;
    private void Awake()
    {
        instance = this;
   

        //instance = this;
        snake = FindObjectOfType<Snake>();  
        screensManager  =  FindObjectOfType<ScreensManager>();
        fals = initial.Initial;

        /*if (!fals)
        {
            Debug.Log("Menu");
            Menu();
      
        }*/
        if(fals)
        {
            Debug.Log("Play");
            PlayGame();
        }
    }
    public void GameOver()
    {
        state = GameState.GameOver;
        if (screensManager != null)
            screensManager.EnableScreen("GameOverScreen");
    }
    
    public void PlayGame()
    {
        state = GameState.Play;
        if (screensManager != null)
            screensManager.EnableScreen("GameScreen");
        //Snake snake = FindObjectOfType<Snake>();
        //snake.RestartSnake();
    }
    public void RestartGame()
    {
        //snake.ResetSnakeSize();
        //snake.RestartSnake();
        //snake.ActivateSnake();

        //state = GameState.Play;
        //if (screensManager != null)
        // screensManager.EnableScreen("GameScreen");
        initial.Initial = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SnakeTetris");
    }
    public void Menu()
    {
        //initial.Initial = false;
        state = GameState.Menu;
        if (screensManager != null)
            screensManager.EnableScreen("MenuSreen");
    }

    public void PauseGame()
    {
        state = GameState.Pause;
        if (screensManager != null)
            screensManager.EnableScreen("PauseScreen");
    }
}
