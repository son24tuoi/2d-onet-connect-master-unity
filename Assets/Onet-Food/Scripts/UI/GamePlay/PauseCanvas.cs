using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private AreYouSureExitPanel areYouSureExitPanelPrefab;

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
        AreYouSureExitPanel areYouSureExitPanel = Instantiate(areYouSureExitPanelPrefab, transform);
        areYouSureExitPanel.onYesEvent = BackHome;
    }

    private void BackHome()
    {
        base.Exit();
        GameManager.Instance.BackToHome();
    }

    public void OnClickReplayButton()
    {
        base.Exit();
        // GameManager.Instance.PlayLevel();
    }

    public void OnClickSettingButton()
    {
        GameManager.Instance.uiController.ShowSettingCanvas();
    }
}
