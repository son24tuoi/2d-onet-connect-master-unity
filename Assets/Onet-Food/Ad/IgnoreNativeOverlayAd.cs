using UnityEngine;

public class IgnoreNativeOverlayAd : MonoBehaviour
{
    private bool _isShow = false;
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

    private void OnEnable()
    {
        _isShow = AdManager.IsShowNativeOverlayAd;
        if (_isShow)
        {
            AdManager.HideNativeOverlayViews();
        }
    }

    private void OnDisable()
    {
        if (_isShow)
        {
            AdManager.ShowNativeOverlayViews();
        }
    }
}