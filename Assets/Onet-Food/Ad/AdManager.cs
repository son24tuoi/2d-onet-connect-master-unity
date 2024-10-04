using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;
using Cysharp.Threading.Tasks;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }

    [Header("Element")]
    [SerializeField] private GoogleMobileAdsController googleMobileAdsController;

    [Space(5)]
    [SerializeField] private AppOpenAdController appOpenAdController;
    [SerializeField] private BannerViewController bannerViewController;
    [SerializeField] private RewardedAdController rewardedAdController;
    [SerializeField] private InterstitialAdController interstitialAdController;

    [Space(10)]
    public GameObject test;

    [Space(10)]
    public AdProfileSO adProfileSO;

    private DataManager m_dataManager;

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

    public bool IsAppOpenAdReady => appOpenAdController.IsAdAvailable;

    public bool RemoveAd => DataManager.Data.iapData.RemoveAd;

    private bool _isInitialized = false;

    private bool _isShowingAd = false;

    private IAdData _iAdData;

    private ILevelData _iLevelData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        googleMobileAdsController.Initialization(() =>
        {
            _isInitialized = true;

            appOpenAdController.LoadAd();

            if (adProfileSO.EnableBannerAd && !RemoveAd)
            {
                bannerViewController.LoadAd();
            }

            rewardedAdController.LoadAd();
            interstitialAdController.LoadAd();
        });

        _iAdData.SaveInterstitialTime(DateTime.Now);

        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

        IAPManager.OnRemoveAdEvent += OnRemoveAd;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;

        IAPManager.OnRemoveAdEvent -= OnRemoveAd;
    }

    private void Init()
    {
        _iAdData = DataManager.Data.adData;
        _iLevelData = DataManager.Data.levelData;
    }

    private void OnAppStateChanged(AppState state)
    {
        Debug.Log("App State changed to : " + state);

        // If the app is Foregrounded and the ad is available, show it.
        if (state == AppState.Foreground && !_isShowingAd)
        {
            ShowAppOpenAd();
        }
    }

    public async UniTask ShowBannerAdTask()
    {
        if (!adProfileSO.EnableBannerAd)
            return;

        if (RemoveAd)
            return;

        await UniTask.WaitUntil(() => _isInitialized);

        await UniTask.WaitUntil(() => bannerViewController.adLoaded);

        ShowBannerAd();
    }

    public void ShowBannerAd()
    {
        if (!adProfileSO.EnableBannerAd)
            return;

        if (RemoveAd)
            return;

        bannerViewController.ShowAd();
    }

    public void ShowAppOpenAd()
    {
        if (RemoveAd)
            return;

        appOpenAdController.ShowAd();
    }

    public void ShowRewardedAd(Action onReward, Action onClose, Action onNotReady)
    {
        _isShowingAd = true;
        rewardedAdController.ShowAd(onReward: () =>
        {
            _isShowingAd = false;
            onReward?.Invoke();
        }, onClose: () =>
        {
            _isShowingAd = false;
            onClose?.Invoke();
        }, onNotReady: () =>
        {
            _isShowingAd = false;
            onNotReady?.Invoke();
        });
    }

    public void CheckShowInterstitialAd(Action onClose, Action onNotReady)
    {
        if (RemoveAd ||
            !adProfileSO.EnableInterstitialAd(_iLevelData.LevelIndex + 1, DateTime.Now.Ticks, _iAdData.InterstitialAdTime))
        {
            onNotReady?.Invoke();
            return;
        }

        ShowInterstitialAd(onClose, onNotReady);
    }

    public void ShowInterstitialAd(Action onClose, Action onNotReady)
    {
        _isShowingAd = true;
        interstitialAdController.ShowAd(onClose: () =>
        {
            _isShowingAd = false;
            _iAdData.SaveInterstitialTime(DateTime.Now);
            onClose?.Invoke();
        }, onNotReady: () =>
        {
            _isShowingAd = false;
            onNotReady?.Invoke();
        });
    }

    public void OnRemoveAd()
    {
        bannerViewController.HideAd();
    }
}
