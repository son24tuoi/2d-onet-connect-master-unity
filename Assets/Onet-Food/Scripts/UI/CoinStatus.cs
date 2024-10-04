using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CoinStatus : MyMonoBehaviour, IEventHandlerWithData, IEventHandler
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject freeLabel;

    private void OnEnable()
    {
        EventManager.Instance.Subcribe(EventID.UpdateItem, this as IEventHandlerWithData);
        EventManager.Instance.Subcribe(EventID.ReceiveFreeCoin, this as IEventHandler);

        SetAmountText(DataManager.Data.itemsData.CoinAmount);

        SetupFreeLabel();
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubcribe(EventID.UpdateItem, this as IEventHandlerWithData);
        EventManager.Instance.Unsubcribe(EventID.ReceiveFreeCoin, this as IEventHandler);
    }

    public void SetAmountText(int value)
    {
        amountText.SetText((value == 0) ? "0" : value.ToString("#,###"));
    }

    public void SetupFreeLabel()
    {
        freeLabel.SetActive(!DataManager.Data.shopData.IsFreeCoinReceived);
    }

    public void OnClickAddButton()
    {
        EventManager.Instance.Trigger(EventID.ShopCanvas);
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.UpdateItem:
                if (eventData.data is ItemStack itemStack)
                {
                    if (itemStack.itemType == ItemsData.ItemType.Coin)
                    {
                        SetAmountText(itemStack.amount);
                    }
                }
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }

    public void EventHandler(EventID eventID)
    {
        switch (eventID)
        {
            case EventID.ReceiveFreeCoin:
                SetupFreeLabel();
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }
}
