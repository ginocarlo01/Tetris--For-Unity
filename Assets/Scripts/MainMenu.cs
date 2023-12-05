using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private GameObject optionsUI;

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
        optionsUI.SetActive(!optionsUI.activeSelf);
    }

    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }

    private void InitUI()
    {
        optionsUI = GameObject.FindGameObjectWithTag("OptionsUI");
        optionsUI.SetActive(false);
    }
}
