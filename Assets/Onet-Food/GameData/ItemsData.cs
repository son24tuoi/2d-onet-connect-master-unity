using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class ItemsData
{
    [SerializedDictionary("Id", "Amount")]
    [SerializeField] private SerializedDictionary<int, int> itemQuantities;

    public enum ItemType
    {
        Hint = 0,
        Shuffle = 1,
        Timer = 2,
        Star = 3,
        Coin = 4,
    }

    public int StarAmount
    {
        get => AmountItem((int)ItemType.Star);
    }

    public int CoinAmount
    {
        get => AmountItem((int)ItemType.Coin);
    }

    public ItemsData()
    {
        itemQuantities = new SerializedDictionary<int, int>
        {
            { (int)ItemType.Hint, 5 },
            { (int)ItemType.Shuffle, 5 },
            { (int)ItemType.Timer, 5 }
        };
    }

    public int GetItem(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Hint => AmountItem((int)ItemType.Hint),
            ItemType.Shuffle => AmountItem((int)ItemType.Shuffle),
            ItemType.Timer => AmountItem((int)ItemType.Timer),
            ItemType.Star => AmountItem((int)ItemType.Star),
            ItemType.Coin => AmountItem((int)ItemType.Coin),
            _ => 0,
        };
    }

    public void AddItem(int key, int value)
    {
        if (itemQuantities.ContainsKey(key))
        {
            itemQuantities[key] += value;
        }
        else
        {
            itemQuantities.Add(key, value);
        }

        if (itemQuantities[key] < 0)
        {
            itemQuantities[key] = 0;
        }
    }

    public void AddItem(ItemType itemType, int value)
    {
        AddItem((int)itemType, value);
    }

    public int AmountItem(int key)
    {
        return itemQuantities.ContainsKey(key) ? itemQuantities[key] : 0;
    }

    public int AmountItem(ItemType itemType)
    {
        return AmountItem((int)itemType);
    }
}