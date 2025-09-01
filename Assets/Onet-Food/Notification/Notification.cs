using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using PrimeTween;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Notification : MonoBehaviour, IEventHandlerWithData
{
    [Header("Element")]
    public GameObject view;
    public TweenFadeCanvasGroup tweenFadeCanvasGroup;
    public GameObject[] messageObjects;
    public TMP_Text[] messageTexts;

    private WaitForSecondsRealtime m_wait = new WaitForSecondsRealtime(1f);

    private Coroutine m_showRoutine;

    private void Start()
    {
        EventManager.Instance.Subcribe(EventID.Notification, this);
        EventManager.Instance.Subcribe(EventID.Wait, this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubcribe(EventID.Notification, this);
        EventManager.Instance.Unsubcribe(EventID.Wait, this);
    }

    private void InAppUpdateNotAvailable()
    {
        Show(
            "Not Update Available",
            NotificationColorType.Yellow
        );
    }

    public void Show(NotificationData notificationData)
    {
        Show(notificationData.message, notificationData.colorType);
    }

    public void Show(string message, NotificationColorType colorType)
    {
        ShowMessage(message, colorType);

        if (m_showRoutine != null)
        {
            StopCoroutine(m_showRoutine);
        }

        m_showRoutine = StartCoroutine(IEShow());
    }

    public void ShowMessage(string message, NotificationColorType colorType)
    {
        // Debug.Log(message + "_" + colorType.ToString());
        int index = (int)colorType;
        bool show = false;

        for (int i = 0; i < messageObjects.Length; i++)
        {
            show = i == index;
            messageObjects[i].SetActive(show);
            if (show)
            {
                messageTexts[i].SetText(message);
            }
        }
    }

    public IEnumerator IEShow()
    {
        // Debug.Log("On");
        view.SetActive(true);

        yield return m_wait;

        tweenFadeCanvasGroup.Fade(1f, 0f, 0.5f,
            complete: () =>
            {
                view.SetActive(false);
            });
    }

    public void ShowWait(bool show)
    {
        if (show)
        {
            ShowMessage("Loading", NotificationColorType.White);
            view.SetActive(true);
        }
        else
        {
            view.SetActive(false);
            // Debug.Log("off");
        }
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.Notification:
                if (eventData.data is NotificationData notificationData)
                {
                    Show(notificationData);
                }
                break;

            case EventID.Wait:
                if (eventData.data is bool show)
                {
                    ShowWait(show);
                }
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(Notification))]
    public class Notification_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Notification target = (Notification)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);


            if (GUILayout.Button("Show Test"))
            {
                target.Show("Test", NotificationColorType.Green);
            }
        }
    }
#endif
}
