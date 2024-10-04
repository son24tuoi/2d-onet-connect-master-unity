using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AdData : IAdData
{
    [Header("Cooldown")]
    [SerializeField] private long interstitialAdTime;

    public long InterstitialAdTime
    {
        get => interstitialAdTime;
        set => interstitialAdTime = value;
    }

    public AdData()
    {
        interstitialAdTime = 0;
    }

    public void SaveInterstitialTime(DateTime dateTime)
    {
        interstitialAdTime = dateTime.Ticks;
    }

    public void SaveInterstitialTime(long ticks)
    {
        interstitialAdTime = ticks;
    }
}

public interface IAdData
{
    public long InterstitialAdTime { get; }
    public void SaveInterstitialTime(DateTime dateTime);
    public void SaveInterstitialTime(long ticks);
}