using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelManager levelManager;
    public UIController uiController;
    public GameObject uiCheat;

    [Space(5)]
    public FPSDisplay fpsDisplay;

    [Space(10)]
    public GameProfileSO gameProfileSO;
    public RewardProfileSO rewardProfileSOForWin;

    private DataManager dataManager;

    private void Awake()
    {
        Instance = this;

#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif
    }

    private void Start()
    {
        dataManager = DataManager.Instance;
        gameProfileSO.Init();
        uiController.ShowMainPanel(UIController.MainPanelType.Home);

        AdManager.Instance.ShowBannerAdTask().Forget();
    }

    private void OnEnable()
    {
        LevelController.OnLoseEvent += LoseLevelCallback;
        LevelController.OnWinEvent += WinLevelCallback;
    }

    private void OnDisable()
    {
        LevelController.OnLoseEvent -= LoseLevelCallback;
        LevelController.OnWinEvent -= WinLevelCallback;
    }

    public void PlayLevel()
    {
        uiController.ShowMainPanel(UIController.MainPanelType.GamePlay);
        dataManager.ResetLevel();
        levelManager.LoadLevel(dataManager.Data.levelData.LevelIndex);
    }

    public void LoadProgressLevel()
    {
        uiController.ShowMainPanel(UIController.MainPanelType.GamePlay);
        levelManager.LoadProgressLevel();
    }

    public void NextLevel()
    {
        if (gameProfileSO.currentLevelIndex < 0)
        {
            levelManager.LoadRandomLevel();
        }
        else
        {
            levelManager.LoadLevel(gameProfileSO.currentLevelIndex + 1);
        }
    }

    public void BackToHome()
    {
        uiController.ShowMainPanel(UIController.MainPanelType.Home);
        gameProfileSO.isPlaying = false;
        levelManager.Clear();
    }

    private void LoseLevelCallback()
    {
        uiController.ShowLoseCanvas();
    }

    private void WinLevelCallback()
    {
        dataManager.WinLevel(gameProfileSO.currentLevelIndex, gameProfileSO.starsReceived);

        dataManager.SaveReward(rewardProfileSOForWin);

        uiController.ShowWinCanvas();

        CheckShowInAppReview();

        CheckShowThankYouForPlaying();
    }

    public void Test()
    {
        
    }

    public void CheckShowInAppReview()
    {
        if (gameProfileSO.currentLevelIndex >= 4 && !dataManager.Data.InAppReview)
        {
            dataManager.Data.InAppReview = true;
            dataManager.SaveData();
        }
    }

    public void CheckShowThankYouForPlaying()
    {
        if (gameProfileSO.currentLevelIndex >= 649 && !dataManager.Data.ThankYouForPlaying)
        {
            uiController.ShowThankYouForPlayingCanvas();

            dataManager.Data.ThankYouForPlaying = true;
            dataManager.SaveData();
        }
    }
}
