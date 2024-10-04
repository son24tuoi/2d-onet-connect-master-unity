using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SinglePackView : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI priceText;

    [SerializeField] private UnityEvent onPurchaseCompletedEvent;

    [Header("Datas")]
    public ItemsProfileSO itemsProfileSO;
    public PackInShopProfileSO packInShopProfileSO;

    private Reward reward;

    public void Setup(PackInShopProfileSO packInShopProfileSO)
    {
        this.packInShopProfileSO = packInShopProfileSO;
        Setup();
    }

    public void Setup()
    {
        reward = packInShopProfileSO.RewardProfileSO.GetReward(0);

        if (reward == null)
            return;

        icon.sprite = itemsProfileSO.GetIcon(reward.itemType);
        amountText.SetText(reward.amount.ToString());
        priceText.SetText(packInShopProfileSO.Price.ToString());
    }

    public void OnClickBuyButton()
    {
        if (DataManager.Instance.CanBuy(packInShopProfileSO.Price))
        {
            DataManager.Instance.AddItem(reward.itemType, reward.amount);
            DataManager.Instance.SpendCoin(packInShopProfileSO.Price);

            onPurchaseCompletedEvent?.Invoke();
        }
        else
        {
            EventManager.Instance.Trigger(EventID.ShopCanvas);
        }
    }


    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(SinglePackView))]
    public class SinglePackView_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SinglePackView target = (SinglePackView)base.target;

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
                UnityEditor.EditorUtility.SetDirty(target.priceText);
            }
        }
    }

#endif

    // -------------------------------------------------------------------------------------------------
}
