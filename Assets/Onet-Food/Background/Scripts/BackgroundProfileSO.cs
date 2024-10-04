using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Background
{
    [CreateAssetMenu(fileName = "BackgroundProfileSO", menuName = "Scriptable Object/Background Profile")]
    public class BackgroundProfileSO : ScriptableObject
    {
        public float[] timeOfDay = {
            4.5f, 5.5f, 6f, 11f, 15.5f, 16.5f, 17.5f, 18.5f
        };

        public Color[] backgroundColors;

        public Color GetBackgroundColor(CloudType cloudType)
        {
            return GetBackgroundColor((int)cloudType);
        }

        public Color GetBackgroundColor(int index)
        {
            index = Mathf.Clamp(index, 0, backgroundColors.Length - 1);

            return backgroundColors[index];
        }

        public void Remote(string configData)
        {
            BackgroundRemote backgroundRemote = JsonUtility.FromJson<BackgroundRemote>(configData);

            if (backgroundRemote == null)
                return;

            if (backgroundRemote.timeOfDay != null)
            {
                timeOfDay = backgroundRemote.timeOfDay;
            }
        }



#if UNITY_EDITOR
        [CustomEditor(typeof(BackgroundProfileSO))]
        public class BackgroundProfileSO_Inspector : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                BackgroundProfileSO target = (BackgroundProfileSO)base.target;

                GUILayout.Space(20f);

                GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 15,
                    fontStyle = FontStyle.Bold
                };

                GUILayout.Label("Quick Access", labelStyle);

                if (GUILayout.Button("Json Template"))
                {
                    BackgroundRemote backgroundRemote = new BackgroundRemote
                    {
                        timeOfDay = target.timeOfDay
                    };

                    FirebaseUtilities.SaveJsonData(backgroundRemote, "background.json");
                }
            }
        }
#endif
    }

    [Serializable]
    public class BackgroundRemote
    {
        public float[] timeOfDay;
    }
}
