using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private GameObject gameController;

    private int score;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");

        UpdateScoreText();

        UpdateMaxScoreText();
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int newScore)
    {
        this.score += newScore;

        UpdateScoreText();
    }


    void UpdateScoreText()
    {
        gameController.GetComponent<UIManager>().scoreText.text = "Score : " + score;
    }

    void UpdateMaxScoreText()
    {
        gameController.GetComponent<UIManager>().maxScoreText.text = "Best: " + JsonReadWriteSystem.INSTANCE.playerData.maxScore;
    }

    public int GetScore()
    {
        return score;
    }
}
