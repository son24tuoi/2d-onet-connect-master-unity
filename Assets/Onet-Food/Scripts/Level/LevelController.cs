using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class LevelController : MyMonoBehaviour, IEventHandler
{
    public static event Action OnWinEvent;
    public static event Action OnLoseEvent;

    [Header("Element")]
    public CardData cardData;
    public Graph graph;
    public GraphView graphView;

    public Pathfinder pathfinder;
    public Suggester suggester;
    public Shuffle shuffle;
    public Alignment alignment;

    public Timer timer;

    public StarCounter starCounter;

    public Combo combo;

    [Header("UI")]
    public GamePlayCanvas gamePlayCanvas;

    [Header("Config")]
    public GameProfileSO gameProfileSO;
    public AlignmentType alignType = AlignmentType.None;

    private LevelProfileSO m_levelProfileSO;
    private int m_amountCard;
    private int m_amountCardEffect;

    private int m_amountMatch;

    private bool IsPlaying
    {
        get => gameProfileSO.isPlaying;
        set => gameProfileSO.isPlaying = value;
    }

    public bool IsClearCardEffect
    {
        get => m_amountCardEffect <= 0;
    }

    public bool IsClearCard
    {
        get => m_amountCard <= 0;
    }

    private void Start()
    {
        PlayerSelection.OnPathFindingEvent += FindPath;

        PathView.OnDoneShowEvent += CheckWin;

        ScreenDetector.OnChangeScreenOrientationEvent += SetPositionCamera;

        EventManager.Instance.Subcribe(EventID.UpdateProgressLevel, this);
        EventManager.Instance.Subcribe(EventID.Revive, this);
    }

    private void OnDestroy()
    {
        PlayerSelection.OnPathFindingEvent -= FindPath;

        PathView.OnDoneShowEvent -= CheckWin;

        ScreenDetector.OnChangeScreenOrientationEvent -= SetPositionCamera;

        EventManager.Instance.Unsubcribe(EventID.UpdateProgressLevel, this);
        EventManager.Instance.Unsubcribe(EventID.Revive, this);
    }

    public void Init(LevelProfileSO levelProfileSO, int[,] graphMap, int[] idCards, float elapsedSeconds, int starsReceived, int amountMatch)
    {
        Clear();

        m_levelProfileSO = levelProfileSO;

        gameProfileSO.SetStarsReceived(starsReceived);

        int[,] alignmentMap = MapData.MakeMap(m_levelProfileSO.textAsset);
        StartCoroutine(IELoadMap(graphMap, alignmentMap, idCards));
        m_amountMatch = amountMatch;

        StartTimer();
        timer.SetElapsedSeconds(elapsedSeconds);

        SetupPreplay();
    }

    public void Init(LevelProfileSO levelProfileSO)
    {
        Clear();

        m_levelProfileSO = levelProfileSO;

        gameProfileSO.SetStarsReceived(0);

        cardData.Init(m_levelProfileSO.GetStartingCards());
        m_amountMatch = 0;

        int[,] instanceMap = MapData.MakeMap(m_levelProfileSO.textAsset);
        StartCoroutine(IELoadMap(instanceMap, instanceMap, cardData.idCards.ToArray()));

        StartTimer();

        SetupPreplay();
    }

    private IEnumerator IELoadMap(int[,] graphMap, int[,] alignmentMap, int[] idCards)
    {
        if (graph != null && cardData != null)
        {
            gameProfileSO.enablePlayerController = false;

            graph.Init(graphMap);
            m_amountCard = graph.walls.Count;
            m_amountCardEffect = m_amountCard;

            graphView.Init();
            graphView.ShowCards(graph.walls, idCards);

            if (pathfinder != null)
            {
                pathfinder.Init(graph, graphView);
            }

            if (suggester != null)
            {
                suggester.Init(graph, graphView, pathfinder);
            }

            if (shuffle != null)
            {
                shuffle.Init(graph, graphView);
            }

            if (alignment != null)
            {
                alignment.Init(alignmentMap, graph, graphView);
            }

            yield return StartCoroutine(IECheckMapConnectivityForShuffling());

            gameProfileSO.enablePlayerController = true;
        }
    }

    public void StartTimer()
    {
        timer.Setup(m_levelProfileSO.timeSystem,
            () =>
            {
                Lose();
            });

        timer.StartTimer();
    }

    public void SetupPreplay()
    {
        IsPlaying = true;
        DataManager.SetIsPlaying(true);
        DataManager.SetIsStartingPlay(false);

        combo.Init();

        gamePlayCanvas.gameObject.SetActive(true);
        gamePlayCanvas.Init();

        SetupCamera();
    }

    private void SetupCamera()
    {
        SetPositionCamera();
        SetSizeCamera();
    }

    private void SetPositionCamera()
    {
        if (!IsPlaying)
            return;

        Vector3 cameraPos = new Vector3((float)(graph.Width - 1) / 2f, (float)(graph.Height - 1) / 2f, -10);
        // Debug.Log("cameraPos: " + cameraPos);
        EnvironmentController.SetPositionCamera(cameraPos);
        Vector3 offset = Vector3.right * (EnvironmentController.mainCamera.ScreenToWorldPoint(gamePlayCanvas.playingArea.position).x - cameraPos.x);
        // Debug.Log("offset: " + offset);
        EnvironmentController.SetPositionCamera(cameraPos - offset);
    }

    private void SetSizeCamera()
    {
        float verticalSize = (float)graph.Height / 2f + Settings.BorderSizeY;
        float horizontalSize = ((float)graph.Width / 2f + Settings.BorderSizeX) / Camera.main.aspect;
        float orthographicSize = (verticalSize > horizontalSize) ? verticalSize : horizontalSize;
        EnvironmentController.SetSizeCamera(orthographicSize);
    }

    public void FindPath(GameObject go1, GameObject go2)
    {
        StartCoroutine(IEFindPath(go1, go2));
    }

    private IEnumerator IEFindPath(GameObject go1, GameObject go2)
    {
        NodeView startNV = go1.GetComponentInParent<NodeView>();
        NodeView goalNV = go2.GetComponentInParent<NodeView>();

        if (startNV != null && goalNV != null)
        {
            Node startNode = startNV.Node;
            Node goalNode = goalNV.Node;

            startNode.nodeType = NodeType.Open;
            goalNode.nodeType = NodeType.Open;

            // Tìm đường đi thỏa mãn
            pathfinder.InitSearch(startNode, goalNode);
            yield return StartCoroutine(pathfinder.IESearchCustom());
            bool find = pathfinder.find;

            if (find)
            {
                graphView.ClearCards(new List<Node> { startNode, goalNode });
                m_amountCard -= 2;
                combo.Setup();
            }
            else
            {
                startNode.nodeType = NodeType.Blocked;
                goalNode.nodeType = NodeType.Blocked;
            }

            List<Node> foundNode = new List<Node>(pathfinder.FoundNodes);
            pathfinder.pathView.ShowTask(foundNode, find).Forget();

            if (!find)
            {
                VibrationManager.Failure();
                AudioManager.PlaySFX(SFXType.Fail);
                Tween.ShakeCamera(Camera.main, 0.25f, 0.1f);
                gamePlayCanvas.Warning();
            }

            graphView.ResetNodeViews(graph);

            if (IsClearCard)
            {
                gamePlayCanvas.Interaction = false;
                yield break;
            }

            if (find)
            {
                // Sắp xếp lại map
                alignType = m_levelProfileSO.GetAlignmentType(m_amountMatch);
                m_amountMatch++;

                if (alignType != AlignmentType.None)
                {
                    alignment.SetAlign(alignType);
                    yield return new WaitForSeconds(0.3f);
                }

                // Kiểm tra tính khả thi của màn chơi
                yield return StartCoroutine(IECheckMapConnectivityForShuffling());
            }
        }
    }

    private IEnumerator IECheckMapConnectivityForShuffling()
    {
        do
        {
            yield return StartCoroutine(suggester.IEHint());

            if (!suggester.hint)
            {
                shuffle.SetupShuffle();
            }
        } while (!suggester.hint);
    }

    public void ChangeAlignDirection(int type)
    {
        alignType = (AlignmentType)type;
    }

    public void UseSupportItem(ItemsData.ItemType itemType)
    {
        switch (itemType)
        {
            case ItemsData.ItemType.Hint:
                suggester.GetHint();
                break;
            case ItemsData.ItemType.Shuffle:
                shuffle.SetupShuffle();
                break;
            case ItemsData.ItemType.Timer:
                timer.AddDuration(Settings.MoreTime);
                break;
        }
    }

    private void CheckWin()
    {
        m_amountCardEffect -= 2;

        if (IsClearCardEffect)
        {
            Win();
        }
    }

    public void Win()
    {
        timer.StopTimer();

        gameProfileSO.elapsedSeconds = timer.timeProfileSO.timerData.elapsedSeconds;
        gameProfileSO.starWin = timer.timeProfileSO.GetStar();

        gamePlayCanvas.Win();

        starCounter.RunToEnd(1f, () =>
        {
            gamePlayCanvas.Interaction = true;
            OnWinEvent?.Invoke();
            IsPlaying = false;
            DataManager.SetIsStartingPlay(true);
        });
    }

    public void Lose()
    {
        IsPlaying = false;
        DataManager.SetIsPlaying(false);
        OnLoseEvent?.Invoke();
    }

    public void Clear()
    {
        if (graphView != null)
        {
            graphView.ClearNodeView();
        }

        timer.StopTimer();
        IsPlaying = false;

        starCounter.StopAll();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            UpdateProgressLevel();
        }
    }

    private void OnApplicationQuit()
    {
        UpdateProgressLevel();
    }

    public void UpdateProgressLevel()
    {
        if (!IsPlaying)
            return;

        // Debug.Log("Save Progress");
        DataManager.UpdateProgressLevel(
            map: graph.GetMap(),
            idCards: graphView.GetIdCards().ToArray(),
            elapsedSeconds: timer.timeProfileSO.timerData.elapsedSeconds,
            starsReceived: gameProfileSO.starsReceived,
            amountMatch: m_amountMatch);
    }

    public void Revive()
    {
        if (IsPlaying)
            return;

        IsPlaying = true;
        DataManager.SetIsPlaying(true);
        timer.AddDuration(Settings.MoreTimeRevive);
        timer.StartTimer();

        gamePlayCanvas.Init();
    }

    public void EventHandler(EventID eventID)
    {
        switch (eventID)
        {
            case EventID.UpdateProgressLevel:
                UpdateProgressLevel();
                break;
            case EventID.Revive:
                Revive();
                break;
            default:
                Debug.Log("Unknown event id");
                break;
        }
    }
}
