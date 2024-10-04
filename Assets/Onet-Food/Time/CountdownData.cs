using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CountdownData
{
    [SerializeField] private float duration;
    [SerializeField] private float remainingTime;
    private Action onComplete;

    public CountdownData(float duration, Action onComplete)
    {
        this.duration = duration;
        this.remainingTime = duration;
        this.onComplete = onComplete;
    }

    public bool Update(float deltaTime)
    {
        remainingTime -= deltaTime;
        if (remainingTime <= 0)
        {
            onComplete?.Invoke();
            return true;
        }
        return false;
    }
}
