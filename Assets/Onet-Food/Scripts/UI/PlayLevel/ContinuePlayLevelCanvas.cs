using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContinuePlayLevelCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI infoTMP;
    [SerializeField] private AreYouSureRestartPanel areYouSureRestartPanelPrefab;

    public void Init()
    {
        infoTMP.SetText("You are playing level " + DataManager.Instance.GetLevelName() + "\nDo you want continue?");
    }

    public void OnClickRestartButton()
    {
        AreYouSureRestartPanel areYouSureRestartPanel = Instantiate(areYouSureRestartPanelPrefab, transform);
        areYouSureRestartPanel.onYesEvent = RestartLevel;
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }

    public void OnClickContinueButton()
    {
        base.Exit(() =>
        {
            GameManager.Instance.LoadProgressLevel();
        });
    }

    private void RestartLevel()
    {
        base.Exit(() =>
        {
            GameManager.Instance.PlayLevel();
        });
    }
}
