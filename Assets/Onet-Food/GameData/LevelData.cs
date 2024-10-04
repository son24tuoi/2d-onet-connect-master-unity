using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData : ILevelData
{
    [SerializeField] private int m_levelIndex;
    [SerializeField] private List<int> m_starWin;
    [SerializeField] private List<int> m_highScores;
    [SerializeField] private List<int> m_elapsedTime;
    [SerializeField] private bool m_tutorialGamePlay = false;

    public int LevelIndex
    {
        get { return m_levelIndex; }
        // set { m_levelIndex = value; }
    }

    public List<int> StarWin
    {
        get { return m_starWin; }
    }

    public List<int> HighScores
    {
        get { return m_highScores; }
    }

    public List<int> ElapsedTime
    {
        get { return m_elapsedTime; }
    }

    public bool TutorialGamePlay
    {
        get { return m_tutorialGamePlay; }
        set { m_tutorialGamePlay = value; }
    }

    public LevelData()
    {
        m_levelIndex = 0;
        m_starWin = new List<int>();
        m_highScores = new List<int>();
        m_elapsedTime = new List<int>();
        m_tutorialGamePlay = false;
    }

    public void IncreaseLevel(int starWin, int highScore, int elapsedSeconds)
    {
        m_levelIndex++;
        m_starWin.Add(starWin);
        m_highScores.Add(highScore);
        m_elapsedTime.Add(elapsedSeconds);
    }

    public int GetStarWin(int levelIndex)
    {
        if (levelIndex >= 0 && m_starWin.Count > levelIndex)
        {
            return m_starWin[levelIndex];
        }

        return -1;
    }

    public int GetHighScore(int levelIndex)
    {
        if (levelIndex >= 0 && m_highScores.Count > levelIndex)
        {
            return m_highScores[levelIndex];
        }

        return -1;
    }

    public bool OverrideStar(int index, int starWin)
    {
        if (starWin > m_starWin[index])
        {
            m_starWin[index] = starWin;
            return true;
        }

        return false;
    }

    public bool OverrideHighScore(int index, int highScore)
    {
        if (highScore > m_highScores[index])
        {
            m_highScores[index] = highScore;
            return true;
        }

        return false;
    }

    public bool OverrideElapsedTime(int index, int elapsedSeconds)
    {
        if (elapsedSeconds < m_elapsedTime[index])
        {
            m_elapsedTime[index] = elapsedSeconds;
            return true;
        }

        return false;
    }

    public bool IsNewHighScore(int index, int highScore)
    {
        if (index < 0)
            return true;

        if (index < m_highScores.Count)
        {
            return highScore > m_highScores[index];
        }

        return true;
    }
}

public interface ILevelData
{
    public int LevelIndex { get; }
}