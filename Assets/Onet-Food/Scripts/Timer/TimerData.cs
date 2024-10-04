using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TimerData
{
    [SerializeField] private bool m_started = false;
    public bool running = false;
    public bool finished = false;

    [SerializeField] private float m_totalSeconds = 0;
    public float elapsedSeconds = 0;

    public bool Started
    {
        get { return m_started; }
    }

    public float Duration
    {
        set { if (!running) m_totalSeconds = value; }
        get { return m_totalSeconds; }
    }

    public float Remaining
    {
        get { return m_totalSeconds - elapsedSeconds; }
    }

    public void Setup(float totalSeconds)
    {
        m_totalSeconds = totalSeconds;

        // only run with valid duration
        if (m_totalSeconds <= 0)
            return;

        m_started = true;
        running = true;
        finished = false;

        elapsedSeconds = 0;
    }

    public void Reset()
    {
        m_started = false;
        running = false;
        finished = false;

        m_totalSeconds = 0;
        elapsedSeconds = 0;
    }

    public void AddDuration(int seconds)
    {
        m_totalSeconds += seconds;
    }
}
