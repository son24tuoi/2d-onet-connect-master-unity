using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using Cysharp.Threading.Tasks;
using UnityEditor;

public class OpenChestView : MonoBehaviour
{
    public static event Action OnCloseEvent;

    [Header("Element")]
    [SerializeField] private Transform closedChest;
    [SerializeField] private Transform openChest;
    [SerializeField] private Transform startChest;
    [SerializeField] private Transform targetChest;

    [Space(10)]
    [SerializeField] private RewardCardViewController rewardCardViewController;

    [Space(10)]
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject continueButton;

    [Space(10)]
    public ChestProfileSO chestProfileSO;
    public RewardProfileSO rewardProfileSO;

    private void OnEnable()
    {
        RewardCardView.OnShowEvent += ShowRewardCardView;
    }

    private void OnDisable()
    {
        RewardCardView.OnShowEvent -= ShowRewardCardView;
    }

    public void Init()
    {
        closedChest.gameObject.SetActive(true);
        openChest.gameObject.SetActive(false);
        openChest.transform.position = startChest.position;

        rewardCardViewController.Init();

        ShowButtons(false);

        chestProfileSO.enableShowRewardCard = true;
    }

    public void SetupRewardCardViews()
    {
        rewardCardViewController.SetupRewards(rewardProfileSO.GetRewards());
    }

    public async UniTask OpenChestTask()
    {
        await Tween.Scale(closedChest, new Vector3(1.1f, 0.9f, 1f), 0.2f, useUnscaledTime: true);
        await Tween.Scale(closedChest, new Vector3(0.9f, 1.1f, 1f), 0.2f, useUnscaledTime: true);
        await Tween.Scale(closedChest, new Vector3(1f, 1f, 1f), 0.2f, useUnscaledTime: true);

        closedChest.gameObject.SetActive(false);
        openChest.gameObject.SetActive(true);
        await Tween.Scale(openChest, new Vector3(0.9f, 1.1f, 1f), Vector3.one, 0.3f, useUnscaledTime: true);

        await Tween.Position(openChest, targetChest.position, 0.3f, useUnscaledTime: true);

        // _ = Tween.Scale(openChest, Vector3.one, new Vector3(0.97f, 1.02f, 1f), 0.1f,
        //     cycles: 9,
        //     endDelay: 0.05f,
        //     useUnscaledTime: true);

        _ = Sequence.Create(useUnscaledTime: true)
                    .Group(Tween.Scale(openChest, Vector3.one, new Vector3(0.95f, 1.05f, 1f), 1f))
                    .Group(Tween.Scale(openChest, new Vector3(0.95f, 1.05f, 1f), Vector3.one, 1f));

        await rewardCardViewController.FlyRewardCardViewTempsTask(openChest.transform.position);

        SetupRewardCardViews();
    }

    private void ShowRewardCardView(Reward reward)
    {
        ShowButtonsTask().Forget();
    }

    private async UniTask ShowButtonsTask()
    {
        chestProfileSO.enableShowRewardCard = false;

        await rewardCardViewController.MoveToTargetPositionTask();

        ShowButtons();

        rewardCardViewController.ShowPreview();
    }

    private async UniTask HideButtonsTask()
    {
        chestProfileSO.enableShowRewardCard = true;
        ShowButtons(false);

        rewardCardViewController.FaceDown();

        await rewardCardViewController.MoveToStartPositionTask();
    }

    public void OnClickDoneButton()
    {
        OnCloseEvent?.Invoke();
    }

    public void OnClickContinueButton()
    {
        AdManager.Instance.ShowRewardedAd(onReward: () =>
        {
            HideButtonsTask().Forget();
        }, onClose: () =>
        {

        }, onNotReady: () =>
        {
            EventManager.Instance.Trigger(new EventData<NotificationData>(
                EventID.Notification,
                new NotificationData(
                    "Ad are not available",
                    NotificationColorType.Yellow
                )
            ));
        });
    }

    public void ShowButtons(bool show = true)
    {
        buttonContainer.SetActive(show);

        if (show)
        {
            continueButton.SetActive(rewardCardViewController.CanShowPreview());
        }
    }





#if UNITY_EDITOR
    [CustomEditor(typeof(OpenChestView))]
    public class OpenChestView_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            OpenChestView target = (OpenChestView)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Init"))
            {
                target.Init();
            }

            if (GUILayout.Button("Setup Reward Card Views"))
            {
                target.SetupRewardCardViews();
            }

            if (GUILayout.Button("Open Chest"))
            {
                target.OpenChestTask().Forget();
            }
        }
    }
#endif
}
