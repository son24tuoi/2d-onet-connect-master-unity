using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI levelText;

    protected override void OnEnable()
    {
        base.OnEnable();

        SetLevelText("Level " + DataManager.Instance.GetLevelName(gameProfileSO.currentLevelIndex));
    }

    public void SetLevelText(int level)
    {
        levelText.SetText(level.ToString());
    }

    public void SetLevelText(string level)
    {
        levelText.SetText(level);
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }

    public void OnClickHomeButton()
    {
        base.Exit();
        GameManager.Instance.ReturnLevelSelection();
    }

    public void OnClickReplayButton()
    {
        base.Exit();
        GameManager.Instance.PlayLevel(gameProfileSO.currentLevelIndex);
    }

    public void OnClickSettingButton()
    {
        GameManager.Instance.uiController.ShowSettingCanvas();
    }
}
