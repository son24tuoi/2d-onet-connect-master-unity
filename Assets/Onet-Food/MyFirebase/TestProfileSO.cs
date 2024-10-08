using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "TestProfileSO", menuName = "Scriptable Object/Test Profile")]
public class TestProfileSO : ScriptableObject
{
    public string[] deviceIdTest;

    public bool IsDeviceTest()
    {
#if UNITY_EDITOR
        return true;
#else
        string deviceId = SystemInfo.deviceUniqueIdentifier;

        for (int i = 0; i < deviceIdTest.Length; i++)
        {
            if (deviceId == deviceIdTest[i])
                return true;
        }

        return false;
#endif
    }

    public void Remote(string configData)
    {
        TestConfigData testConfigData = JsonUtility.FromJson<TestConfigData>(configData);

        if (testConfigData == null)
            return;

        if (testConfigData.deviceIdTest != null)
        {
            deviceIdTest = testConfigData.deviceIdTest;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TestProfileSO))]
    public class BackgroundProfileSO_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TestProfileSO target = (TestProfileSO)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Json Template"))
            {
                TestConfigData testConfigData = new TestConfigData
                {
                    deviceIdTest = target.deviceIdTest
                };

                FirebaseUtilities.SaveJsonData(testConfigData, "test.json");
            }
        }
    }
#endif
}

[Serializable]
public class TestConfigData
{
    public string[] deviceIdTest;
}