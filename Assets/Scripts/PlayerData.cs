using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{

    public int maxScore;
    public PlayerData(int maxScore=0)
    {
        this.maxScore = maxScore;
    }

    public int MaxScore { get => maxScore; set => maxScore = value; }

    
}
