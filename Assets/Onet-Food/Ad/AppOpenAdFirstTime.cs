using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppOpenAdFirstTime : MyMonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject loadingCanvas;

    private void Start()
    {
        if (AdManager.IsAppOpenAdReady)
        {
            LoadingUI();
            AdManager.ShowAppOpenAd();
        }
        else
        {
            LoadingUI(false);
        }

        AppOpenAdController.OnCloseAdEvent += CloseAppOpenAdCallback;
    }

    private void OnDestroy()
    {
        AppOpenAdController.OnCloseAdEvent -= CloseAppOpenAdCallback;
    }

    private void CloseAppOpenAdCallback()
    {
        LoadingUI(false);
    }

    public void LoadingUI(bool loading = true)
    {
        uiContainer.SetActive(!loading);
        loadingCanvas.SetActive(loading);
    }
}
