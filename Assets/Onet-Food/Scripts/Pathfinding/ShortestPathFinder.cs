using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShortestPathFinder : MonoBehaviour
{
    private Node m_startNode;
    private Node m_goalNode;
    private Graph m_graph;
    private GraphView m_graphView;

    private PriorityQueue<Node> m_frontierNodes;
    private List<Node> m_exploredNodes;
    private List<Node> m_pathNodes;
    private List<Node> m_zigzagNodes;

    public List<Node> PathNodes => m_pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = new Color(0.85f, 0.85f, 0.85f, 1f);
    public Color highlightColor = new Color(1f, 1f, 0.5f, 1f);

    public bool showIterations = true;
    public bool showColor = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;

    public bool isComplete = false;

    private int m_iterations = 0;
    private int m_turnCount = 0;

    public enum Mode
    {
        BreadthFirstSearch = 0,
        Dijkstra = 1,
        AStar = 2
    }

    public Mode mode = Mode.BreadthFirstSearch;

    public void Init(Graph graph, GraphView graphView)
    {
        if (graph == null || graphView == null)
        {
            Debug.LogWarning("PATHFINDER Init error: missing component(s)!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
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

        isComplete = false;
        m_iterations = 0;
        m_turnCount = 0;

        m_startNode = start;
        m_goalNode = goal;

        m_startNode.distanceTraveled = 0f;

        m_frontierNodes = new PriorityQueue<Node>();
        m_frontierNodes.Enqueue(start);
        m_exploredNodes = new List<Node>();
        m_pathNodes = new List<Node>();

        if (showColor)
        {
            ShowColor(m_graphView, start, goal);
        }
    }

    public IEnumerator IESearch(float timeStep = 0.1f)
    {
        yield return null;

        while (!isComplete)
        {
            if (m_frontierNodes.Count > 0)
            {
                Node currentNode = m_frontierNodes.Dequeue();
                m_iterations++;

                if (!m_exploredNodes.Contains(currentNode))
                {
                    m_exploredNodes.Add(currentNode);
                }

                if (mode == Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if (mode == Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }
                else if (mode == Mode.AStar)
                {
                    ExpandFrontierAStar(currentNode);
                }

                if (m_frontierNodes.Contains(m_goalNode))
                {
                    m_pathNodes = GetPathNodes(m_goalNode);
                    m_zigzagNodes = GetZigZagNodes(m_pathNodes);

                    if (exitOnGoal)
                    {
                        isComplete = true;
                        Debug.Log("PATHFINDER mode: " + mode.ToString() + "     path length = " + m_goalNode.distanceTraveled.ToString());
                    }
                }

                if (showIterations)
                {
                    ShowDiagnostics();

                    yield return new WaitForSeconds(timeStep);
                }
            }
            else
            {
                isComplete = true;
            }
        }

        ShowDiagnostics();
        // if (m_pathNodes.Contains(m_goalNode))
        // {
        //     ShowPaths();
        // }
    }

    private void ExpandFrontierBreadthFirst(Node node)
    {
        if (node == null)
            return;

        for (int i = 0; i < node.neighbors.Count; i++)
        {
            if (node.neighbors[i].nodeType != NodeType.Blocked &&
                !m_exploredNodes.Contains(node.neighbors[i]) &&
                !m_frontierNodes.Contains(node.neighbors[i]))
            {
                float distanceToNeighbor = m_graph.GetNodeDistanceNeighbor(node, node.neighbors[i]);
                float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                node.neighbors[i].distanceTraveled = newDistanceTraveled;
                node.neighbors[i].previous = node;
                node.neighbors[i].priority = m_frontierNodes.Count;

                m_frontierNodes.Enqueue(node.neighbors[i]);
            }
        }
    }

    private void ExpandFrontierDijkstra(Node node)
    {
        if (node == null)
            return;

        List<Node> neighbors = new List<Node>();

        int dx = node.xIndex - m_goalNode.xIndex;
        int dy = node.yIndex - m_goalNode.yIndex;

        if (Mathf.Abs(dx) > Mathf.Abs(dy) && dx != 0)
        {
            if (dx < 0)
            {
                if (node.neighbors.Count > 1) neighbors.Add(node.neighbors[1]);
                if (node.neighbors.Count > 3) neighbors.Add(node.neighbors[3]);
            }
            else
            {
                if (node.neighbors.Count > 3) neighbors.Add(node.neighbors[3]);
                if (node.neighbors.Count > 1) neighbors.Add(node.neighbors[1]);
            }
        }
        else if (Mathf.Abs(dx) < Mathf.Abs(dy) && dy != 0)
        {
            if (dy < 0)
            {
                if (node.neighbors.Count > 0) neighbors.Add(node.neighbors[0]);
                if (node.neighbors.Count > 2) neighbors.Add(node.neighbors[2]);
            }
            else
            {
                if (node.neighbors.Count > 2) neighbors.Add(node.neighbors[2]);
                if (node.neighbors.Count > 0) neighbors.Add(node.neighbors[0]);
            }
        }

        for (int i = 0; i < node.neighbors.Count; i++)
        {
            if (!neighbors.Contains(node.neighbors[i]))
            {
                neighbors.Add(node.neighbors[i]);
            }
        }

        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].nodeType != NodeType.Blocked &&
                !m_exploredNodes.Contains(neighbors[i]))
            {
                float distanceToNeighbor = m_graph.GetNodeDistanceNeighbor(node, neighbors[i]);
                float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                if (float.IsPositiveInfinity(neighbors[i].distanceTraveled) ||
                    newDistanceTraveled < neighbors[i].distanceTraveled)
                {
                    neighbors[i].previous = node;
                    neighbors[i].distanceTraveled = newDistanceTraveled;
                }
                else if (newDistanceTraveled == neighbors[i].distanceTraveled)
                {
                    neighbors[i].secondPrevious = node;
                }

                if (!m_frontierNodes.Contains(neighbors[i]))
                {
                    neighbors[i].priority = neighbors[i].distanceTraveled;
                    m_frontierNodes.Enqueue(neighbors[i]);
                }
            }
        }
    }

    private void ExpandFrontierAStar(Node node)
    {
        if (node == null)
            return;

        List<Node> neighbors = new List<Node>(node.neighbors);

        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].nodeType != NodeType.Blocked &&
                !m_exploredNodes.Contains(neighbors[i]))
            {
                float distanceToNeighbor = m_graph.GetNodeDistanceNeighbor(node, neighbors[i]);
                float newDistanceTraveled = distanceToNeighbor + node.distanceTraveled;

                if (float.IsPositiveInfinity(neighbors[i].distanceTraveled) ||
                    newDistanceTraveled < neighbors[i].distanceTraveled)
                {
                    neighbors[i].previous = node;
                    neighbors[i].distanceTraveled = newDistanceTraveled;
                }
                else if (newDistanceTraveled == neighbors[i].distanceTraveled)
                {
                    neighbors[i].secondPrevious = node;
                }

                if (!m_frontierNodes.Contains(neighbors[i]))
                {
                    float disToGoal = m_graph.GetNodeDistance(neighbors[i], m_goalNode);
                    bool isTurn = m_graph.IsTurn(node, neighbors[i]);
                    float priorityTurn = 0;
                    if (isTurn)
                    {
                        m_turnCount++;
                        priorityTurn = 100 * m_turnCount;
                    }
                    node.neighbors[i].priority = neighbors[i].distanceTraveled + disToGoal + priorityTurn;
                    m_frontierNodes.Enqueue(neighbors[i]);
                }
            }
        }
    }

    private List<Node> GetPathNodes(Node endNode)
    {
        List<Node> path = new List<Node>();

        if (endNode == null)
        {
            return path;
        }
        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode != null)
        {
            path.Add(currentNode);
            currentNode = currentNode.previous;
        }

        path.Reverse();

        return path;
    }

    private void ShowDiagnostics()
    {
        if (showColor)
        {
            ShowColor();
        }

        if (m_graphView != null && showArrows)
        {
            m_graphView.ShowNodeArrows(m_frontierNodes.ToList(), arrowColor);

            if (m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodeArrows(m_pathNodes, highlightColor);
            }
        }
    }

    private void ShowPaths()
    {
        Color lineColor = (m_zigzagNodes.Count < Settings.ZigzagIterations) ? Color.yellow : Color.red;

        if (m_graphView != null)
        {
            if (m_frontierNodes.Contains(m_goalNode))
            {
                m_graphView.ShowNodePaths(m_pathNodes, lineColor);
            }
        }
    }

    private List<Node> GetZigZagNodes(List<Node> pathNodes)
    {
        List<Node> zigzagNodes = new List<Node>();

        if (pathNodes.Count >= 2)
        {
            Direction preDirection = GetPathDirection(pathNodes[0], pathNodes[1]);

            for (int i = 1; i < pathNodes.Count - 1; i++)
            {
                Direction dir = GetPathDirection(pathNodes[i], pathNodes[i + 1]);

                if (dir != preDirection)
                {
                    zigzagNodes.Add(pathNodes[i]);
                }

                preDirection = dir;
            }
        }

        return zigzagNodes;
    }

    private Direction GetPathDirection(Node source, Node target)
    {
        if (source == null || target == null)
        {
            return Direction.None;
        }

        return source.GetPathDiretion(target);
    }

    public bool IsMatch()
    {
        return m_pathNodes.Count > 0 &&
            m_zigzagNodes.Count < Settings.ZigzagIterations;
    }

    private void ShowColor()
    {
        ShowColor(m_graphView, m_startNode, m_goalNode);
    }

    private void ShowColor(GraphView graphView, Node start, Node goal)
    {
        if (graphView == null || start == null || goal == null)
        {
            return;
        }

        if (m_frontierNodes != null)
        {
            graphView.ColorNodes(m_frontierNodes.ToList(), frontierColor);
        }

        if (m_exploredNodes != null)
        {
            graphView.ColorNodes(m_exploredNodes.ToList(), exploredColor);
        }

        if (m_pathNodes != null && m_pathNodes.Count > 0)
        {
            graphView.ColorNodes(m_pathNodes, pathColor);
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

}
