using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;

public class FirebaseAnalytics : MonoBehaviour
{
    [Header("Element")]
    public TestProfileSO testProfileSO;

    [Header("Setting")]
    public bool enableDebugLog = false;

    public void EventAdImpression(GoogleMobileAds.Api.AdValue adValue)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        double revenue = adValue.Value / 1000000f;
        Parameter[] impressionParameters = new Parameter[] {
            new Parameter("ad_platform", "AdMob"),
            new Parameter("value", revenue),
            new Parameter("currency", adValue.CurrencyCode),
        };

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            "ad_impression",
            impressionParameters
        );

        Log($"ad_impression, revenue {revenue}");
    }

    public void EventLevelStart(int level)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        Parameter levelParam = new Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level.ToString());

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventLevelStart,
            levelParam
        );

        Log($"level_start, level {level}");
    }

    public void EventLevelEnd(int level, float elapsedSeconds)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        Parameter levelParam = new Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level.ToString());

        Parameter elapsedSecondsParam = new Parameter("elapsed_seconds", elapsedSeconds);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventLevelEnd,
            new Parameter[] { levelParam, elapsedSecondsParam }
        );

        Log($"level_end, level {level}, elapsedSeconds {elapsedSeconds}");
    }

    public void EventLevelUp(int level)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        Parameter levelParam = new Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level.ToString());

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            Firebase.Analytics.FirebaseAnalytics.EventLevelUp,
            levelParam
        );

        Log($"level_up, level {level}");
    }

    public void EventSpendSupportItem(ItemsData.ItemType itemType, int level, int amount)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        Parameter itemTypeParam = new Parameter("item_type", itemType switch
        {
            ItemsData.ItemType.Hint => "hint",
            ItemsData.ItemType.Shuffle => "shuffle",
            ItemsData.ItemType.Timer => "timer",
            _ => ""
        });

        Parameter levelParam = new Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level.ToString());

        Parameter amountParam = new Parameter("amount", amount);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            "spend_support_item",
            new Parameter[] { itemTypeParam, levelParam, amountParam }
        );

        Log($"spend_support_item, itemType {itemType}, level {level}, amount {amount}");
    }

    public void EventEarnSupportItem(ItemsData.ItemType itemType, SupportItemsController.TransactionType transactionType, int level, int amount)
    {
        if (testProfileSO.IsDeviceTest())
            return;

        Parameter itemTypeParam = new Parameter("item_type", itemType switch
        {
            ItemsData.ItemType.Hint => "hint",
            ItemsData.ItemType.Shuffle => "shuffle",
            ItemsData.ItemType.Timer => "timer",
            _ => ""
        });

        Parameter transactionTypeParam = new Parameter("transaction_type", transactionType switch
        {
            SupportItemsController.TransactionType.Ad => "ad",
            SupportItemsController.TransactionType.Coin => "coin",
            _ => ""
        });

        Parameter levelParam = new Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterLevel, level.ToString());

        Parameter amountParam = new Parameter("amount", amount);

        Firebase.Analytics.FirebaseAnalytics.LogEvent(
            "spend_support_item",
            new Parameter[] { itemTypeParam, transactionTypeParam, levelParam, amountParam }
        );

        Log($"earn_support_item, itemType {itemType}, transactionType {transactionType}, level {level}, amount {amount}");
    }

    private void Log(string data)
    {
        if (enableDebugLog)
        {
            Debug.Log($"FIREBASE ANALYTICS: {data}");
        }
    }
}
