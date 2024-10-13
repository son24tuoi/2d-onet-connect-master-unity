using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEditor;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static event Action OnStartEvent;

    [Header("Element")]
    public TimeProfileSO timeProfileSO;
    public GameProfileSO gameProfileSO;

    private Action m_timeoutEvent;

    private Coroutine m_updateRoutine;

    public TimerData TimerData => timeProfileSO.timerData;

    public void Setup(TimeSystem timeSystem, Action timeoutEvent)
    {
        timeProfileSO.Setup(timeSystem);

        m_timeoutEvent = timeoutEvent;
    }

    public void StartTimer()
    {
        StopTimer();

        timeProfileSO.timerData.running = true;
        m_updateRoutine = StartCoroutine(IEUpdate());

        OnStartEvent?.Invoke();
    }

    public void StopTimer()
    {
        TimerData.running = false;

        if (m_updateRoutine != null)
        {
            StopCoroutine(m_updateRoutine);
        }
    }

    public IEnumerator IEUpdate()
    {
        while (TimerData.running)
        {
            TimerData.elapsedSeconds += Time.deltaTime;
            if (TimerData.elapsedSeconds >= TimerData.Duration)
            {
                TimerData.running = false;
                TimerData.finished = true;
            }
            yield return null;
        }

        if (TimerData.finished)
        {
            m_timeoutEvent?.Invoke();
        }
    }

    public void AddDuration(int seconds)
    {
        timeProfileSO.AddDuration(seconds);
    }

    public void SetElapsedSeconds(float seconds)
    {
        timeProfileSO.timerData.elapsedSeconds = seconds;
    }
}
