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
    private LevelController m_levelController;

    public Timer Timer => m_levelController.timer;

    private void Awake()
    {
        m_levelController = GetComponent<LevelController>();
    }

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

    public void LoadRealLevel(int index)
    {
        m_levelController.Init(levelManagerProfileSO.GetLevelProfileSO(index));
    }

    public void LoadRandomLevel()
    {
        m_levelController.Init(levelManagerProfileSO.GetBonusLevelProfileSO());
    }

    public void Clear()
    {
        m_levelController.Clear();
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
