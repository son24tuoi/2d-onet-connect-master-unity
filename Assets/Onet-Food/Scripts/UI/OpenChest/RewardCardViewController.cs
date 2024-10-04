using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class RewardCardViewController : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject rewardCardContainer;
    [SerializeField] private RewardCardView[] rewardCardViews;

    [Space(5)]
    [SerializeField] private GameObject rewardCardTempContainer;
    [SerializeField] private GameObject[] rewardCardViewTemps;

    [Space(5)]
    [SerializeField] private Transform startRewardCardContainer;
    [SerializeField] private Transform targetRewardCardContainer;

    public void Init()
    {
        rewardCardContainer.SetActive(false);
        rewardCardTempContainer.SetActive(false);

        rewardCardContainer.transform.localPosition = startRewardCardContainer.localPosition;
        rewardCardTempContainer.transform.localPosition = startRewardCardContainer.localPosition;
    }

    public void SetupRewards(Reward[] rewards)
    {
        for (int i = 0; i < rewardCardViews.Length; i++)
        {
            rewardCardViews[i].Init();

            if (rewards.Length > i)
            {
                rewardCardViews[i].tweenShakeRotation.Shake();
                rewardCardViews[i].SetupReward(rewards[i]);
            }
        }
    }

    public async UniTask FlyRewardCardViewTempsTask(Vector3 startPos, float moveTime = 0.25f)
    {
        ShowRewardCardViewTemps(false);
        rewardCardTempContainer.SetActive(true);

        for (int i = 0; i < rewardCardViewTemps.Length; i++)
        {
            rewardCardViewTemps[i].transform.position = startPos;
            rewardCardViewTemps[i].transform.localScale = Vector3.zero;
            rewardCardViewTemps[i].SetActive(true);

            _ = Sequence.Create(useUnscaledTime: true)
                        .Group(Tween.Scale(rewardCardViewTemps[i].transform, 1f, moveTime))
                        .Group(Tween.Position(rewardCardViewTemps[i].transform, rewardCardViews[i].transform.position, moveTime, Ease.OutQuad));

            await Tween.Delay(0.05f, useUnscaledTime: true);
        }

        rewardCardTempContainer.SetActive(false);
        rewardCardContainer.SetActive(true);
    }

    public void ShowRewardCardViewTemps(bool show = true)
    {
        for (int i = 0; i < rewardCardViewTemps.Length; i++)
        {
            rewardCardViewTemps[i].SetActive(show);
        }
    }

    public void ShowPreview()
    {
        for (int i = 0; i < rewardCardViews.Length; i++)
        {
            if (!rewardCardViews[i].isShow)
            {
                rewardCardViews[i].tweenShakeRotation.Stop();
                rewardCardViews[i].ShowPreview();
            }
        }
    }

    public void FaceDown()
    {
        for (int i = 0; i < rewardCardViews.Length; i++)
        {
            if (!rewardCardViews[i].isShow)
            {
                rewardCardViews[i].tweenShakeRotation.Shake();
                rewardCardViews[i].TurnOffTask().Forget();
            }
        }
    }

    public async UniTask MoveToTargetPositionTask(float duration = 0.3f)
    {
        await Tween.Position(rewardCardContainer.transform,
            startValue: startRewardCardContainer.position,
            endValue: targetRewardCardContainer.position,
            duration,
            useUnscaledTime: true);
    }

    public async UniTask MoveToStartPositionTask(float duration = 0.3f)
    {
        await Tween.Position(rewardCardContainer.transform,
            startValue: targetRewardCardContainer.position,
            endValue: startRewardCardContainer.position,
            duration,
            useUnscaledTime: true);
    }

    public bool CanShowPreview()
    {
        for (int i = 0; i < rewardCardViews.Length; i++)
        {
            if (!rewardCardViews[i].isShow)
            {
                return true;
            }
        }

        return false;
    }

}
