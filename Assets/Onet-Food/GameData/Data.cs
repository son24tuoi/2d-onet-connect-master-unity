using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Data
{
    public LevelData levelData;
    public ItemsData itemsData;
    public AudioData audioData;
    public VibrationData vibrationData;
    public ChestData chestData;
    public IAPData iapData;
    public AdData adData;
    public ShopData shopData;
    public TimeData timeData;

    [SerializeField] private bool inAppReview;
    [SerializeField] private bool thankYouForPlaying;

    public bool InAppReview
    {
        get => inAppReview;
        set => inAppReview = value;
    }

    public bool ThankYouForPlaying
    {
        get => thankYouForPlaying;
        set => thankYouForPlaying = value;
    }

    public Data()
    {
        levelData = new LevelData();
        itemsData = new ItemsData();
        audioData = new AudioData();
        vibrationData = new VibrationData();
        chestData = new ChestData();
        iapData = new IAPData();
        adData = new AdData();
        shopData = new ShopData();
        timeData = new TimeData();

        inAppReview = false;
    }
}
