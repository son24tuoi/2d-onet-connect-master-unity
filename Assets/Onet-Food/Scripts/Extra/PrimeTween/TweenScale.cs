using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTween
{
    public class TweenScale : MonoBehaviour
    {
        [Header("Setting")]
        public Vector3 from = Vector3.zero;
        public Vector3 to = Vector3.one;
        public float duration = 1f;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = false;
        public bool playOnEnable = true;

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Scale();
            }
        }

        public void Scale()
        {
            Tween.Scale(transform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void Scale(Action onComplete)
        {
            Tween.Scale(transform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }

        public void Scale(Vector3 from, Vector3 to, float duration, Ease ease, Action complete = null)
        {
            Tween.Scale(transform, from, to, duration, ease, useUnscaledTime: ignoreTimeScale)
                .OnComplete(() => complete?.Invoke());
        }

        public void ReverseScale()
        {
            Tween.Scale(transform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ReverseScale(Action onComplete)
        {
            Tween.Scale(transform, to, from, duration, ease, useUnscaledTime: ignoreTimeScale)
                .OnComplete(() => onComplete?.Invoke());
        }
    }
}
