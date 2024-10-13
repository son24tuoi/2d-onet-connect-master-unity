using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(LevelController))]
public class LevelManager : MonoBehaviour
{
    [Header("Data")]
    public LevelManagerProfileSO levelManagerProfileSO;
    public GameProfileSO gameProfileSO;

    [Header("Element")]
    [SerializeField] private LevelController levelController;

    public Timer Timer => levelController.timer;

    public LevelData LevelData => DataManager.Instance.Data.levelData;

    public void LoadLevel(int index)
    {
        if (index >= 0 && index <= levelManagerProfileSO.MaxLevelIndex)
        {
            gameProfileSO.currentLevelIndex = index;
            LoadRealLevel(index);

            CheckShowTutorialGamePlay(index);
        }
        else
        {
            gameProfileSO.currentLevelIndex = -1;
            LoadRandomLevel();
        }
    }

    public void LoadProgressLevel()
    {
        LoadProgressLevel(LevelData.LevelIndex, LevelData.NodeGrid.GetMap(), LevelData.IdCards);
    }

    public void LoadProgressLevel(int index, int[,] map, int[] idCards)
    {
        gameProfileSO.currentLevelIndex = index;
        levelController.Init(
            levelManagerProfileSO.GetLevelProfileSO(index),
            map, idCards,
            LevelData.ElapsedSeconds,
            LevelData.StarsReceived,
            LevelData.AmountMatch);
    }

    public void LoadRealLevel(int index)
    {
        levelController.Init(levelManagerProfileSO.GetLevelProfileSO(index));
    }

    public void LoadRandomLevel()
    {
        levelController.Init(levelManagerProfileSO.GetBonusLevelProfileSO());
    }

    public void Clear()
    {
        levelController.Clear();
    }

    public async void CheckShowTutorialGamePlay(int levelIndex)
    {
        if (levelIndex != 0)
            return;

        if (!DataManager.Instance.TutorialGamePlay)
        {
            DataManager.Instance.SaveTutorialGamePlay();
            await UniTask.NextFrame();
            GameManager.Instance.uiController.ShowTutorialCanvas();
        }
    }
}
