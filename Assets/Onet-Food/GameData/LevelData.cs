using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [SerializeField] private bool m_tutorialGamePlay;

    [Header("Progress Level")]
    [SerializeField] private bool m_isPlaying;
    [SerializeField] private int m_levelIndex;
    [SerializeField] private NodeGrid m_nodeGrid;
    [SerializeField] private int[] m_idCards;
    [SerializeField] private float m_elapsedSeconds;
    [SerializeField] private int m_starsReceived;
    [SerializeField] private int m_amountMatch;

    public int LevelIndex
    {
        get { return m_levelIndex; }
        // set { m_levelIndex = value; }
    }

    public bool TutorialGamePlay
    {
        get { return m_tutorialGamePlay; }
        set { m_tutorialGamePlay = value; }
    }

    public bool IsPlaying
    {
        get { return m_isPlaying; }
        set { m_isPlaying = value; }
    }

    public int[] IdCards
    {
        get { return m_idCards; }
        set { m_idCards = value; }
    }

    public NodeGrid NodeGrid
    {
        get { return m_nodeGrid; }
        set { m_nodeGrid = value; }
    }

    public float ElapsedSeconds
    {
        get { return m_elapsedSeconds; }
        set { m_elapsedSeconds = value; }
    }

    public int StarsReceived
    {
        get { return m_starsReceived; }
        set { m_starsReceived = value; }
    }

    public int AmountMatch
    {
        get { return m_amountMatch; }
        set { m_amountMatch = value; }
    }

    public LevelData()
    {
        m_levelIndex = 0;
        m_tutorialGamePlay = false;
        m_isPlaying = false;

        m_elapsedSeconds = 0f;
        m_starsReceived = 0;
        m_amountMatch = 0;
    }

    public void IncreaseLevel()
    {
        m_levelIndex++;
    }
}