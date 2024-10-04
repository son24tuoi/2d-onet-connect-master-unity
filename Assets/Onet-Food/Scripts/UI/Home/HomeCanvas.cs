using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCanvas : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private RemoveAdButton removeAdButton;
    [SerializeField] private ChestProgressView chestProgressView;
    [SerializeField] private NotifyShop notifyShop;

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

    private void OnEnable()
    {
        Setup();

        IAPManager.OnRemoveAdEvent += RemoveAd;
    }

    private void OnDisable()
    {
        IAPManager.OnRemoveAdEvent -= RemoveAd;
    }

    public void Setup()
    {
        removeAdButton.gameObject.SetActive(!DataManager.Data.iapData.RemoveAd);
        chestProgressView.Setup(DataManager.Data.chestData.ChestIndex, DataManager.Data.itemsData.StarAmount);
        notifyShop.Setup();
    }

    private void RemoveAd()
    {
        removeAdButton.gameObject.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        GameManager.Instance.uiController.ShowLevelSelectionCanvas();
    }

    public void OnClickSettingButton()
    {
        EventManager.Instance.Trigger(EventID.Setting);
    }

    public void OnClickShopButton()
    {
        EventManager.Instance.Trigger(EventID.ShopCanvas);
    }
}
