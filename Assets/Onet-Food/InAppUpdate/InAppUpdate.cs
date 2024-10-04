using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google.Play.AppUpdate;
using Google.Play.Common;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class InAppUpdate : MonoBehaviour
{
    public static event Action OnErrorEvent;

    [SerializeField] private UpdateCanvas updateCanvasPrefab;

    private UpdateCanvas updateCanvas;
    private AppUpdateManager appUpdateManager;
    private AppUpdateInfo appUpdateInfoResult;

    private void Start()
    {
        UpdateButton.OnClickEvent += StartImmediateAppUpdate;
        CheckForUpdateButton.OnClickEvent += CheckForUpdate;

        Init(false);
    }

    private void OnDestroy()
    {
        UpdateButton.OnClickEvent -= StartImmediateAppUpdate;
        CheckForUpdateButton.OnClickEvent -= CheckForUpdate;
    }

    private void CheckForUpdate()
    {
        Debug.Log(nameof(CheckForUpdate));

        Init(true);
    }

    private void Init(bool showError)
    {
        if (appUpdateManager == null)
        {
            appUpdateManager = new AppUpdateManager();
        }

        StartCoroutine(IECheckForUpdate(showError));
    }

    private IEnumerator IECheckForUpdate(bool showError)
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            appUpdateInfoResult = appUpdateInfoOperation.GetResult();
            // Check AppUpdateInfo's UpdateAvailability, UpdatePriority,
            // IsUpdateTypeAllowed(), etc. and decide whether to ask the user
            // to start an in-app update.

            if (appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                Debug.Log("IN APP UPDATE: Update Available");

                // Creates an AppUpdateOptions defining an immediate in-app
                // update flow and its parameters.
                ShowUpdateCanvas();
            }
            else
            {
                Debug.Log("IN APP UPDATE: Not Update Available");

                if (showError)
                {
                    OnErrorEvent?.Invoke();
                }
            }
        }
        else
        {
            // Log appUpdateInfoOperation.Error.
            Debug.Log(appUpdateInfoOperation.Error);

            if (showError)
            {
                OnErrorEvent?.Invoke();
            }
        }
    }

    public void StartImmediateAppUpdate()
    {
        Debug.Log(nameof(StartImmediateAppUpdate));

        var appUpdateOptions = AppUpdateOptions.ImmediateAppUpdateOptions();

        StartCoroutine(IEStartImmediateAppUpdate(appUpdateInfoResult, appUpdateOptions));
    }

    private IEnumerator IEStartImmediateAppUpdate(AppUpdateInfo appUpdateInfoResult, AppUpdateOptions appUpdateOptions)
    {
        // Creates an AppUpdateRequest that can be used to monitor the
        // requested in-app update flow.
        var startUpdateRequest = appUpdateManager.StartUpdate(
            // The result returned by PlayAsyncOperation.GetResult().
            appUpdateInfoResult,
            // The AppUpdateOptions created defining the requested in-app update
            // and its parameters.
            appUpdateOptions);
        yield return startUpdateRequest;

        // If the update completes successfully, then the app restarts and this line
        // is never reached. If this line is reached, then handle the failure (for
        // example, by logging result.Error or by displaying a message to the user).
    }

    public void ShowUpdateCanvas()
    {
        if (updateCanvas == null)
        {
            updateCanvas = Instantiate(updateCanvasPrefab, transform);
        }

        updateCanvas.gameObject.SetActive(true);
    }















#if UNITY_EDITOR

    [CustomEditor(typeof(InAppUpdate))]
    public class InApppUpdate_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            InAppUpdate target = (InAppUpdate)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.ShowUpdateCanvas)))
            {
                target.ShowUpdateCanvas();
            }
        }
    }

#endif
}
