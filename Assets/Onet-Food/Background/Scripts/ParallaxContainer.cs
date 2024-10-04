using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PrimeTween;
using Cysharp.Threading.Tasks;

namespace Background
{
    public class ParallaxContainer : MonoBehaviour
    {
        [Header("Element")]
        public SpriteRenderer[] spriteRenderers;
        public Parallax[] parallaxArray;

        [Header("Setting")]
        public float hiddenValue = 0f;
        public float visibleValue = 1f;
        public float duration = 300;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = true;

        private bool m_isUpdate = false;

        private void OnDisable()
        {
            m_isUpdate = false;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            m_isUpdate = true;
            UpdateTask().Forget();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            m_isUpdate = false;
        }

        public void FadeOut()
        {
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Tween.Alpha(spriteRenderers[i],
                            visibleValue,
                            hiddenValue,
                            duration,
                            ease,
                            useUnscaledTime: ignoreTimeScale);
            }
            Tween.Delay(duration, Hide, useUnscaledTime: ignoreTimeScale);
        }

        public void FadeIn()
        {
            Show();
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Tween.Alpha(spriteRenderers[i],
                            hiddenValue,
                            visibleValue,
                            duration,
                            ease,
                            useUnscaledTime: ignoreTimeScale);
            }
        }

        public async UniTask UpdateTask()
        {
            while (m_isUpdate)
            {
                for (int i = 0; i < parallaxArray.Length; i++)
                {
                    parallaxArray[i].UpdatePosition();
                }
                await UniTask.WaitForSeconds(0.1f);
            }
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(ParallaxContainer))]
        public class ParallaxContainer_Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                ParallaxContainer target = (ParallaxContainer)base.target;

                GUILayout.Space(20f);

                if (GUILayout.Button("Get Sprite Renderers"))
                {
                    target.spriteRenderers = target.GetComponentsInChildren<SpriteRenderer>();
                    EditorUtility.SetDirty(target);
                }

                if (GUILayout.Button("Get Parallax Array"))
                {
                    target.parallaxArray = target.GetComponentsInChildren<Parallax>();
                    EditorUtility.SetDirty(target);
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Fade Out"))
                {
                    target.FadeOut();
                }

                if (GUILayout.Button("Fade In"))
                {
                    target.FadeIn();
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Show"))
                {
                    target.Show();
                }

                if (GUILayout.Button("Hide"))
                {
                    target.Hide();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Opacity 1"))
                {
                    Color color;
                    for (int i = 0; i < target.spriteRenderers.Length; i++)
                    {
                        color = target.spriteRenderers[i].color;
                        color.a = target.visibleValue;
                        target.spriteRenderers[i].color = color;
                    }
                    EditorUtility.SetDirty(this);
                }
            }
        }
#endif
    }
}
