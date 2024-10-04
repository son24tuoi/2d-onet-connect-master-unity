using System;
using System.Collections;
using System.Collections.Generic;
using Background;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class ShopCanvas : Popup
{
    [Header("Elements")]
    public ScrollToItem scrollToItem;
    [SerializeField] private RectTransform content;
    [SerializeField] private TweenSizeUI noAdsContainer;
    [SerializeField] private FreeSinglePackView freeCoinSinglePackView;
    [SerializeField] private VisibleUIElement visibleUIElement;
    [SerializeField] private GameObject freeLabelObject;

    private bool isFreeCoinReceived;

    protected override void OnEnable()
    {
        base.OnEnable();

        IAPManager.OnRemoveAdEvent += RemoveAdCallback;

        Init();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        IAPManager.OnRemoveAdEvent -= RemoveAdCallback;
    }

    public void Init()
    {
        if (DataManager.Instance.Data.iapData.RemoveAd)
        {
            noAdsContainer.ChangeSizeImmediate();
        }

        SetupFreeCoinTypeSinglePack();
        freeCoinSinglePackView.Setup();

        CheckShowFreeLabel();
    }

    private void RemoveAdCallback()
    {
        noAdsContainer.ChangeSize(() =>
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        });
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

    public void SetupFreeCoinTypeSinglePack()
    {
        isFreeCoinReceived = DataManager.Instance.Data.shopData.IsFreeCoinReceived;

        freeCoinSinglePackView.SetNotify(!isFreeCoinReceived);
        freeCoinSinglePackView.SetType(showAd: isFreeCoinReceived);
    }

    public void FreeCoinPurchaseCompleted()
    {
        if (DataManager.Instance.Data.shopData.IsFreeCoinReceived)
            return;

        DataManager.Instance.ReceiveFreeCoin();

        SetupFreeCoinTypeSinglePack();
    }

    public void ScrollRectValueChanged(Vector2 value)
    {
        CheckShowFreeLabel();
    }

    public void CheckShowFreeLabel()
    {
        freeLabelObject.SetActive(!isFreeCoinReceived && visibleUIElement.IsObjectHidden());
    }
}
