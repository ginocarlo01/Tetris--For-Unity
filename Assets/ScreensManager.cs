using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] screens;
    [SerializeField]
    string defaultScreen;
    private void Start()
    {
        foreach(GameObject i in screens) 
        {
            i.SetActive(false);
        }
        if (screens.Length > 0)
        {
            EnableScreen(defaultScreen);
        }
    }

    public void EnableScreen(string name="")
    {
        for(int i = 0; i < screens.Length; i++)
        {
            screens[i].SetActive(false);

            if(screens[i].name == name) 
            {
                screens[i].SetActive(true);
            }
        }
    }
}