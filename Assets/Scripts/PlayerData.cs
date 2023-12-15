using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData 
{

    public int maxScore;

    public bool soundOn;
    public PlayerData(int maxScore=0, bool soundOn=true)
    {
        this.maxScore = maxScore;
        this.SoundOn = soundOn;
    }

    public int MaxScore { get => maxScore; set => maxScore = value; }
    public bool SoundOn { get => soundOn; set => soundOn = value; }
}
