using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RewardView : MonoBehaviour
{
    #region Fields

    [Header("Elements")]
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;

    #endregion Fields

    // -------------------------------------------------------------------------------------------------

    #region Properties



    #endregion Properties

    // -------------------------------------------------------------------------------------------------

    #region Unity Lifecycle Methods



    #endregion Unity Lifecycle Methods

    // -------------------------------------------------------------------------------------------------

    public void Setup(Sprite icon, int amount)
    {
        image.sprite = icon;
        amountText.SetText(amount.ToString());
    }

#if UNITY_EDITOR
    public void SaveChangeInspector()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EditorUtility.SetDirty(transform.GetChild(i).gameObject);
        }
    }
#endif
}
