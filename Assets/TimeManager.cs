using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float timer;

    private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
    }

    void Update()
    {
        timer += Time.deltaTime;
        UpdateTimer();
    }

    void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        gameController.GetComponent<UIManager>().timerText.text = timeString;
    }
}
