using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class FakeStoreCanvas : MonoBehaviour
{
    private Action onCompleteEvent;
    private Action onCancelEvent;

    public void Setup(Action onComplete, Action onCancel)
    {
        onCompleteEvent = onComplete;
        onCancelEvent = onCancel;
    }

    public void OnClick_CompleteButton()
    {
        onCompleteEvent?.Invoke();

        Exit();
    }

    public void OnClick_CancelButton()
    {
        onCancelEvent?.Invoke();

        Exit();
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }
}
