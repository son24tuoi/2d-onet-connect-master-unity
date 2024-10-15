using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinuePlayLevelCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI infoTMP;

    public void Init()
    {
        infoTMP.SetText("You are playing level " + DataManager.Instance.GetLevelName() + "\nDo you want continue?");
    }

    public void OnClickPlayButton()
    {
        base.Exit(() =>
        {
            GameManager.Instance.PlayLevel();
        });
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }

    public void OnClickResumeButton()
    {
        base.Exit(() =>
        {
            GameManager.Instance.LoadProgressLevel();
        });
    }
}
