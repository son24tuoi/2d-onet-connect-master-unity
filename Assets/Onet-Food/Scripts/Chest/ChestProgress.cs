using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestProgress : MyMonoBehaviour
{
    public static event Action<int, int> OnUpdateChestEvent;

    [Header("Element")]
    public OpenChestCanvas openChestCanvas;
    public SupportItemsController supportItemsController;

    [Space(10)]
    public RewardProfileSO rewardProfileSO;
    public ChestProfileSO chestProfileSO;

    private List<Reward> m_rewards;

    public ItemsData ItemsData => DataManager.Data.itemsData;
    public ChestData ChestData => DataManager.Data.chestData;

    private void Start()
    {
        ChestProgressView.OnChestEvent += ChestCallback;
        RewardCardView.OnShowEvent += ShowRewardCallback;
        OpenChestView.OnCloseEvent += CloseOpenChestCallback;
    }

    private void OnDestroy()
    {
        ChestProgressView.OnChestEvent -= ChestCallback;
        RewardCardView.OnShowEvent -= ShowRewardCallback;
        OpenChestView.OnCloseEvent -= CloseOpenChestCallback;
    }

    private void ChestCallback()
    {
        ShowOpenChestCanvas(rewardProfileSO);

        m_rewards = new List<Reward>();
    }

    private void ShowRewardCallback(Reward reward)
    {
        m_rewards.Add(reward);
    }

    private void CloseOpenChestCallback()
    {
        for (int i = 0; i < m_rewards.Count; i++)
        {
            ItemsData.AddItem(m_rewards[i].itemType, m_rewards[i].amount);
        }

        ItemsData.AddItem(ItemsData.ItemType.Star, -chestProfileSO.GetRequire(ChestData.ChestIndex));
        ChestData.IncreaseChestIndex();

        DataManager.SaveData();

        supportItemsController.UpdateButtons();
        OnUpdateChestEvent?.Invoke(ChestData.ChestIndex, ItemsData.StarAmount);

        openChestCanvas.OnClose();
    }

    public void ShowOpenChestCanvas(RewardProfileSO rewardProfileSO)
    {
        openChestCanvas.Init(rewardProfileSO);
        openChestCanvas.gameObject.SetActive(true);
    }
}
