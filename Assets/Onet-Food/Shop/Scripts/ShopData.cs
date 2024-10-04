using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShopData
{
    [SerializeField] private bool isFreeCoinReceived;

    public bool IsFreeCoinReceived
    {
        get => isFreeCoinReceived;
        set => isFreeCoinReceived = value;
    }

    public ShopData()
    {
        isFreeCoinReceived = false;
    }
}