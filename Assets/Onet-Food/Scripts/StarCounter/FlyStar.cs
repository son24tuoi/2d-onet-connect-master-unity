using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class FlyStar : MonoBehaviour
{
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

    public List<GameObject> starActive = new List<GameObject>();

    public void InitStar(Vector3 start, Vector3 end, float moveTime, float scaleTime, float endDelayScale = 0f, Action onComplete = null)
    {
        GameObject star = ObjectPool.GetStarObject();
        SetupStar(star, start, end, moveTime, scaleTime, endDelayScale, onComplete);
    }

    public void SetupStar(GameObject star, Vector3 start, Vector3 end, float moveTime, float scaleTime, float endDelayScale = 0f, Action onComplete = null)
    {
        star.transform.position = start;
        star.transform.localScale = Vector3.zero;
        star.SetActive(true);

        starActive.Add(star);

        Tween.Scale(star.transform, Vector3.one, scaleTime, Ease.Linear, endDelay: endDelayScale);
        Tween.Position(star.transform, end, moveTime, Ease.Linear, startDelay: scaleTime + endDelayScale)
            .OnComplete(() =>
            {
                star.SetActive(false);
                onComplete?.Invoke();

                starActive.Remove(star);
            });

    }

    public void StopAll()
    {
        for (int i = 0; i < starActive.Count; i++)
        {
            Tween.StopAll(starActive[i].transform);
            starActive[i].SetActive(false);
        }
        starActive.Clear();
    }
}
