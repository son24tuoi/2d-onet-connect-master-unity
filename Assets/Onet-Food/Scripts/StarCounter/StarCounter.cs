using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class StarCounter : MonoBehaviour, IEventHandlerWithData
{
    [Header("Element")]
    public FlyStar flyStar;
    public GameProfileSO gameProfileSO;
    public TimeProfileSO timeProfileSO;

    private Transform m_starTransform;
    private Vector3[] m_threeStarsPos;

    private int m_amountStar;

    private ObjectPool m_objectPool;

    public ObjectPool ObjectPool
    {
        get
        {
            if (ReferenceEquals(m_objectPool, null))
            {
                m_objectPool = ObjectPool.Instance;
            }
            return m_objectPool;
        }
    }

    private void Start()
    {
        EventManager.Instance.Subcribe(EventID.StarDestination, this);
        EventManager.Instance.Subcribe(EventID.ThreeStarsPosition, this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubcribe(EventID.StarDestination, this);
        EventManager.Instance.Unsubcribe(EventID.ThreeStarsPosition, this);
    }

    public void EventHandler<T>(EventData<T> eventData)
    {
        switch (eventData.eventID)
        {
            case EventID.StarDestination:
                if (eventData.data is Transform starTransform)
                {
                    UpdateStarDestination(starTransform);
                }
                break;

            case EventID.ThreeStarsPosition:
                if (eventData.data is Vector3[] pos)
                {
                    UpdateThreeStarsPosition(pos);
                }
                break;

            default:
                break;
        }
    }

    private void UpdateStarDestination(Transform starTransform)
    {
        m_starTransform = starTransform;
    }

    private void UpdateThreeStarsPosition(Vector3[] pos)
    {
        m_threeStarsPos = new Vector3[pos.Length];
        for (int i = 0; i < pos.Length; i++)
        {
            m_threeStarsPos[i] = pos[i];
        }
    }

    public void InitStar(Vector3 start, float moveTime, float scaleTime, float endDelayScale = 0f, Action onComplete = null)
    {
        flyStar.InitStar(start, m_starTransform.position, moveTime, scaleTime, endDelayScale, onComplete);
    }

    public void InitStar(Vector3 start, Vector3 end, float moveTime, float scaleTime, float endDelayScale = 0f, Action onComplete = null)
    {
        flyStar.InitStar(start, end, moveTime, scaleTime, endDelayScale, () =>
        {
            gameProfileSO.AddStarsReceived(1);
            onComplete?.Invoke();
        });
    }

    public void RunToEnd(float duration, Action onComplete)
    {
        StartCoroutine(IERunElapsedTime(duration,
            timeProfileSO.timerData.elapsedSeconds,
            timeProfileSO.timerData.Duration,
            onComplete));
    }

    public IEnumerator IERunElapsedTime(float duration, float start, float end, Action onComplete)
    {
        float elapsed = 0f;
        int preSeconds = (int)start;

        float timeThreeStar = timeProfileSO.timeSystem.maxPlayTime - timeProfileSO.timeSystem.TimeThreeStar;
        float timeTwoStar = timeProfileSO.timeSystem.maxPlayTime - timeProfileSO.timeSystem.TimeTwoStar;
        float timeOneStar = timeProfileSO.timeSystem.maxPlayTime - timeProfileSO.timeSystem.TimeOneStar;

        float moveTime = 0.2f;
        float scaleDuration = 0.1f;
        float endDelayScale = 0f;

        m_amountStar = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float seconds = Mathf.Lerp(start, end, t);
            timeProfileSO.timerData.elapsedSeconds = seconds;

            if (timeThreeStar < seconds && timeThreeStar >= preSeconds)
            {
                m_amountStar += 150;
                InitStar(m_threeStarsPos[2], moveTime, scaleDuration, endDelayScale, () =>
                {
                    m_amountStar -= 150;
                });
                gameProfileSO.AddStarsReceived(150);
            }
            else if (timeTwoStar < seconds && timeTwoStar >= preSeconds)
            {
                m_amountStar += 100;
                InitStar(m_threeStarsPos[1], moveTime, scaleDuration, endDelayScale, () =>
                {
                    m_amountStar -= 100;
                });
                gameProfileSO.AddStarsReceived(100);
            }
            else if (timeOneStar < seconds && timeOneStar >= preSeconds)
            {
                m_amountStar += 50;
                InitStar(m_threeStarsPos[0], moveTime, scaleDuration, endDelayScale, () =>
                {
                    m_amountStar -= 50;
                });
                gameProfileSO.AddStarsReceived(50);
            }

            if ((int)seconds > preSeconds)
            {
                preSeconds = (int)seconds;
                gameProfileSO.AddStarsReceived(1);
            }
            yield return null;
        }

        while (m_amountStar > 0)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        onComplete?.Invoke();
    }

    public void StopAll()
    {
        flyStar.StopAll();
    }
}
