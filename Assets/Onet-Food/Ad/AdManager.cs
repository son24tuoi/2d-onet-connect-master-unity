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

    private bool _isShowNativeOverlayAd = false;
    private List<INativeOverlayAdView> nativeOverlayAdViews = new List<INativeOverlayAdView>();
    private DataManager _dataManager;

    private bool _isInitialized = false;

    private bool _isShowingAd = false;

    private IAdData _iAdData;

    private LevelData _levelData;

    private readonly WaitForSecondsRealtime _wait1s = new WaitForSecondsRealtime(1f);

    public DataManager DataManager
    {
        get
        {
            if (ReferenceEquals(_dataManager, null))
            {
                _dataManager = DataManager.Instance;
            }
            return _dataManager;
        }
    }

    public bool IsAppOpenAdReady => appOpenAdController.IsAdAvailable;

    public bool RemoveAd => DataManager.Data.iapData.RemoveAd;

    public bool IsShowNativeOverlayAd
    {
        get => _isShowNativeOverlayAd;
        set => _isShowNativeOverlayAd = value;
    }

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

            rewardedAdController.LoadAd();
            interstitialAdController.LoadAd();

            if (adProfileSO.EnableBannerAd && !RemoveAd)
            {
                bannerViewController.LoadAd();
            }
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
        _levelData = DataManager.Data.levelData;
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

    public void CheckShowInterstitialAd(Action onCloseAndNotReady)
    {
        CheckShowInterstitialAd(onCloseAndNotReady, onCloseAndNotReady);
    }

    public void CheckShowInterstitialAd(Action onClose, Action onNotReady)
    {
        if (RemoveAd ||
            !adProfileSO.EnableInterstitialAd(_levelData.LevelIndex + 1, DateTime.Now.Ticks, _iAdData.InterstitialAdTime))
        {
            onNotReady?.Invoke();
            return;
        }

        StartCoroutine(ShowInterstitialAd(onClose, onNotReady));
    }

    public IEnumerator ShowInterstitialAd(Action onClose, Action onNotReady)
    {
        if (interstitialAdController.CanShowAd())
        {
            EventManager.Instance.Trigger(new EventData<NotificationData>(
                EventID.Notification,
                new NotificationData(
                    "Ad Break",
                    NotificationColorType.White
                )
            ));

            EventManager.Instance.Trigger(new EventData<bool>(EventID.IgnoreUI, true));
            yield return _wait1s;
            EventManager.Instance.Trigger(new EventData<bool>(EventID.IgnoreUI, false));
        }

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

    public void AddNativeOverlayView(INativeOverlayAdView adView)
    {
        nativeOverlayAdViews.Add(adView);
    }

    public void RemoveNativeOverlayView(INativeOverlayAdView adView)
    {
        nativeOverlayAdViews.Remove(adView);

        if (nativeOverlayAdViews.Count == 0)
            IsShowNativeOverlayAd = false;
    }

    public void ShowNativeOverlayViews()
    {
        for (int i = 0; i < nativeOverlayAdViews.Count; i++)
        {
            nativeOverlayAdViews[i].Show();
        }
    }

    public void HideNativeOverlayViews()
    {
        for (int i = 0; i < nativeOverlayAdViews.Count; i++)
        {
            nativeOverlayAdViews[i].Hide();
        }
    }
}
