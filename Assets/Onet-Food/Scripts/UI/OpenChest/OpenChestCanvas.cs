using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class OpenChestCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private OpenChestView openChestView;

    protected override void OnEnable()
    {
        base.OnEnable();

        Tween.Delay(m_tweenScale.duration, () =>
        {
            openChestView.OpenChestTask().Forget();
        }, useUnscaledTime: true);
    }

    public void Init(RewardProfileSO rewardProfileSO)
    {
        openChestView.Init();
        openChestView.rewardProfileSO = rewardProfileSO;
    }

    public void OnClose()
    {
        Exit();
    }

    public override void Exit(Action exitEvent = null)
    {
        m_tweenFadeCanvasGroup.Fade(1f, 0f, 0.2f, Ease.OutQuad, complete: () =>
        {
            exitEvent?.Invoke();
            gameObject.SetActive(false);
        });
    }
}
