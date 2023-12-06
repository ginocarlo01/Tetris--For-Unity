using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private GameObject optionsUI;
    private Animator optionsUIAnim;
    private bool optionsWasPressed = false;

    private void Start()
    {
        InitUI();
    }

    public void HandleStartButtonOnClickEvent(string _scene)
    {
        SceneManager.LoadScene(_scene);
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

    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }

    private void InitUI()
    {
        //optionsUI = GameObject.FindGameObjectWithTag("OptionsUI");
        optionsUIAnim = GameObject.FindGameObjectWithTag("OptionsUI").GetComponent<Animator>();
        //optionsUI.SetActive(false);
    }
}
