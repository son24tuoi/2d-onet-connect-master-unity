using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

using ItemType = ItemsData.ItemType;

public class SupportItemButton : MyMonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image icon;
    [SerializeField] private SupportItemPopup supportItemPopup;

    [Space(5)]
    [SerializeField] private GameObject plusIcon;

    [Space(5)]
    [SerializeField] private GameObject amountObject;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Config")]
    [SerializeField] private ItemType itemType;
    public ItemsProfileSO itemsProfileSO;

    public ItemType ItemType
    {
        get => itemType;
    }

    public int ItemAmount
    {
        get => DataManager.Data.itemsData.GetItem(itemType);
    }

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        SetPlusIcon(ItemAmount);
    }

    public void SetIcon(ItemType itemType)
    {
        icon.sprite = itemsProfileSO.GetIcon(itemType);
    }

    public void SetPlusIcon(int itemAmount)
    {
        bool isPlusIcon = itemAmount <= 0;

        plusIcon.SetActive(isPlusIcon);
        amountObject.SetActive(!isPlusIcon);

        if (!isPlusIcon)
        {
            amountText.SetText(itemAmount.ToString());
        }
    }

    public void OnClickButton()
    {
        if (ItemAmount >= 1)
        {
            if (supportItemPopup != null)
            {
                supportItemPopup.Show(itemType);
            }
        }
        else
        {
            ShowShop();
        }
    }

    public void ShowShop()
    {
        EventManager.Instance.Trigger(new EventData<ItemType>(
            EventID.SupportItemShop,
            itemType
        ));
    }






#if UNITY_EDITOR
    [CustomEditor(typeof(SupportItemButton))]
    public class SupportItemButton_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SupportItemButton target = (SupportItemButton)base.target;

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
            }
        }
    }
#endif
}
