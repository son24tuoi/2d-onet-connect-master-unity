using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public ItemsData.ItemType itemType;
    public int amount;

    public Reward(ItemsData.ItemType itemType = ItemsData.ItemType.Coin, int amount = 0)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}
