using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifyShop : MonoBehaviour, IEventHandler
{
    [SerializeField] private GameObject notifyObject;

    private void OnEnable()
    {
        EventManager.Instance.Subcribe(EventID.ReceiveFreeCoin, this);
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubcribe(EventID.ReceiveFreeCoin, this);
    }

    public void Setup()
    {
        notifyObject.SetActive(!DataManager.Instance.Data.shopData.IsFreeCoinReceived);
    }

    public void EventHandler(EventID eventID)
    {
        switch (eventID)
        {
            case EventID.ReceiveFreeCoin:
                Setup();
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }
}
