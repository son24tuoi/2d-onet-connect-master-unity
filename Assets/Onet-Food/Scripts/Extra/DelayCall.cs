using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DelayCall
{
    private readonly float delayTime;
    private readonly Action action;
    private readonly bool ignoreTimeScale;
    private CancellationTokenSource cancellationTokenSource;

    public float DelayTime
    {
        get => delayTime;
    }

    public DelayCall(float delayTime, Action action, bool ignoreTimeScale = false)
    {
        this.delayTime = delayTime;
        this.action = action;
        this.ignoreTimeScale = ignoreTimeScale;
    }

    public async void StartDelayCall()
    {
        cancellationTokenSource = new CancellationTokenSource();
        CancellationToken token = cancellationTokenSource.Token;

        try
        {
            await UniTask.WaitForSeconds(delayTime, ignoreTimeScale: ignoreTimeScale, cancellationToken: token);

            action?.Invoke();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void CancelDelayCall()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
        }
    }
}