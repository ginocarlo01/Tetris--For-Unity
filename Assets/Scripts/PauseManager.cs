using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    [SerializeField]
    private Signal pauseSignal;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            pauseSignal.Raise();

        }
    }
}
