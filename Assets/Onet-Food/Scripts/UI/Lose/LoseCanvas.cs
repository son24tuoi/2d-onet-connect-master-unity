using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private SummaryView m_summaryView;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        m_summaryView.Init(gameProfileSO.currentLevelIndex, false);
    }

    public void OnClickHomeButton()
    {
        GameManager.Instance.BackToHome();
        Exit();
    }

    public void OnClickReplayButton()
    {
        GameManager.Instance.PlayLevel(gameProfileSO.currentLevelIndex);
        Exit();
    }
}
