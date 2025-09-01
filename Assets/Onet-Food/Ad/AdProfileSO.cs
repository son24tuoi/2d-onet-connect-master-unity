using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AdProfileSO", menuName = "Scriptable Object/Ad Profile")]
public class AdProfileSO : ScriptableObject
{
    [SerializeField] private bool enableBannerAd = false;
    [SerializeField] private int interstitialAdCooldownBySecond = 0;
    [SerializeField] private int levelStartShowInterstitialAd = 10;

    public bool EnableBannerAd => enableBannerAd;

    public bool EnableInterstitialAd(int level, long nowTimeByTicks, long previousTimeByTicks)
    {
        if (level < levelStartShowInterstitialAd)
            return false;

        if (interstitialAdCooldownBySecond < 0)
            return false;

        return new TimeSpan(nowTimeByTicks - previousTimeByTicks).TotalSeconds > interstitialAdCooldownBySecond;
    }

    public void Remote(string configData)
    {
        AdConfigData adConfigData = JsonUtility.FromJson<AdConfigData>(configData);

        if (adConfigData == null)
            return;

        if (configData.Contains(nameof(adConfigData.enableBannerAd)))
        {
            enableBannerAd = adConfigData.enableBannerAd;
        }

        if (configData.Contains(nameof(adConfigData.interstitialAdCooldownBySecond)))
        {
            interstitialAdCooldownBySecond = adConfigData.interstitialAdCooldownBySecond;
        }

        if (configData.Contains(nameof(adConfigData.levelStartShowInterstitialAd)))
        {
            levelStartShowInterstitialAd = adConfigData.levelStartShowInterstitialAd;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AdProfileSO))]
    public class AdProfileSO_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AdProfileSO target = (AdProfileSO)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Json Template"))
            {
                AdConfigData adConfigData = new AdConfigData
                {
                    enableBannerAd = target.enableBannerAd,
                    interstitialAdCooldownBySecond = target.interstitialAdCooldownBySecond,
                    levelStartShowInterstitialAd = target.levelStartShowInterstitialAd
                };
            }
        }
    }
#endif
}

[Serializable]
public class AdConfigData
{
    public bool enableBannerAd;
    public int interstitialAdCooldownBySecond;
    public int levelStartShowInterstitialAd;
}
