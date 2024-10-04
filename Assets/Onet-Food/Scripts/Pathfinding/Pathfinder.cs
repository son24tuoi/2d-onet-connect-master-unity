using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] private ShortestPathFinder m_shortestPathFinder;
    public PathView pathView;

    private Node m_startNode;
    private Node m_goalNode;
    private Graph m_graph;
    private GraphView m_graphView;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;

    public bool showColor = true;

    private List<Node> m_foundNodes = new List<Node>();
    public bool find = false;

    private List<ZiczacNodes> m_ziczacNodes = new List<ZiczacNodes>();

    public List<Node> FoundNodes
    {
        get => m_foundNodes;
    }

    [Serializable]
    public class ZiczacNodes
    {
        public bool find = false;
        public List<Node> nodes = new List<Node>();
    }

    public void Init(Graph graph, GraphView graphView)
    {
        if (graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;

        m_shortestPathFinder.Init(graph, graphView);
        pathView.amountStar = 0;
    }

    public void ResetNodes()
    {
        for (int x = 0; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                m_graph.nodes[x, y].Reset();
            }
        }
    }

    public void InitSearch(Node start, Node goal)
    {
        if (start == null || goal == null)
        {
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return;
        }

        if (start.nodeType == NodeType.Blocked || goal.nodeType == NodeType.Blocked)
        {
            Debug.LogWarning("PATHFINDER Init error: start and goal nodes must be unblocked!");
            return;
        }

        ResetNodes();

        m_startNode = start;
        m_goalNode = goal;

        if (showColor)
        {
            ShowColor(m_graphView, start, goal);
        }
    }

    private void ShowColor(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];

        if (startNodeView != null)
        {
            startNodeView.ColorNode(startColor);
        }

        NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (goalNodeView != null)
        {
            goalNodeView.ColorNode(goalColor);
        }
    }

    public void ShowNodePaths()
    {
        if (m_foundNodes.Contains(m_goalNode))
        {
            m_graphView.ShowNodePaths(m_foundNodes, find ? Color.yellow : Color.red);
        }
    }

    public void ShowNodePaths(List<Node> foundNodes, Node goalNode, bool find)
    {
        if (foundNodes.Contains(goalNode))
        {
            m_graphView.ShowNodePaths(foundNodes, find ? Color.yellow : Color.red);
        }
    }

    public IEnumerator IESearchCustom(float timeStep = 0.1f)
    {
        float timeStart = Time.time;
        yield return null;

        m_foundNodes.Clear();
        ResetNodes();
        find = Search();

        if (find)
        {
            m_foundNodes.Insert(0, m_startNode);
            for (int i = m_foundNodes.Count - 1; i > 0; i--)
            {
                m_foundNodes[i].previous = m_foundNodes[i - 1];
            }
        }
        else
        {
            m_shortestPathFinder.InitSearch(m_startNode, m_goalNode);
            yield return StartCoroutine(m_shortestPathFinder.IESearch(timeStep));

            m_foundNodes = m_shortestPathFinder.PathNodes;
            find = m_shortestPathFinder.IsMatch();
        }

        // Debug.Log("PATHFINDER IESearch: elapse time = " + (Time.time - timeStart).ToString() + " seconds");
    }

    public bool Search()
    {
        bool find = NoTurn();
        if (find) return true;

        find = OneTurn();
        if (find) return true;

        find = TwoTurn();
        if (find) return true;

        return false;
    }

    private bool NoTurn()
    {
        if (m_startNode.xIndex == m_goalNode.xIndex) // Same x, Check y
        {
            m_foundNodes = m_graph.FindNodesAlongVertical(m_startNode, m_goalNode);

            return m_graph.IsNodesFree(m_foundNodes);
        }
        else if (m_startNode.yIndex == m_goalNode.yIndex) // Same y, Check x
        {
            m_foundNodes = m_graph.FindNodesAlongHorizontal(m_startNode, m_goalNode);

            return m_graph.IsNodesFree(m_foundNodes);
        }
        else
        {
            return false;
        }
    }

    private bool OneTurn()
    {
        int dx = m_goalNode.xIndex - m_startNode.xIndex;
        int dy = m_goalNode.yIndex - m_startNode.yIndex;

        int dxA = Mathf.Abs(dx);
        int dyA = Mathf.Abs(dy);

        if (dx > 0 && dy > 0) // Quadrant I
        {
            return (dxA >= dyA) ?
                    OneTurnQuadrant(Direction.Right, Direction.Up) :
                    OneTurnQuadrant(Direction.Up, Direction.Right);
        }
        else if (dx < 0 && dy > 0) // Quadrant II
        {
            return (dxA >= dyA) ?
                    OneTurnQuadrant(Direction.Left, Direction.Up) :
                    OneTurnQuadrant(Direction.Up, Direction.Left);
        }
        else if (dx < 0 && dy < 0) // Quadrant III
        {
            return (dxA >= dyA) ?
                    OneTurnQuadrant(Direction.Left, Direction.Down) :
                    OneTurnQuadrant(Direction.Down, Direction.Left);
        }
        else if (dx > 0 && dy < 0) // Quadrant IV
        {
            return (dxA >= dyA) ?
                    OneTurnQuadrant(Direction.Right, Direction.Down) :
                    OneTurnQuadrant(Direction.Down, Direction.Right);
        }
        else
        {
            return false;
        }
    }

    private bool TwoTurn()
    {
        m_foundNodes.Clear();
        m_ziczacNodes.Clear();

        m_ziczacNodes.Add(ZiczacLine(Direction.Right, Direction.Up, Direction.Left));
        m_ziczacNodes.Add(ZiczacLine(Direction.Right, Direction.Up, Direction.Right));
        m_ziczacNodes.Add(ZiczacLine(Direction.Left, Direction.Up, Direction.Right));
        m_ziczacNodes.Add(ZiczacLine(Direction.Left, Direction.Up, Direction.Left));
        m_ziczacNodes.Add(ZiczacLine(Direction.Right, Direction.Down, Direction.Left));
        m_ziczacNodes.Add(ZiczacLine(Direction.Right, Direction.Down, Direction.Right));
        m_ziczacNodes.Add(ZiczacLine(Direction.Left, Direction.Down, Direction.Right));
        m_ziczacNodes.Add(ZiczacLine(Direction.Left, Direction.Down, Direction.Left));
        m_ziczacNodes.Add(ZiczacLine(Direction.Down, Direction.Right, Direction.Up));
        m_ziczacNodes.Add(ZiczacLine(Direction.Down, Direction.Right, Direction.Down));
        m_ziczacNodes.Add(ZiczacLine(Direction.Down, Direction.Left, Direction.Down));
        m_ziczacNodes.Add(ZiczacLine(Direction.Down, Direction.Left, Direction.Up));
        m_ziczacNodes.Add(ZiczacLine(Direction.Up, Direction.Left, Direction.Up));
        m_ziczacNodes.Add(ZiczacLine(Direction.Up, Direction.Left, Direction.Down));
        m_ziczacNodes.Add(ZiczacLine(Direction.Up, Direction.Right, Direction.Down));
        m_ziczacNodes.Add(ZiczacLine(Direction.Up, Direction.Right, Direction.Up));

        List<ZiczacNodes> findZiczacNodes = m_ziczacNodes.FindAll(e => e.find == true);

        if (findZiczacNodes.Count == 0)
        {
            return false;
        }

        int minIndex = 0;

        for (int i = 1; i < findZiczacNodes.Count; i++)
        {
            if (findZiczacNodes[minIndex].nodes.Count > findZiczacNodes[i].nodes.Count)
            {
                minIndex = i;
            }
        }

        m_foundNodes = findZiczacNodes[minIndex].nodes;

        return true;
    }

    private bool PerpendicularLine(Direction firstDir, Direction secDir)
    {
        if (firstDir == Direction.None || secDir == Direction.None)
        {
            Debug.LogWarning("PATHFINDER PerpendicularLine direction must not be None");
            return false;
        }

        Direction dirToGoal = secDir switch
        {
            Direction.Up => Direction.Down,
            Direction.Right => Direction.Left,
            Direction.Down => Direction.Up,
            Direction.Left => Direction.Right,
            _ => throw new System.NotImplementedException(),
        };

        if (m_graph.IsNeighborFree(m_startNode, firstDir) &&
            m_graph.IsNeighborFree(m_goalNode, dirToGoal))
        {
            m_foundNodes = m_graph.FindNodesAlongPerpendicularLine
                                (m_startNode, m_goalNode, firstDir, secDir);

            if (m_graph.IsNodesFree(m_foundNodes))
            {
                return true;
            }
        }

        return false;
    }

    private bool OneTurnQuadrant(Direction firstDir, Direction secDir)
    {
        if (PerpendicularLine(firstDir, secDir)) // Finding: fisrtDir -> secDir
        {
            return true;
        }

        if (PerpendicularLine(secDir, firstDir)) // Finding: secDir -> firstDir
        {
            return true;
        }

        return false;
    }

    private ZiczacNodes ZiczacLine(Direction firstDir, Direction secondDir, Direction thirdDir)
    {
        ZiczacNodes ziczacNodes = new ZiczacNodes();

        if (firstDir == Direction.None || secondDir == Direction.None || thirdDir == Direction.None)
        {
            Debug.LogWarning("PATHFINDER PerpendicularLine direction must not be None");
            return ziczacNodes;
        }

        List<Node> firstNodes = m_graph.FindNodesAlongStraight(m_startNode, firstDir);
        List<Node> nodeInPerpendicularLine = new List<Node>();

        for (int i = 0; i < firstNodes.Count; i++)
        {
            if (m_graph.IsNodeFree(firstNodes[i]))
            {
                nodeInPerpendicularLine = m_graph.FindNodesAlongPerpendicularLine
                                            (firstNodes[i], m_goalNode, secondDir, thirdDir);

                if (m_graph.IsNodesFree(nodeInPerpendicularLine))
                {
                    List<Node> findNodes = new List<Node>();
                    findNodes.AddRange(firstNodes.GetRange(0, i + 1));
                    findNodes.AddRange(nodeInPerpendicularLine);

                    ziczacNodes = new ZiczacNodes
                    {
                        find = true,
                        nodes = findNodes
                    };
                    return ziczacNodes;
                }
            }
            else
            {
                break;
            }
        }

        return ziczacNodes;
    }
}
