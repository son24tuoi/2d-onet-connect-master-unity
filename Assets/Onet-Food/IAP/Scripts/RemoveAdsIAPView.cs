using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RemoveAdsIAPView : MonoBehaviour
{
    private void OnEnable()
    {
        IAPManager.OnRemoveAdEvent += RemoveAdCallback;

        if (DataManager.Instance.Data.iapData.RemoveAd)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        IAPManager.OnRemoveAdEvent -= RemoveAdCallback;
    }

    private void RemoveAdCallback()
    {
        DelayCall delayCall = new DelayCall(0.1f, () =>
        {
            gameObject.SetActive(false);
        },
        ignoreTimeScale: true);

        delayCall.StartDelayCall();
    }
}
