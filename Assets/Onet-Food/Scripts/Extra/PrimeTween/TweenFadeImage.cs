using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PrimeTween
{
    [RequireComponent(typeof(Image))]
    public class TweenFadeImage : MonoBehaviour
    {
        private Image m_Image;

        [Header("Setting")]
        [Range(0, 1)] public float from = 0f;
        [Range(0, 1)] public float to = 1f;
        public float duration = 1f;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = false;
        public bool playOnEnable = true;

        public Image Image
        {
            get
            {
                if (ReferenceEquals(m_Image, null))
                {
                    m_Image = GetComponent<Image>();
                }
                return m_Image;
            }
        }

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Fade();
            }
        }

        public void Fade()
        {
            Tween.Alpha(Image, from, to, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ReverseFade()
        {
            Tween.Alpha(Image, to, from, duration, ease, useUnscaledTime: ignoreTimeScale);
        }
    }

















#if UNITY_EDITOR

    [CustomEditor(typeof(TweenFadeImage))]
    public class TweenFadeImage_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TweenFadeImage target = (TweenFadeImage)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.Fade)))
            {
                target.Fade();
            }

            if (GUILayout.Button(nameof(target.ReverseFade)))
            {
                target.ReverseFade();
            }
        }
    }

#endif
}
