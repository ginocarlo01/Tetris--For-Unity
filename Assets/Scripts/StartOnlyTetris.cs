using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartOnlyTetris : MonoBehaviour
{
    [SerializeField]
    Signal startTetrisSignal;
    void Start()
    {
        startTetrisSignal.Raise();
    }

    
}
