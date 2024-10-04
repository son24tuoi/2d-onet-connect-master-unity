using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimeSystem
{
    [Tooltip("Time by seconds")]
    public int maxPlayTime;

    [Range(0, 1)] public float oneStar;
    [Range(0, 1)] public float twoStar;
    [Range(0, 1)] public float threeStar;

    public float TimeOneStar => oneStar * maxPlayTime;

    public float TimeTwoStar => twoStar * maxPlayTime;

    public float TimeThreeStar => threeStar * maxPlayTime;

    public string GetTimePlay()
    {
        int minutes = maxPlayTime / 60;
        return minutes.ToString("00") + ":" + (maxPlayTime - minutes * 60).ToString("00");
    }
}
