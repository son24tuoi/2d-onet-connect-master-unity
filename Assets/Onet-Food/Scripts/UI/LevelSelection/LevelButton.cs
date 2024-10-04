using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelButton : MyMonoBehaviour
{
    [Header("Element")]
    public LevelLockButton levelLockButton;
    public LevelUnlockButton levelUnlockButton;

    [Space(10)]
    public GameProfileSO gameProfileSO;

    private int m_level;
    private int m_star;
    private StateType m_stateType;

    public enum StateType
    {
        None,
        Lock,
        Unlock
    }

    public void Setup(int level, int star)
    {
        m_level = level;
        m_star = star;

        Setup();
    }

    public void Setup()
    {
        m_stateType = GetStateType(m_level);

        levelLockButton.gameObject.SetActive(m_stateType == StateType.Lock);
        levelUnlockButton.gameObject.SetActive(m_stateType == StateType.Unlock);

        if (m_stateType == StateType.Unlock)
        {
            levelUnlockButton.Init(m_level, m_star);
        }
        else
        {
            levelLockButton.Init(m_level);
        }
    }

    public StateType GetStateType(int level)
    {
        if (level >= 0)
        {
            if (level <= DataManager.Data.levelData.LevelIndex) // Level đã vượt qua và chưa vượt qua
            {
                return StateType.Unlock;
            }
            else // Level đang khóa
            {
                return StateType.Lock;
            }
        }

        return StateType.None;
    }

    public void OnClickButton()
    {
        if (gameProfileSO.isCheat)
        {
            GameManager.Instance.uiController.ShowPlayLevelCanvas(m_level);
            return;
        }

        switch (m_stateType)
        {
            case StateType.Unlock:
                GameManager.Instance.uiController.ShowPlayLevelCanvas(m_level);
                break;
        }
    }
}
