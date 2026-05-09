using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class IAPManager : MonoBehaviour
{
    public static IAPManager Instance { get; private set; }

    public static event Action OnRemoveAdEvent;

    [Header("Element")]
    public RemoveAdCanvas removeAdCanvasPrefab;
    public FakeStoreCanvas fakeStoreCanvasPrefab;

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

    private IAPData m_iapData;

    public IAPData IAPData
    {
        get
        {
            if (ReferenceEquals(m_iapData, null))
            {
                m_iapData = DataManager.Data.iapData;
            }
            return m_iapData;
        }
    }

    private RemoveAdCanvas m_removeAdCanvas;
    private FakeStoreCanvas m_fakeStoreCanvas;

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

    private void Init()
    {

    }

    private void Start()
    {
        RemoveAdButton.OnClick += ShowRemoveAdsPopup;
    }

    private void OnDestroy()
    {
        RemoveAdButton.OnClick -= ShowRemoveAdsPopup;
    }

    public void ShowRemoveAdsPopup()
    {
        if (m_removeAdCanvas == null)
        {
            m_removeAdCanvas = Instantiate(removeAdCanvasPrefab);
        }
        else
        {
            m_removeAdCanvas.gameObject.SetActive(true);
        }
    }

    public void RemoveAd()
    {
        IAPData.RemoveAd = true;
        DataManager.SaveData();

        OnRemoveAdEvent?.Invoke();
    }

    public void GetReward(RewardProfileSO rewardProfileSO) => DataManager.SaveReward(rewardProfileSO);

    public void ShowFakeStorePopup(Action onComplete, Action onCancel)
    {
        if (m_fakeStoreCanvas == null)
        {
            m_fakeStoreCanvas = Instantiate(fakeStoreCanvasPrefab);
        }
        else
        {
            m_fakeStoreCanvas.gameObject.SetActive(true);
        }

        m_fakeStoreCanvas.Setup(onComplete, onCancel);
    }
}
