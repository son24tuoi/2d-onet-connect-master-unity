using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combo : MonoBehaviour
{
    public static event Action OnStartComboEvent;

    [Header("Element")]
    public ComboProfileSO comboProfileSO;
    public TimeProfileSO timeProfileSO;

    public TimerData TimerData => comboProfileSO.timerData;

    private Coroutine m_cooldownRoutine;

    public void Init()
    {
        comboProfileSO.Reset();
    }

    public void Setup()
    {
        comboProfileSO.comboIndex++;
        StartCoolDown(2);

        if (comboProfileSO.comboIndex >= 2)
        {
            timeProfileSO.AddElapsedSeconds(-comboProfileSO.reward);
        }
    }

    public void StartCoolDown(float time)
    {
        if (m_cooldownRoutine != null)
        {
            StopCoroutine(m_cooldownRoutine);
        }

        TimerData.Setup(time);
        m_cooldownRoutine = StartCoroutine(IECountdown());

        OnStartComboEvent?.Invoke();
    }

    public IEnumerator IECountdown()
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

        comboProfileSO.Reset();
    }
}
