using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class PackIAP : BaseIAP
{
    [Header("Datas")]
    public RewardProfileSO rewardProfileSO;

    public override void OnPurchaseCompleted(Product purchasedProduct)
    {
        base.OnPurchaseCompleted(purchasedProduct);

        IAPManager.Instance.GetReward(rewardProfileSO);
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        base.OnPurchaseFailed(product, failureDescription);
    }
}
