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
}
