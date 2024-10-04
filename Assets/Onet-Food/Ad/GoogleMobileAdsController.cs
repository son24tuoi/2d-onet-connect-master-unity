using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;
using UnityEngine;

public class GoogleMobileAdsController : MonoBehaviour
{
    // Always use test ads.
    // https://developers.google.com/admob/unity/test-ads
    internal static List<string> TestDeviceIds = new List<string>()
    {
        AdRequest.TestDeviceSimulator,
#if UNITY_IPHONE
        "96e23e80653bb28980d3f40beb58915c",
#elif UNITY_ANDROID
        "702815ACFC14FF222DA1DC767672A573"
#endif
    };

    // The Google Mobile Ads Unity plugin needs to be run only once.
    private static bool? _isInitialized;

    // Helper class that implements consent using the
    // Google User Messaging Platform (UMP) Unity plugin.
    [SerializeField, Tooltip("Controller for the Google User Messaging Platform (UMP) Unity plugin.")]
    private GoogleMobileAdsConsentController _consentController;

    public void Initialization(Action onInitializationSuccessful)
    {
        // On Android, Unity is paused when displaying interstitial or rewarded video.
        // This setting makes iOS behave consistently with Android.
        MobileAds.SetiOSAppPauseOnBackground(true);

        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        // https://developers.google.com/admob/unity/quick-start#raise_ad_events_on_the_unity_main_thread
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

        // Configure your RequestConfiguration with Child Directed Treatment
        // ad content rating
        // tag for child 
        // and the Test Device Ids.
        RequestConfiguration requestConfiguration = new RequestConfiguration
        {
            MaxAdContentRating = MaxAdContentRating.G,
            TagForChildDirectedTreatment = TagForChildDirectedTreatment.True,
            TestDeviceIds = TestDeviceIds
        };
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // If we can request ads, we should initialize the Google Mobile Ads Unity plugin.
        if (_consentController.CanRequestAds)
        {
            InitializeGoogleMobileAds(onInitializationSuccessful);
        }

        // Ensures that privacy and consent information is up to date.
        InitializeGoogleMobileAdsConsent(onInitializationSuccessful);
    }

    /// <summary>
    /// Initializes the Google Mobile Ads Unity plugin.
    /// </summary>
    private void InitializeGoogleMobileAds(Action onInitializationSuccessful)
    {
        // The Google Mobile Ads Unity plugin needs to be run only once and before loading any ads.
        if (_isInitialized.HasValue)
        {
            return;
        }

        _isInitialized = false;

        // Initialize the Google Mobile Ads Unity plugin.
        Debug.Log("Google Mobile Ads Initializing.");
        MobileAds.Initialize((InitializationStatus initstatus) =>
        {
            if (initstatus == null)
            {
                Debug.LogError("Google Mobile Ads initialization failed.");
                _isInitialized = null;
                return;
            }

            // If you use mediation, you can check the status of each adapter.
            var adapterStatusMap = initstatus.getAdapterStatusMap();
            if (adapterStatusMap != null)
            {
                foreach (var item in adapterStatusMap)
                {
                    Debug.Log(string.Format("Adapter {0} is {1}",
                        item.Key,
                        item.Value.InitializationState));
                }
            }

            Debug.Log("Google Mobile Ads initialization complete.");
            _isInitialized = true;

            onInitializationSuccessful?.Invoke();
        });
    }

    /// <summary>
    /// Ensures that privacy and consent information is up to date.
    /// </summary>
    private void InitializeGoogleMobileAdsConsent(Action onInitializationSuccessful)
    {
        Debug.Log("Google Mobile Ads gathering consent.");

        _consentController.GatherConsent((string error) =>
        {
            if (error != null)
            {
                Debug.LogError("Failed to gather consent with error: " +
                    error);
            }
            else
            {
                Debug.Log("Google Mobile Ads consent updated: "
                    + ConsentInformation.ConsentStatus);
            }

            if (_consentController.CanRequestAds)
            {
                InitializeGoogleMobileAds(onInitializationSuccessful);
            }
        });
    }

}
