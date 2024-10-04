using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField] protected TweenScale m_tweenScale;
    [SerializeField] protected TweenFadeCanvasGroup m_tweenFadeCanvasGroup;

    [Space(5)]
    public GameProfileSO gameProfileSO;

    protected virtual void OnEnable()
    {
        gameProfileSO.ShowPopup();
    }

    protected virtual void OnDisable()
    {
        gameProfileSO.HidePopup();
    }

    public virtual void Exit(Action exitEvent = null)
    {
        m_tweenScale.Scale(Vector3.one, Vector3.zero, 0.35f, Ease.InBack, () =>
        {
            exitEvent?.Invoke();
            gameObject.SetActive(false);
        });
    }

    public virtual void ExitAndRemove(Action exitEvent = null)
    {
        m_tweenScale.Scale(Vector3.one, Vector3.zero, 0.35f, Ease.InBack, () =>
        {
            exitEvent?.Invoke();
            Destroy(gameObject);
        });
    }
}
