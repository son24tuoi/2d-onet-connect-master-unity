using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardContainerView : MonoBehaviour
{
    #region Fields

    [Header("Elements")]
    [SerializeField] private RewardView[] rewardViews;

    [Header("Config")]
    public ItemsProfileSO itemsProfileSO;
    public RewardProfileSO rewardProfileSO;

    #endregion Fields

    // -------------------------------------------------------------------------------------------------

    #region Properties



    #endregion Properties

    // -------------------------------------------------------------------------------------------------

    #region Unity Lifecycle Methods



    #endregion Unity Lifecycle Methods

    // -------------------------------------------------------------------------------------------------

    public void Setup(RewardProfileSO rewardProfileSO)
    {
        this.rewardProfileSO = rewardProfileSO;

        Setup();
    }

    public void Setup()
    {
        int length = rewardProfileSO.Amount;
        Reward reward;
        for (int i = 0; i < length; i++)
        {
            reward = rewardProfileSO.rewards[i];
            rewardViews[i].Setup(itemsProfileSO.GetIcon(reward.itemType), reward.amount);
            rewardViews[i].gameObject.SetActive(true);
        }

        if (rewardViews.Length > length)
        {
            for (int i = length; i < rewardViews.Length; i++)
            {
                rewardViews[i].gameObject.SetActive(false);
            }
        }
    }













    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(RewardContainerView))]
    public class RewardContainerView_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            RewardContainerView target = (RewardContainerView)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.Setup)))
            {
                target.Setup();

                UnityEditor.EditorUtility.SetDirty(this);
                for (int i = 0; i < target.rewardViews.Length; i++)
                {
                    target.rewardViews[i].SaveChangeInspector();
                }
            }
        }
    }

#endif

    // -------------------------------------------------------------------------------------------------


}
