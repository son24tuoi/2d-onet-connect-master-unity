using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemType = ItemsData.ItemType;

public class SupportItemsController : MyMonoBehaviour, IEventHandlerWithData
{
    [Header("Element")]
    [SerializeField] private LevelController levelController;

    [Space(5)]
    [SerializeField] private SupportItemButton[] supportItemButtons;

    [Space(10)]
    public GameProfileSO gameProfileSO;

    public enum TransactionType
    {
        Coin = 0,
        Ad = 1
    }

    private int m_price = 1;

    public ItemsData ItemsData => DataManager.Data.itemsData;

    private void Start()
    {
        SupportItemBuyView.OnBuyEvent += Buy;

        EventManager.Instance.Subcribe(EventID.UpdateItem, this);
        EventManager.Instance.Subcribe(EventID.UseSupportItem, this);
    }

    private void OnDestroy()
    {
        SupportItemBuyView.OnBuyEvent -= Buy;

        EventManager.Instance.Unsubcribe(EventID.UpdateItem, this);
        EventManager.Instance.Unsubcribe(EventID.UseSupportItem, this);
    }

    public void UseSupportItem(ItemType itemType)
    {
        if (ItemsData.AmountItem(itemType) >= 1)
        {
            DataManager.AddItem(itemType, -1);
            DataManager.SaveData();

            levelController.UseSupportItem(itemType);
            UpdateButtons();

        }
        else
        {
            ShowShop(itemType);
        }
    }

    public void ShowShop(ItemType itemType)
    {
        EventManager.Instance.Trigger(new EventData<ItemType>(
            EventID.SupportItemShop,
            itemType
        ));
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < supportItemButtons.Length; i++)
        {
            supportItemButtons[i].Init();
        }
    }

    public void Buy(TransactionType transactionType, ItemType itemType)
    {
        switch (transactionType)
        {
            case TransactionType.Coin:
                TransactionCoin(itemType);
                break;
            case TransactionType.Ad:
                TransactionAd(itemType);
                break;
        }

        UpdateButtons();
    }

    public void TransactionCoin(ItemType itemType)
    {
        if (DataManager.CanBuy(m_price))
        {
            DataManager.SpendCoin(m_price);
            DataManager.AddItem(itemType, 1);
            DataManager.SaveData();

            TriggerEventNotification(new NotificationData(
                $"Get 1 {itemType} item",
                NotificationColorType.Green
            ));
        }
        else
        {
            TriggerEventNotification(new NotificationData(
                "Not enough coin",
                NotificationColorType.Red
            ));
        }
    }

    public void TransactionAd(ItemType itemType)
    {
        bool reward = false;
        AdManager.ShowRewardedAd(onReward: () =>
        {
            reward = true;

            DataManager.AddItem(itemType, 1);
            UpdateButtons();

            TriggerEventNotification(new NotificationData(
                reward ? $"Get 1 {itemType} item" : "Not rewarded ad",
                reward ? NotificationColorType.Green : NotificationColorType.Red
            ));
        }, onClose: () =>
        {

        }, onNotReady: () =>
        {
            TriggerEventNotification(new NotificationData(
                "Ad are not available",
                NotificationColorType.Yellow
            ));
        });
    }

    public void TriggerEventNotification(NotificationData notificationData)
    {
        EventManager.Instance.Trigger(new EventData<NotificationData>(
            eventID: EventID.Notification,
            notificationData
        ));
    }

    private void UpdateButton(ItemStack itemStack)
    {
        int length = supportItemButtons.Length;
        for (int i = 0; i < length; i++)
        {
            if (supportItemButtons[i].ItemType == itemStack.itemType)
            {
                supportItemButtons[i].SetPlusIcon(itemStack.amount);
            }
        }
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.UpdateItem:
                if (eventData.data is ItemStack itemStack)
                {
                    UpdateButton(itemStack);
                }
                break;

            case EventID.UseSupportItem:
                if (eventData.data is ItemType itemType)
                {
                    UseSupportItem(itemType);
                }
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }
}
