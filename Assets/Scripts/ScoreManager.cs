using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private UIManager gameController;

    private int score, maxScore;

    private bool updatedHighScore;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<UIManager>();

        UpdateScoreText();

        UpdateMaxScoreText();
        maxScore = JsonReadWriteSystem.INSTANCE.playerData.MaxScore;
    }

    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int newScore)
    {
        this.score += newScore;

        if(score > JsonReadWriteSystem.INSTANCE.playerData.MaxScore && !updatedHighScore)
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
        gameController.maxScoreText.text = "Best: " + score;//JsonReadWriteSystem.INSTANCE.playerData.MaxScore;
    }

    void UpdateHighScoreTxt()
    {
        gameController.highScoreText.text = "New High Score";
    }

    public int GetScore()
    {
        return score;
    }
    public int MaxScore { get => maxScore; set => maxScore = value; }
    //public int MaxScore1 { get => maxScore; set => maxScore = value; }
}
