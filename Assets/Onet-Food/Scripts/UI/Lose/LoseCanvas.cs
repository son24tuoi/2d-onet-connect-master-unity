using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class LoseCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private SummaryView m_summaryView;
    [SerializeField] private GameObject[] contents;
    [SerializeField] private TweenScale outOfTimeScale;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init();
    }

    public void Init()
    {
        m_summaryView.Init(gameProfileSO.currentLevelIndex, false);

        ShowContents(0);
    }

    public void ShowContents(int index)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            contents[i].SetActive(i == index);
        }
    }

    public void OnClickHomeButton()
    {
        GameManager.Instance.BackToHome();
        Exit();
    }

    public void OnClickReplayButton()
    {
        GameManager.Instance.PlayLevel();
        Exit();
    }

    public void Revive()
    {
        outOfTimeScale.Scale(Vector3.one, Vector3.zero, 0.35f, Ease.InBack, () =>
        {
            gameObject.SetActive(false);
        });
    }
}
