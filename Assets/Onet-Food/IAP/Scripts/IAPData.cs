using System;
using UnityEngine;

[Serializable]
public class IAPData
{
    [SerializeField] private bool removeAd;

    public bool RemoveAd
    {
        get => removeAd;
        set => removeAd = value;
    }

    public IAPData()
    {
        removeAd = false;
    }
}