using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;

public class LevelSelectionCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TextMeshProUGUI playButtonTMP;

    [Space(10)]
    public LevelManagerProfileSO levelManagerProfileSO;

    private int m_levelIndex;

    protected DataManager m_dataManager;

    public DataManager DataManager
    {
        get
        {
            if (ReferenceEquals(m_dataManager, null))
            {
                m_dataManager = DataManager.Instance;
            }
            return m_dataManager;
        }
    }

    protected GameManager m_gameManager;

    public GameManager GameManager
    {
        get
        {
            if (ReferenceEquals(m_gameManager, null))
            {
                m_gameManager = GameManager.Instance;
            }
            return m_gameManager;
        }
    }

    protected override void OnEnable()
    {
        gameProfileSO.enablePlayerController = false;

        Init();
    }

    public void Init()
    {
        int levelIndex = DataManager.Data.levelData.LevelIndex;
        m_levelIndex = (levelIndex <= levelManagerProfileSO.MaxLevelIndex) ? levelIndex : -1;
        playButtonTMP.SetText("Level " + DataManager.GetLevelName(m_levelIndex));
    }

    public void OnClickExitButton()
    {
        m_tweenFadeCanvasGroup.Fade(1f, 0f, 0.2f, Ease.OutQuad, complete: () =>
        {
            gameObject.SetActive(false);
            GameManager.uiController.ShowMainPanel(UIController.MainPanelType.Home);
        });
    }

    public void OnClickPlayButton()
    {
        GameManager.uiController.ShowPlayLevelCanvas(m_levelIndex);
    }

    public void OnClickShopButton()
    {
        EventManager.Instance.Trigger(EventID.ShopCanvas);
    }
}
