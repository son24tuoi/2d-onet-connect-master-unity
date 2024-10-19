using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DataController))]
public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    private DataController dataController;

    public Data Data
    {
        get => dataController.data;
    }

    public bool TutorialGamePlay
    {
        get => Data.levelData.TutorialGamePlay;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Init()
    {
        dataController = GetComponent<DataController>();
    }

    public void SaveData()
    {
        dataController.Save();
    }

    public void WinLevel(int levelIndex, int score)
    {
        if (levelIndex < 0)
            return;

        Data.levelData.IncreaseLevel();
        Data.itemsData.AddItem(ItemsData.ItemType.Star, score);

        SaveData();
    }

    public string GetLevelName()
    {
        return GetLevelName(Data.levelData.LevelIndex);
    }

    public string GetLevelName(int levelIndex)
    {
        return (levelIndex >= 0) ? (levelIndex + 1).ToString() : "Random";
    }

    public void ResetLevel()
    {
        Data.levelData.LevelIndex = 0;

        SaveData();
    }

    public void SaveTutorialGamePlay()
    {
        if (!Data.levelData.TutorialGamePlay)
        {
            Data.levelData.TutorialGamePlay = true;
            SaveData();
        }
    }

    public void AddItem(ItemsData.ItemType itemType, int amount)
    {
        Data.itemsData.AddItem(itemType, amount);

        EventManager.Instance.Trigger(new EventData<ItemStack>(
            EventID.UpdateItem,
            new ItemStack(
                itemType,
                Data.itemsData.AmountItem(itemType)
                )
            ));
    }

    public bool CanBuy(int price)
    {
        return Data.itemsData.CoinAmount >= price;
    }

    public void SpendCoin(int amount)
    {
        AddItem(ItemsData.ItemType.Coin, -amount);
    }

    public void CheckTime()
    {
        if (Data.timeData.IsNewDay())
        {
            Data.shopData.IsFreeCoinReceived = false;
        }

        Data.timeData.LastCheckedTime = DateTime.Now.Ticks;

        SaveData();
    }

    public void ReceiveFreeCoin()
    {
        if (Data.shopData.IsFreeCoinReceived)
            return;

        Data.shopData.IsFreeCoinReceived = true;
        SaveData();
        EventManager.Instance.Trigger(EventID.ReceiveFreeCoin);
    }

    public void SaveReward(RewardProfileSO rewardProfileSO)
    {
        int length = rewardProfileSO.Amount;
        for (int i = 0; i < length; i++)
        {
            AddItem(
                rewardProfileSO.rewards[i].itemType,
                rewardProfileSO.rewards[i].amount
            );
        }

        SaveData();
    }

    public void SetIsPlaying(bool isPlaying)
    {
        Data.levelData.IsPlaying = isPlaying;

        SaveData();
    }

    public void SetIsStartingPlay(bool isStartingPlay)
    {
        Data.levelData.IsStarttingPlay = isStartingPlay;

        SaveData();
    }

    public void UpdateProgressLevel(int[,] map, int[] idCards, float elapsedSeconds, int starsReceived, int amountMatch)
    {
        if (!Data.levelData.IsPlaying)
            return;

        Data.levelData.NodeGrid = new NodeGrid(map);
        Data.levelData.IdCards = idCards;
        Data.levelData.ElapsedSeconds = elapsedSeconds;
        Data.levelData.StarsReceived = starsReceived;
        Data.levelData.AmountMatch = amountMatch;

        SaveData();
    }
}
