using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrimeTween
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TweenFadeCanvasGroup : MonoBehaviour
    {
        private CanvasGroup m_canvasGroup;

        [Header("Setting")]
        [Range(0, 1)] public float from = 0f;
        [Range(0, 1)] public float to = 1f;
        public float duration = 1f;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = false;
        public bool playOnEnable = true;

        public CanvasGroup CanvasGroup
        {
            get
            {
                if (ReferenceEquals(m_canvasGroup, null))
                {
                    m_canvasGroup = GetComponent<CanvasGroup>();
                }
                return m_canvasGroup;
            }
        }

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Fade();
            }
        }

        [ContextMenu(nameof(Fade))]
        public void Fade()
        {
            Tween.Alpha(CanvasGroup, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void Fade(float duration)
        {
            Tween.Alpha(CanvasGroup, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void Fade(float from, float to, float duration, Ease ease = Ease.Default, Action complete = null)
        {
            Tween.Alpha(CanvasGroup, from, to, duration, ease, useUnscaledTime: ignoreTimeScale)
                .OnComplete(() =>
                {
                    complete?.Invoke();
                });
        }

        [ContextMenu(nameof(ReverseFade))]
        public void ReverseFade()
        {
            Tween.Alpha(CanvasGroup, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ReverseFade(float duration)
        {
            Tween.Alpha(CanvasGroup, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
        }
    }
}
