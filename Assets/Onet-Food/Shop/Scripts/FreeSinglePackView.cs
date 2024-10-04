using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FreeSinglePackView : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private GameObject adIcon;
    [SerializeField] private GameObject notifyObject;

    [SerializeField] private UnityEvent onPurchaseCompletedEvent;

    [Header("Datas")]
    public ItemsProfileSO itemsProfileSO;
    public RewardProfileSO rewardProfileSO;

    private Reward reward;

    public void SetType(bool showAd)
    {
        adIcon.SetActive(showAd);
    }

    public void SetNotify(bool notify)
    {
        notifyObject.SetActive(notify);
    }

    public void Setup(RewardProfileSO rewardProfileSO)
    {
        this.rewardProfileSO = rewardProfileSO;
        Setup();
    }

    public void Setup()
    {
        reward = rewardProfileSO.GetReward(0);

        if (reward == null)
            return;

        icon.sprite = itemsProfileSO.GetIcon(reward.itemType);
        amountText.SetText(reward.amount.ToString());
    }

    public void OnClickBuyButton()
    {
        if (adIcon.activeSelf)
        {
            AdManager.Instance.ShowRewardedAd(
                onReward: Purchase,
                onClose: null,
                onNotReady: NotReadyAd
            );
        }
        else
        {
            Purchase();
        }
    }

    public void Purchase()
    {
        DataManager.Instance.AddItem(reward.itemType, reward.amount);
        onPurchaseCompletedEvent?.Invoke();
    }

    private void NotReadyAd()
    {
        EventManager.Instance.Trigger(new EventData<NotificationData>(
                EventID.Notification,
                new NotificationData(
                    "Ad are not available",
                    NotificationColorType.Yellow
                )
            ));
    }


    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(FreeSinglePackView))]
    public class SingleFreePackView_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            FreeSinglePackView target = (FreeSinglePackView)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.Setup)))
            {
                target.Setup();
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.EditorUtility.SetDirty(target.icon);
                UnityEditor.EditorUtility.SetDirty(target.amountText);
            }
        }
    }

#endif
}
