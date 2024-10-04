using System;
using UnityEngine;

[Serializable]
public class ChestData
{
    [SerializeField] private int chestIndex;

    public int ChestIndex
    {
        get { return chestIndex; }
    }

    public ChestData()
    {
        chestIndex = 0;
    }

    public void IncreaseChestIndex()
    {
        chestIndex++;
    }
}