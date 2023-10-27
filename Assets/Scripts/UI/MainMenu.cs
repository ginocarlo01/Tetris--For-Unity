using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string nextScene;
    
    public void HandleStartButtonOnClickEvent()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void HandleOptionsButtonOnClickEvent()
    {
        //add a search for the object by its tag!
    }

    public void HandleQuitButtonOnClickEvent()
    {
        Application.Quit();
    }
}
