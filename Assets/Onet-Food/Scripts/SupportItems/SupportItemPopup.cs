using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;

using ItemType = ItemsData.ItemType;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class SupportItemPopup : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image icon;
    [SerializeField] private TweenFadeCanvasGroup fadeItemButtonGroup;
    [SerializeField] private TweenFadeCanvasGroup fadeGroup;
    [SerializeField] private TweenScale iconScale;
    [SerializeField] private TweenHeaving iconHeaving;
    [SerializeField] private TweenMove[] tweenMoves;
    [SerializeField] private TweenScale[] tweenScales;

    [Header("Config")]
    [SerializeField] private ItemType itemType;
    public ItemsProfileSO itemsProfileSO;

    public void Show(ItemType itemType)
    {
        this.itemType = itemType;
        SetIcon(itemType);
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        fadeItemButtonGroup.ReverseFade(fadeGroup.duration);
        fadeGroup.Fade();
        iconScale.Scale(() =>
        {
            iconHeaving.enabled = true;
        });

        int length = tweenMoves.Length;
        for (int i = 0; i < length; i++)
        {
            tweenMoves[i].LocalMoveUI();
            tweenScales[i].Scale();
        }
    }

    public async void Hide()
    {
        fadeGroup.ReverseFade();
        iconHeaving.enabled = false;
        iconScale.ReverseScale();
        fadeGroup.ReverseFade();

        fadeItemButtonGroup.Fade(fadeGroup.duration);
        await UniTask.WaitForSeconds(fadeGroup.duration);
        gameObject.SetActive(false);
    }

    public void SetIcon(ItemType itemType)
    {
        icon.sprite = itemsProfileSO.GetIcon(itemType);
    }

    public void OnClickButton()
    {
        EventManager.Instance.Trigger(new EventData<ItemType>(
            EventID.UseSupportItem,
            itemType
        ));
    }






#if UNITY_EDITOR

    [CustomEditor(typeof(SupportItemPopup))]
    public class SupportItemPopup_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SupportItemPopup target = (SupportItemPopup)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.Show)))
            {
                target.Show();
            }

            if (GUILayout.Button(nameof(target.Hide)))
            {
                target.Hide();
            }

            if (GUILayout.Button("Update Icon"))
            {
                target.SetIcon(target.itemType);
                EditorUtility.SetDirty(target.iconScale.gameObject);
            }
        }
    }

#endif
}
