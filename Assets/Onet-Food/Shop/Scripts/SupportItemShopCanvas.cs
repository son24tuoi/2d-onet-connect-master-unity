using System;
using System.Collections;
using System.Collections.Generic;
using Background;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class SupportItemShopCanvas : Popup
{
    [Header("Elements")]
    [SerializeField] private SinglePackView[] singlePackViewArray;

    [Header("Datas")]
    public ItemsData.ItemType itemType;
    public PackInShopProfileSO[] hintPacks;
    public PackInShopProfileSO[] shufflePacks;
    public PackInShopProfileSO[] timerPacks;

    public void Init(ItemsData.ItemType itemType)
    {
        this.itemType = itemType;

        Init();
    }

    public void Init()
    {
        switch (itemType)
        {
            case ItemsData.ItemType.Hint:
                SetupAllSinglePackView(hintPacks);
                break;

            case ItemsData.ItemType.Shuffle:
                SetupAllSinglePackView(shufflePacks);
                break;

            case ItemsData.ItemType.Timer:
                SetupAllSinglePackView(timerPacks);
                break;

            default:
                Debug.Log("Ignore " + itemType);
                break;
        }
    }

    public void SetupAllSinglePackView(PackInShopProfileSO[] packs)
    {
        int length = singlePackViewArray.Length;
        for (int i = 0; i < length; i++)
        {
            singlePackViewArray[i].Setup(packs[i]);
        }
    }

    public void OnClickExitButton()
    {
        Exit();
    }

    public override void Exit(Action exitEvent = null)
    {
        m_tweenFadeCanvasGroup.Fade(1f, 0f, 0.2f, Ease.OutQuad, complete: () =>
        {
            gameObject.SetActive(false);
        });
    }
}
