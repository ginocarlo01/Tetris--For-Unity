using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum GameState
{
    Pause,
    Play,
    GameOver,
    Menu
}

public class GameManager : MonoBehaviour
{
    //[SerializeField]
    //GameObject gameOverScreen;
    ScreensManager screensManager;
    public static GameManager instance;
    public GameState state;
    private void Awake()
    {
        instance = this;
        screensManager  =  FindObjectOfType<ScreensManager>();
        state = GameState.Play;
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
    }

    public void Menu()
    {
        state = GameState.Menu;
        if (screensManager != null)
            screensManager.EnableScreen("MenuSreen");
    }
}
