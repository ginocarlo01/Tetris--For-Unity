using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] screens;
    [SerializeField]
    string defaultScreen;
    [Header("Debug")]
    [SerializeField]
    string debugScreen;
    [SerializeField]
    bool enableDebugScreen;
    [SerializeField]
    bool disableScreen;
    [SerializeField]
    bool enableDefaultScreen;
    private void OnValidate()
    {
        if (disableScreen)
        {
            DisableScreen();
            disableScreen = false;
        }

        if (enableDefaultScreen)
        {
            EnableScreen(defaultScreen);
            enableDefaultScreen = false;
        }

        if (enableDebugScreen)
        {
            EnableScreen(debugScreen);
            enableDebugScreen = false;
        }
    }
    private void Start()
    {
        if (screens.Length > 0)
        {
        /*foreach(GameObject i in screens) 
        {
            i.SetActive(false);
        }*/
        
            EnableScreen(defaultScreen);
        }
    }

    public void EnableScreen(string name="")
    {
        for(int i = 0; i < screens.Length; i++)
        {
            

            if(screens[i].name == name) 
            {
                screens[i].SetActive(true);
            }
            else
            {
                screens[i].SetActive(false);
            }
        }
    }

    void DisableScreen()
    {
        foreach (GameObject i in screens)
        {
            i.SetActive(false);
        }
    }
}
