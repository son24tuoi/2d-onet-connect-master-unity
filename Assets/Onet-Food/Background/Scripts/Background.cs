using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Background
{
    public class Background : MonoBehaviour
    {
        [Header("Element")]
        public ParallaxContainer[] parallaxContainers;

        [Space(10)]
        public BackgroundProfileSO backgroundProfileSO;

        private CloudType m_cloudType;
        private CloudType m_nextCloudType;

        public CloudType CloudType => m_cloudType;

        public float[] TimeOfDay => backgroundProfileSO.timeOfDay;

        public bool CheckChangeBackground()
        {
            m_nextCloudType = GetCurrentCloudType(DateTime.Now.Hour);

            if (m_nextCloudType != m_cloudType)
            {
                parallaxContainers[(int)m_cloudType].FadeOut();
                parallaxContainers[(int)m_nextCloudType].FadeIn();

                m_cloudType = m_nextCloudType;
                return true;
            }

            return false;
        }

        public void Init()
        {
            m_cloudType = GetCurrentCloudType(DateTime.Now.Hour);

            int index = (int)m_cloudType;

            for (int i = 0; i < parallaxContainers.Length; i++)
            {
                if (i == index)
                {
                    parallaxContainers[i].Show();
                }
                else
                {
                    parallaxContainers[i].Hide();
                }
            }
        }

        public CloudType GetCurrentCloudType(float hour)
        {
            for (int i = 0; i < TimeOfDay.Length - 1; i++)
            {
                if (hour >= TimeOfDay[i] && hour < TimeOfDay[i + 1])
                {
                    return (CloudType)i;
                }
            }

            return CloudType.Evening;
        }


#if UNITY_EDITOR
        [CustomEditor(typeof(Background))]
        public class Background_Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                Background target = (Background)base.target;

                GUILayout.Space(20f);

                if (GUILayout.Button("Init"))
                {
                    target.Init();
                    EditorUtility.SetDirty(target);
                }
            }
        }
#endif
    }
}
