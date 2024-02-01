using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class InitialGame : ScriptableObject
{
    [SerializeField]
    bool initial;
    public bool Initial { get => initial; set => initial = value; }
}
