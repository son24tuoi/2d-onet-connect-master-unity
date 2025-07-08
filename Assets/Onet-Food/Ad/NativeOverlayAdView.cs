using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NativeOverlayAdController))]
public class NativeOverlayAdView : MonoBehaviour, INativeOverlayAdView
{
    [SerializeField] private NativeOverlayAdController nativeOverlayAdController;
    [SerializeField] private RectTransform adPlacementTarget;
    [SerializeField] private Canvas canvas;

    private readonly WaitForSecondsRealtime wait = new WaitForSecondsRealtime(1f);
    private AdManager _adManager;

    public AdManager AdManager
    {
        get
        {
            if (ReferenceEquals(_adManager, null))
            {
                _adManager = AdManager.Instance;
            }
            return _adManager;
        }
    }

    private void Reset()
    {
        nativeOverlayAdController = GetComponent<NativeOverlayAdController>();
        adPlacementTarget = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    private void OnEnable()
    {
        Show();
        AdManager.AddNativeOverlayView(this);
    }

    private void OnDisable()
    {
        Hide();
        AdManager.RemoveNativeOverlayView(this);
    }

    private IEnumerator IEWaitRenderAd()
    {
        while (!nativeOverlayAdController.IsRendered)
        {
            nativeOverlayAdController.RenderAdPlacmentTarget(adPlacementTarget, canvas);
            yield return wait;
        }

        AdManager.IsShowNativeOverlayAd = true;
    }

    public void Show()
    {
        nativeOverlayAdController.LoadAd();
        StartCoroutine(IEWaitRenderAd());
    }

    public void Hide()
    {
        nativeOverlayAdController.DestroyAd();
    }
}