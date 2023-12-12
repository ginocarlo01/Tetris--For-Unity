using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private UIManager gameController;

    private int score;

    private bool updatedHighScore;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();

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

        if(score > JsonReadWriteSystem.INSTANCE.playerData.maxScore && !updatedHighScore)
        {
            updatedHighScore = true;
            UpdateHighScoreTxt();
        }

        UpdateScoreText();
    }


    void UpdateScoreText()
    {
        gameController.scoreText.text = "Score : " + score;
    }

    void UpdateMaxScoreText()
    {
        gameController.maxScoreText.text = "Best: " + JsonReadWriteSystem.INSTANCE.playerData.maxScore;
    }

    void UpdateHighScoreTxt()
    {
        gameController.highScoreText.text = "New High Score";
    }

    public int GetScore()
    {
        return score;
    }
}
