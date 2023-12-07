using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindowGame : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;

    [SerializeField]
    private Animator optionsUIAnim;

    private bool optionsWasPressed = false;

    [SerializeField]
    private Signal pauseSignal;

    public void HandleStartButtonOnClickEvent()
    {
        pauseSignal.Raise();
    }

    public void HandleOptionsButtonOnClickEvent()
    {
        if (optionsWasPressed)
        {
            optionsWasPressed = false;
            optionsUIAnim.Play("HideOptionsUI");
        }
        else
        {
            optionsWasPressed = true;
            optionsUIAnim.Play("ShowOptionsUI");
        }
    }

    public void HandleQuitButtonOnClickEvent(string _scene)
    {
        SceneManager.LoadScene(_scene);
    }

    public void HandlePause()
    {
        optionsWasPressed = false;
        pauseUI.SetActive(!pauseUI.activeSelf);
    }
}
