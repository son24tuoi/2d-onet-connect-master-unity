using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class OutOfTimeContent : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI moreTimeText;
    [SerializeField] private TextMeshProUGUI streakText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject[] contents;

    [SerializeField] private UnityEvent reviveEvent;
    [SerializeField] private UnityEvent loseEvent;

    private void OnEnable()
    {
        Setup();
    }

    public void Setup()
    {
        moreTimeText.SetText($"<color=#FF76F2>FREE {Settings.MoreTimeRevive / 60} MINUTES</color>\nAND PLAY ON");
        streakText.SetText($"<color=#FF76F2>YOU WILL LOSE \nTHE WINNING STREAK:</color>\nLEVEL {DataManager.Instance.Data.levelData.LevelIndex}");
        priceText.SetText(Settings.PriceRevive.ToString());

        ShowContents(0);
    }

    public void ShowContents(int index)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            contents[i].SetActive(i == index);
        }
    }

    public void OnClickExitButton()
    {
        if (contents[0].activeSelf)
        {
            ShowContents(1);
        }
        else
        {
            loseEvent?.Invoke();
        }
    }

    public void OnClickBuyButton()
    {
        if (DataManager.Instance.CanBuy(Settings.PriceRevive))
        {
            DataManager.Instance.SpendCoin(Settings.PriceRevive);

            SuccessPurchase();
        }
        else
        {
            EventManager.Instance.Trigger(new EventData<NotificationData>(
                eventID: EventID.Notification,
                new NotificationData(
                    "Not enough coin",
                    NotificationColorType.Red)
            ));
        }
    }

    public void OnClickAdButton()
    {
        AdManager.Instance.ShowRewardedAd(
            onReward: SuccessPurchase,
            onClose: null,
            onNotReady: () =>
            {
                EventManager.Instance.Trigger(new EventData<NotificationData>(
                    eventID: EventID.Notification,
                    new NotificationData(
                        "Ad are not available",
                        NotificationColorType.Yellow)
                ));
            }
        );
    }

    private void SuccessPurchase()
    {
        EventManager.Instance.Trigger(EventID.Revive);

        reviveEvent?.Invoke();
    }
}
