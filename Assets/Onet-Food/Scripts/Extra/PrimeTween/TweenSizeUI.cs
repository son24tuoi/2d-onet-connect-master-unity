using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTween
{
    public class TweenSizeUI : MonoBehaviour
    {
        [Header("Setting")]
        public Vector2 from = Vector2.zero;
        public Vector2 to = Vector2.one;
        public float duration = 1f;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = false;
        public bool playOnEnable = true;

        private RectTransform _rectTranform;

        public RectTransform RectTransform
        {
            get
            {
                if (ReferenceEquals(_rectTranform, null))
                {
                    _rectTranform = GetComponent<RectTransform>();
                }
                return _rectTranform;
            }
        }

        private void OnEnable()
        {
            if (playOnEnable)
            {
                ChangeSize();
            }
        }

        public void ChangeSize()
        {
            Tween.UISizeDelta(RectTransform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ChangeSize(Action onUpdate)
        {
            Tween.UISizeDelta(RectTransform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale)
                .OnUpdate(this, (target, tween) =>
                {
                    onUpdate?.Invoke();
                });
        }

        public void ReverseChangeSize()
        {
            Tween.UISizeDelta(RectTransform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ChangeSizeImmediate()
        {
            RectTransform.sizeDelta = to;
        }
    }
}
