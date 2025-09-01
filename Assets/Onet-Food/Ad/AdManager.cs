using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance { get; private set; }


    [Space(10)]
    public GameObject test;

    [Space(10)]
    public AdProfileSO adProfileSO;

    private bool _isShowNativeOverlayAd = false;
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

    public bool IsAppOpenAdReady => true;

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
        _isInitialized = true;

        _iAdData.SaveInterstitialTime(DateTime.Now);

        IAPManager.OnRemoveAdEvent += OnRemoveAd;
    }

    private void OnDestroy()
    {
        IAPManager.OnRemoveAdEvent -= OnRemoveAd;
    }

    private void Init()
    {
        _iAdData = DataManager.Data.adData;
        _levelData = DataManager.Data.levelData;
    }

    public async UniTask ShowBannerAdTask()
    {
        if (!adProfileSO.EnableBannerAd)
            return;

        if (RemoveAd)
            return;

        await UniTask.WaitUntil(() => _isInitialized);

        ShowBannerAd();
    }

    public void ShowBannerAd()
    {
        if (!adProfileSO.EnableBannerAd)
            return;

        if (RemoveAd)
            return;

    }

    public void ShowAppOpenAd()
    {
        if (RemoveAd)
            return;

    }

    public void ShowRewardedAd(Action onReward, Action onClose, Action onNotReady)
    {
        _isShowingAd = true;
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
        if (true)
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
    }

    public void OnRemoveAd()
    {
        
    }
}
