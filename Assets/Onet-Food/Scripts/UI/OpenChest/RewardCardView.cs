using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;
using Cysharp.Threading.Tasks;
using UnityEditor;
using TMPro;
using System;

public class RewardCardView : MonoBehaviour
{
    public static event Action<Reward> OnShowEvent;

    [Header("Element")]
    [SerializeField] private Transform faceDown;
    [SerializeField] private Transform faceUp;
    [SerializeField] private GameObject faceUpLock;

    [Space(10)]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    [Space(10)]
    public ItemsProfileSO itemsProfileSO;
    public ChestProfileSO chestProfileSO;

    [Space(10)]
    public TweenShakeRotation tweenShakeRotation;

    [Header("Data")]
    public Reward reward;

    public bool isShow = false;

    public void Init()
    {
        faceDown.gameObject.SetActive(true);
        faceUp.gameObject.SetActive(false);

        isShow = false;
    }

    public void SetupReward(Reward reward)
    {
        this.reward = reward;

        SetupReward();
    }

    public void SetupReward()
    {
        itemIcon.sprite = itemsProfileSO.GetIcon(reward.itemType);
        amountText.SetText(reward.amount.ToString());
    }

    public void Show()
    {
        if (chestProfileSO.enableShowRewardCard)
        {
            chestProfileSO.enableShowRewardCard = false;
            tweenShakeRotation.Stop();
            SetLockRewardCard(false);
            ShowTask().Forget();
            isShow = true;
        }
    }

    public void ShowPreview()
    {
        if (!chestProfileSO.enableShowRewardCard)
        {
            SetLockRewardCard(true);
            TurnOnTask().Forget();
        }
    }

    public async UniTask ShowTask()
    {
        await TurnOnTask();

        OnShowEvent?.Invoke(reward);
    }

    public async UniTask TurnOnTask()
    {
        await Tween.LocalRotation(faceDown, endValue: new Vector3(0, 90, 0), duration: 0.3f, useUnscaledTime: true);

        faceDown.localRotation = Quaternion.identity;
        faceDown.gameObject.SetActive(false);
        faceUp.gameObject.SetActive(true);

        await Tween.LocalRotation(faceUp, startValue: new Vector3(0, -90, 0), endValue: new Vector3(0, 0, 0), duration: 0.3f, useUnscaledTime: true);
    }

    public async UniTask TurnOffTask()
    {
        await Tween.LocalRotation(faceUp, endValue: new Vector3(0, 90, 0), duration: 0.3f, useUnscaledTime: true);

        faceUp.localRotation = Quaternion.identity;
        faceUp.gameObject.SetActive(false);
        faceDown.gameObject.SetActive(true);

        await Tween.LocalRotation(faceDown, startValue: new Vector3(0, -90, 0), endValue: new Vector3(0, 0, 0), duration: 0.3f, useUnscaledTime: true);
    }

    public void SetLockRewardCard(bool isLock = true)
    {
        faceUpLock.SetActive(isLock);
    }





#if UNITY_EDITOR
    [CustomEditor(typeof(RewardCardView))]
    public class RewardCardView_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            RewardCardView target = (RewardCardView)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = UnityEngine.FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Init"))
            {
                target.Init();
            }

            if (GUILayout.Button("Setup Reward"))
            {
                target.SetupReward();
            }

            if (GUILayout.Button("Show"))
            {
                target.ShowTask().Forget();
            }
        }
    }
#endif
}
