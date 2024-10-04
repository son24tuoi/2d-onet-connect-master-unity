using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class TweenMove : MonoBehaviour
{
    [Header("Setting")]
    public Vector3 from = Vector3.zero;
    public Vector3 to = Vector3.one;
    public float duration = 1f;
    public Ease ease = Ease.Default;
    public bool ignoreTimeScale = false;
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
            Move();
        }
    }

    private void OnDisable()
    {
        Tween.StopAll(transform);
    }

    public void Move()
    {
        Tween.Position(transform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
    }

    public void ReverseMove()
    {
        Tween.Position(transform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
    }

    public void LocalMove()
    {
        Tween.LocalPosition(transform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
    }

    public void ReverseLocalMove()
    {
        Tween.LocalPosition(transform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
    }

    public void LocalMoveUI()
    {
        Tween.UIAnchoredPosition(RectTransform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
    }

    public void ReverseLocalMoveUI()
    {
        Tween.UIAnchoredPosition(RectTransform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
    }
}
