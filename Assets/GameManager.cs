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
    public void GameOver()
    {
        gameObject.SetActive(false);
    }
}
