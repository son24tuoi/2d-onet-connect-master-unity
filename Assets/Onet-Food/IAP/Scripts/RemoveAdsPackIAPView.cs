using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RemoveAdsPackIAPView : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RewardView rewardView;

    [Header("Datas")]
    public ItemsProfileSO itemsProfileSO;
    public RewardProfileSO rewardProfileSO;

    private Reward reward;

    private void OnEnable()
    {
        IAPManager.OnRemoveAdEvent += RemoveAdCallback;
    }

    private void Start()
    {
        Setup();
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

    public void Setup()
    {
        if (DataManager.Instance.Data.iapData.RemoveAd)
        {
            gameObject.SetActive(false);
        }

        reward = rewardProfileSO.GetReward(0);

        if (reward == null)
            return;

        rewardView.Setup(itemsProfileSO.GetIcon(reward.itemType), reward.amount);
    }
}
