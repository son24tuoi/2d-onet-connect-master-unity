using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using System;
using Cysharp.Threading.Tasks;

public class PathView : MonoBehaviour, IEventHandlerWithData
{
    public static event Action OnDoneShowEvent;

    [Header("Element")]
    public StarCounter starCounter;

    [Header("Setting")]
    public float starSpeedFactor = 0.0001f;

    private Transform m_starDestination;

    public int amountStar;

    public bool IsShow
    {
        get => amountStar > 0;
    }

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

    private Camera m_mainCamera;

    public Camera MainCamera
    {
        get
        {
            if (ReferenceEquals(m_mainCamera, null))
            {
                m_mainCamera = Camera.main;
            }
            return m_mainCamera;
        }
    }

    private void Start()
    {
        EventManager.Instance.Subcribe(EventID.StarDestination, this);
    }

    private void OnDestroy()
    {
        EventManager.Instance.Unsubcribe(EventID.StarDestination, this);
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

            default:
                Debug.Log("Unknown EventID");
                break;
        }
    }

    private void UpdateStarDestination(Transform starTransform)
    {
        m_starDestination = starTransform;
    }

    public async UniTask ShowTask(List<Node> foundNodes, bool found)
    {
        float delayOffLine = found ? 0.2f : 0.3f;
        await ShowLineTask(foundNodes, found ? Color.yellow : Color.red, delayOffLine);

        if (found)
        {
            await ShowStarTask(foundNodes);
            OnDoneShowEvent?.Invoke();
        }
    }

    public async UniTask ShowLineTask(List<Node> foundNodes, Color color, float delayOff)
    {
        List<GameObject> lines = new List<GameObject>();
        for (int i = 0; i < foundNodes.Count - 1; i++)
        {
            GameObject line = ObjectPool.GetLineObject();
            SetupLine(line, foundNodes[i].position, foundNodes[i + 1].position, color);
            line.SetActive(true);
            lines.Add(line);
        }

        await UniTask.Delay(TimeSpan.FromSeconds(delayOff), true);

        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SetActive(false);
        }
    }

    public void SetupLine(GameObject line, Vector3 from, Vector3 to, Color color)
    {
        if (line != null)
        {
            line.transform.position = from;

            Vector3 dir = (to - from).normalized;
            line.transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);

            if (line.TryGetComponent(out SpriteRenderer sr))
            {
                sr.material.color = color;
            }
        }
    }

    public async UniTask ShowStarTask(List<Node> foundNodes)
    {
        float moveTime = 0;
        float scaleDuration = 0.1f;
        float endDelayScale = 0.05f;
        float maxMoveTime = 0;
        Vector3 pos;

        for (int i = 0; i < foundNodes.Count; i++)
        {
            pos = MainCamera.WorldToScreenPoint(foundNodes[i].position);
            moveTime = Vector3.Distance(pos, m_starDestination.position) * starSpeedFactor;
            if (moveTime > maxMoveTime)
            {
                maxMoveTime = moveTime;
            }

            starCounter.InitStar(pos, m_starDestination.position, moveTime, scaleDuration, endDelayScale, () =>
            {
                amountStar--;
            });
            amountStar++;
        }

        await UniTask.Delay(TimeSpan.FromSeconds(maxMoveTime + scaleDuration + endDelayScale), true);
    }

    public void StopAll()
    {

    }
}
