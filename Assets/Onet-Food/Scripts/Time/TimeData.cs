using System;
using UnityEngine;

[Serializable]
public class TimeData
{
    [SerializeField] private long lastCheckedTime;

    public long LastCheckedTime
    {
        get => lastCheckedTime;
        set => lastCheckedTime = value;
    }

    public TimeData()
    {
        lastCheckedTime = 0;
    }

    public bool IsNewDay()
    {
        DateTime lastTime = new DateTime(LastCheckedTime);

        if (lastTime.Year < DateTime.Now.Year)
            return true;

        return lastTime.DayOfYear < DateTime.Now.DayOfYear;
    }
}