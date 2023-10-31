using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    private GameObject optionsUI;

    private void Start()
    {
        InitUI();
    }

    public void HandleStartButtonOnClickEvent()
    {
        SceneManager.LoadScene(nextScene);
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
