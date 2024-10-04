using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using TransactionType = SupportItemsController.TransactionType;

public class SupportItemBuyView : MonoBehaviour
{
    public static event Action<TransactionType, ItemsData.ItemType> OnBuyEvent;

    [Header("Element")]
    [SerializeField] private Image icon;

    [Space(5)]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI priceText;

    [Header("Config")]
    [SerializeField] private ItemsData.ItemType itemType;
    public ItemsProfileSO itemsProfileSO;

    public void SetIcon(ItemsData.ItemType itemType)
    {
        icon.sprite = itemsProfileSO.GetIcon(itemType);
    }

    public void SetTitle(ItemsData.ItemType itemType)
    {
        titleText.SetText(itemType.ToString() + " Item");
    }

    public void OnClickBuyButton()
    {
        OnBuyEvent?.Invoke(TransactionType.Coin, itemType);
    }

    public void OnClickAdsButton()
    {
        OnBuyEvent?.Invoke(TransactionType.Ad, itemType);
    }



#if UNITY_EDITOR
    [CustomEditor(typeof(SupportItemBuyView))]
    public class SupportItemBuyView_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SupportItemBuyView target = (SupportItemBuyView)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Update Icon"))
            {
                target.SetIcon(target.itemType);
                target.SetTitle(target.itemType);
                EditorUtility.SetDirty(this);
            }
        }
    }
#endif
}
