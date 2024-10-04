using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayLevelCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI levelTMP;
    [SerializeField] private TextMeshProUGUI timeTMP;
    [SerializeField] private TextMeshProUGUI infoTMP;
    [SerializeField] private LevelInfo levelInfo;

    [Space(10)]
    [SerializeField] private LevelManagerProfileSO levelManagerProfileSO;

    private int m_levelIndex;

    public LevelData LevelData => DataManager.Instance.Data.levelData;

    public void Init(int levelIndex)
    {
        m_levelIndex = levelIndex;

        levelTMP.SetText("Level " + DataManager.Instance.GetLevelName(levelIndex));

        infoTMP.SetText("Break all the squares");

        levelInfo.Setup(LevelData.GetStarWin(levelIndex), LevelData.GetHighScore(levelIndex));

        LevelProfileSO levelProfileSO = levelManagerProfileSO.GetLevelProfileSO(m_levelIndex);

        timeTMP.SetText(levelProfileSO.timeSystem.GetTimePlay());
    }

    public void OnClickPlayButton()
    {
        GameManager.Instance.uiController.ShowLevelSelectionCanvas(false);
        base.Exit(() =>
        {
            GameManager.Instance.PlayLevel(m_levelIndex);
        });
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }
}
