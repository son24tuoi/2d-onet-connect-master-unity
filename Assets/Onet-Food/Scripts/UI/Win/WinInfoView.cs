using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;

public class WinInfoView : MyMonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI scoreValueTMP;
    [SerializeField] private TextMeshProUGUI timeValueTMP;
    [SerializeField] private GameObject highScore;
    [SerializeField] private RewardContainerView rewardContainerView;
    public TweenFadeCanvasGroup tweenFadeCanvasGroup;

    [Space(5)]
    public GameProfileSO gameProfileSO;
    public RewardProfileSO rewardProfileSO;

    public void Init()
    {
        timeValueTMP.SetText(gameProfileSO.GetElapsedSeconds());
        highScore.SetActive(false);
        IncreaseScoreValueTask(0, 0.9f).Forget();

        rewardContainerView.Setup(rewardProfileSO);
    }

    public async UniTask IncreaseScoreValueTask(int startValue, float duration)
    {
        SetScoreValue(startValue);

        await Tween.Delay(tweenFadeCanvasGroup.duration, useUnscaledTime: true);

        await Tween.Custom(startValue, gameProfileSO.starsReceived, duration,
            onValueChange: value => SetScoreValue((int)value),
            useUnscaledTime: true);

        if (gameProfileSO.isNewHighScore)
        {
            highScore.transform.localScale = Vector3.one * 3f;
            highScore.SetActive(true);
            await Tween.Scale(highScore.transform, 1f, duration: 0.1f, useUnscaledTime: true);
        }
    }

    public void SetScoreValue(int value)
    {
        scoreValueTMP.SetText(value.ToString());
    }
}
