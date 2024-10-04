using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;

public class TweenBounceUI : MonoBehaviour
{
    [Header("Setting")]
    public TweenSettings<Vector3> bounceSetting;
    public bool playOnEnable = true;

    private RectTransform _rectTransform;

    public RectTransform RectTransform
    {
        get
        {
            if (ReferenceEquals(_rectTransform, null))
            {
                _rectTransform = GetComponent<RectTransform>();
            }
            return _rectTransform;
        }
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Bounce();
        }
    }

    private void OnDisable()
    {
        Tween.StopAll(RectTransform);
    }

    public void Bounce()
    {
        Tween.UIAnchoredPosition(RectTransform, bounceSetting.startValue, bounceSetting.endValue, bounceSetting.settings);
    }
}
