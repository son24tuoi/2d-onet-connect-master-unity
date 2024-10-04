using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class RemoveAdIAP : BaseIAP
{
    public override void OnPurchaseCompleted(Product purchasedProduct)
    {
        base.OnPurchaseCompleted(purchasedProduct);

        IAPManager.Instance.RemoveAd();
    }

    public override void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        base.OnPurchaseFailed(product, failureDescription);
    }


}
