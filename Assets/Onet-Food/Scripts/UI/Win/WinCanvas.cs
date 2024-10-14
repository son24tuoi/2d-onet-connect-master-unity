using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using System;

public class WinCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private SummaryView summaryView;
    [SerializeField] private WinInfoView winInfoView;

    [Space(5)]
    [SerializeField] private ChestProgressView chestProgressView;

    protected DataManager m_dataManager;

    public DataManager DataManager
    {
        get
        {
            if (ReferenceEquals(m_dataManager, null))
            {
                m_dataManager = DataManager.Instance;
            }
            return m_dataManager;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        summaryView.Init(gameProfileSO.currentLevelIndex);
        winInfoView.Init();

        SetChest();
    }

    private void SetChest()
    {
        int starItem = DataManager.Data.itemsData.StarAmount;
        int preStarItem = starItem - gameProfileSO.starsReceived;

        chestProgressView.Setup(DataManager.Data.chestData.ChestIndex, preStarItem);

        chestProgressView.RunEffect(preStarItem, starItem, 1f, 0.5f);
    }

    public void OnClickNextButton()
    {
        Exit(() =>
        {
            CheckShowInterstitialAd(() =>
            {
                GameManager.Instance.NextLevel();
            });
        });
    }

    public void OnClickHomeButton()
    {
        CheckShowInterstitialAd(() =>
        {
            GameManager.Instance.BackToHome();
            Exit();
        });
    }

    public void OnClickReplayButton()
    {
        CheckShowInterstitialAd(() =>
        {
            Exit(() =>
            {
                GameManager.Instance.PlayLevel(gameProfileSO.currentLevelIndex);
            });
        });
    }

    private void CheckShowInterstitialAd(Action onCloseAndNotReady)
    {
        AdManager.Instance.CheckShowInterstitialAd(onCloseAndNotReady, onCloseAndNotReady);
    }
}
