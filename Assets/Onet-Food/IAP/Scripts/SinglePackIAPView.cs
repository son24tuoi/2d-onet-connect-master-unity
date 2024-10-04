using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SinglePackIAPView : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    [Header("Datas")]
    public ItemsProfileSO itemsProfileSO;
    public RewardProfileSO rewardProfileSO;

    private Reward reward;

    public void Setup(RewardProfileSO rewardProfileSO)
    {
        this.rewardProfileSO = rewardProfileSO;
        Setup();
    }

    public void Setup()
    {
        reward = rewardProfileSO.GetReward(0);

        if (reward == null)
            return;

        icon.sprite = itemsProfileSO.GetIcon(reward.itemType);
        amountText.SetText(reward.amount.ToString());
    }


    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(SinglePackIAPView))]
    public class SinglePackIAPView_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SinglePackIAPView target = (SinglePackIAPView)base.target;

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
                UnityEditor.EditorUtility.SetDirty(target);
                UnityEditor.EditorUtility.SetDirty(target.icon);
                UnityEditor.EditorUtility.SetDirty(target.amountText);
            }
        }
    }

#endif
}
