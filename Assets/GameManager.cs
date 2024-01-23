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
    [SerializeField]
    GameObject gameOverScreen;
    ScreensManager screensManager;
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void GameOver()
    {
        if (screensManager == null)
            screensManager.EnableScreen("GameOver");
    }
}
