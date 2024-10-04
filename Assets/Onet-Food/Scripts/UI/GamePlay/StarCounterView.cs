using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public class StarCounterView : MonoBehaviour, IEventHandler
{
    [Header("Element")]
    [SerializeField] private Transform starDestination;
    [SerializeField] private TextMeshProUGUI amountTMP;
    [SerializeField] private GameProfileSO gameProfileSO;

    private void OnEnable()
    {
        EventManager.Instance.Trigger(new EventData<Transform>(
            EventID.StarDestination,
            starDestination
        ));

        EventManager.Instance.Subcribe(EventID.StarReceived, this);

        UpdateAmount();
    }

    private void OnDisable()
    {
        EventManager.Instance.Unsubcribe(EventID.StarReceived, this);
    }

    private void UpdateAmount()
    {
        amountTMP.SetText(gameProfileSO.StarsReceived.ToString());
        Tween.PunchScale(starDestination, Vector3.one * 1.05f, 0.03f, useUnscaledTime: true);
    }

    public void EventHandler(EventID eventID)
    {
        switch (eventID)
        {
            case EventID.StarReceived:
                UpdateAmount();
                break;

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }
}
