using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;

public class LevelController : MyMonoBehaviour
{
    public static event Action OnWinEvent;
    public static event Action OnLoseEvent;

    [Header("Element")]
    public CardData cardData;
    public Graph graph;
    private GraphView m_graphView;

    public Pathfinder pathfinder;
    public Suggester suggester;
    public Shuffle shuffle;
    public Align align;

    public Timer timer;

    public StarCounter starCounter;

    public Combo combo;

    [Header("UI")]
    public GamePlayCanvas gamePlayCanvas;

    [Header("Config")]
    public GameProfileSO gameProfileSO;
    public Align.AlignType alignType = Align.AlignType.None;

    private LevelProfileSO m_levelProfileSO;
    private int m_amountCard;
    private int m_amountCardEffect;

    private bool m_isPlaying;

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
    }

    private void OnDestroy()
    {
        PlayerSelection.OnPathFindingEvent -= FindPath;

        PathView.OnDoneShowEvent -= CheckWin;

        ScreenDetector.OnChangeScreenOrientationEvent -= SetPositionCamera;
    }

    public void Init(LevelProfileSO levelProfileSO)
    {
        Clear();

        m_levelProfileSO = levelProfileSO;

        gameProfileSO.SetStarsReceived(0);
        StartCoroutine(IELoadMap());

        timer.Setup(m_levelProfileSO.timeSystem,
            () =>
            {
                Lose();
            });
        timer.StartTimer();
        m_isPlaying = true;

        combo.Init();

        gamePlayCanvas.gameObject.SetActive(true);
        gamePlayCanvas.Init();

        SetupCamera();

        FirebaseManager.firebaseAnalytics.EventLevelStart(gameProfileSO.currentLevelIndex);
    }

    public IEnumerator IELoadMap()
    {
        if (graph != null && cardData != null)
        {
            gameProfileSO.enablePlayerController = false;

            int[,] mapInstance = MapData.MakeMap(m_levelProfileSO.textAsset);
            graph.Init(mapInstance);
            m_amountCard = graph.walls.Count;
            m_amountCardEffect = m_amountCard;
            cardData.Init(m_levelProfileSO.GetStartingCards());

            if (graph.TryGetComponent<GraphView>(out GraphView graphView))
            {
                m_graphView = graphView;
                graphView.Init(graph);
                graphView.ShowCards(graph.walls, cardData.idCards);
            }

            if (pathfinder != null)
            {
                pathfinder.Init(graph, graphView);
            }

            if (suggester != null)
            {
                suggester.Init(graph, m_graphView, pathfinder);
            }

            if (shuffle != null)
            {
                shuffle.Init(graph, graphView);
            }

            if (align != null)
            {
                align.Init(mapInstance, graph, graphView);
            }

            alignType = m_levelProfileSO.alignType;

            yield return StartCoroutine(IECheckMapConnectivityForShuffling());

            gameProfileSO.enablePlayerController = true;
        }
    }

    private void SetupCamera()
    {
        SetPositionCamera();
        SetSizeCamera();
    }

    private void SetPositionCamera()
    {
        if (!m_isPlaying)
            return;

        Vector3 cameraPos = new Vector3((float)(graph.Width - 1) / 2f, (float)(graph.Height - 1) / 2f, -10);
        Debug.Log("cameraPos: " + cameraPos);
        EnvironmentController.SetPositionCamera(cameraPos);
        Vector3 offset = Vector3.right * (EnvironmentController.mainCamera.ScreenToWorldPoint(gamePlayCanvas.playingArea.position).x - cameraPos.x);
        Debug.Log("offset: " + offset);
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

            pathfinder.InitSearch(startNode, goalNode);
            yield return StartCoroutine(pathfinder.IESearchCustom());
            bool find = pathfinder.find;

            if (find)
            {
                m_graphView.ClearCards(new List<Node> { startNode, goalNode });
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
            }

            m_graphView.ResetNodeViews(graph);

            if (IsClearCard)
            {
                gamePlayCanvas.Interaction = false;
                yield break;
            }

            if (alignType != Align.AlignType.None)
            {
                align.SetAlign(alignType);
                yield return new WaitForSeconds(0.3f);
            }

            yield return StartCoroutine(IECheckMapConnectivityForShuffling());
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
        alignType = (Align.AlignType)type;
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
            m_isPlaying = false;
        });

        FirebaseManager.firebaseAnalytics.EventLevelEnd(gameProfileSO.currentLevelIndex, gameProfileSO.elapsedSeconds);
    }

    public void Lose()
    {
        m_isPlaying = false;
        OnLoseEvent?.Invoke();
    }

    public void Clear()
    {
        if (m_graphView != null)
        {
            m_graphView.ClearNodeView();
        }

        timer.StopTimer();
        m_isPlaying = false;

        starCounter.StopAll();
    }
}
