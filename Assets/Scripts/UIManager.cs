using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    public TextMeshProUGUI maxScoreText;

    public TextMeshProUGUI timerText;

    public TextMeshProUGUI highScoreText;

    public TextMeshProUGUI resultsTxt;

    private bool isVisible;

    private void Start()
    {
        Cursor.visible = isVisible;
        highScoreText.text = "";
        resultsTxt.text = "";
    }

    public void UpdateCursor()
    {
        isVisible = !isVisible;
        Cursor.visible = isVisible;
    }

}
