using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("Datas")]
    [SerializeField] private List<CountdownData> countdownDatas = new List<CountdownData>();

    private float deltaTime;

    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        for (int i = 0; i < countdownDatas.Count; i++)
        {
            if (countdownDatas[i].Update(deltaTime))
            {
                countdownDatas.RemoveAt(i);
            }
        }
    }

    private void Init()
    {

    }

    public void RegisterCountdown(float duration, Action onComplete)
    {
        countdownDatas.Add(new CountdownData(duration, onComplete));
    }
}
