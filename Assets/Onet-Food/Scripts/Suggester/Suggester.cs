using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suggester : MonoBehaviour
{
    private Graph m_graph;
    private GraphView m_graphView;
    private Pathfinder m_pathfinder;

    private List<Node> m_closedNodes;
    private List<Node> m_accessibleNodes;
    private List<NodeView> m_accessibleNodeViews;

    private List<SimilarNodeView> m_similarNodeViews;

    private Node m_firstNode;
    private Node m_secondNode;

    public bool hint = false;

    [Serializable]
    public class SimilarNodeView
    {
        public int idCard;
        public List<NodeView> nodeViews;
    }

    public void Init(Graph graph, GraphView graphView, Pathfinder pathfinder)
    {
        if (graph == null || graphView == null || pathfinder == null)
        {
            Debug.LogWarning("SUGGESTER Init error: missing component(s)!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
        m_pathfinder = pathfinder;
    }

    public void InitNodes()
    {
        m_closedNodes = new List<Node>();
        m_accessibleNodes = new List<Node>();

        for (int y = 0; y < m_graph.Height; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                Node node = m_graph.nodes[x, y];
                if (node.nodeType == NodeType.Blocked)
                {
                    if (node.IsNeighborBlocked())
                    {
                        m_closedNodes.Add(node);
                    }
                    else
                    {
                        m_accessibleNodes.Add(node);
                    }
                }
            }
        }
    }

    public void GetHint()
    {
        if (m_firstNode != null && m_secondNode != null &&
            m_firstNode.nodeType == NodeType.Blocked && m_secondNode.nodeType == NodeType.Blocked)
        {
            NodeView firstNodeView = m_graphView.nodeViews[m_firstNode.xIndex, m_firstNode.yIndex];
            NodeView secondNodeView = m_graphView.nodeViews[m_secondNode.xIndex, m_secondNode.yIndex];

            if (firstNodeView.GetIDCard() == secondNodeView.GetIDCard())
            {
                ShowHighlightHint();
                return;
            }
        }

        StartCoroutine(IEHint(true));
    }

    public IEnumerator IEHint(bool showHighlight = false)
    {
        InitNodes();

        hint = GetHintClosedNodes();

        if (!hint)
        {
            yield return StartCoroutine(IEHintAccessibleNodes());
        }

        m_pathfinder.ResetNodes();

        if (hint && showHighlight)
        {
            ShowHighlightHint();
        }
    }

    public bool GetHintClosedNodes()
    {
        for (int i = 0; i < m_closedNodes.Count; i++)
        {
            if (GetHintClosedNode(m_closedNodes[i]))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetHintClosedNode(Node node)
    {
        int idCard = m_graphView.GetIDCard(node);
        int idCardNeighbor;

        for (int i = 0; i < node.neighbors.Count; i++)
        {
            idCardNeighbor = m_graphView.GetIDCard(node.neighbors[i]);
            if (idCardNeighbor == idCard)
            {
                SetNodeHint(node, node.neighbors[i]);
                return true;
            }
        }

        return false;
    }

    public IEnumerator IEHintAccessibleNodes()
    {
        m_accessibleNodeViews = m_graphView.GetNodeViews(m_accessibleNodes);

        List<int> uniqueIDCards = GetUniqueIDCards(m_accessibleNodeViews);

        m_similarNodeViews = GetSimilarNodeViews(uniqueIDCards, m_accessibleNodeViews);

        for (int i = 0; i < m_similarNodeViews.Count; i++)
        {
            SimilarNodeView similarNodeView = m_similarNodeViews[i];

            GetHintSimilarNodeView(similarNodeView);

            if (hint)
            {
                yield break;
            }
            yield return null;
        }
    }

    public List<int> GetUniqueIDCards(List<NodeView> nodeViews)
    {
        List<int> idCards = new List<int>();

        int idCard;

        for (int i = 0; i < nodeViews.Count; i++)
        {
            idCard = nodeViews[i].GetIDCard();

            if (!idCards.Contains(idCard))
            {
                idCards.Add(idCard);
            }
        }

        return idCards;
    }

    public List<SimilarNodeView> GetSimilarNodeViews(List<int> uniqueIDCards, List<NodeView> nodeViews)
    {
        List<SimilarNodeView> similarNodeViews = new List<SimilarNodeView>();

        for (int i = 0; i < uniqueIDCards.Count; i++)
        {
            SimilarNodeView similarNodeView = new SimilarNodeView
            {
                idCard = uniqueIDCards[i],
                nodeViews = nodeViews.FindAll(nv => nv.GetIDCard() == uniqueIDCards[i])
            };

            similarNodeViews.Add(similarNodeView);
        }

        return similarNodeViews;
    }

    public void GetHintSimilarNodeView(SimilarNodeView similarNodeView)
    {
        int count = similarNodeView.nodeViews.Count;
        Node firstNode;
        Node secondNode;

        for (int i = 0; i < count - 1; i++)
        {
            firstNode = similarNodeView.nodeViews[i].Node;

            for (int j = i + 1; j < count; j++)
            {
                secondNode = similarNodeView.nodeViews[j].Node;

                firstNode.nodeType = NodeType.Open;
                secondNode.nodeType = NodeType.Open;

                m_pathfinder.InitSearch(firstNode, secondNode);
                bool find = m_pathfinder.Search();

                firstNode.nodeType = NodeType.Blocked;
                secondNode.nodeType = NodeType.Blocked;

                if (find)
                {
                    hint = true;
                    SetNodeHint(firstNode, secondNode);

                    return;
                }
            }
        }
    }

    public void SetNodeHint(Node firstNode, Node secondNode)
    {
        m_firstNode = firstNode;
        m_secondNode = secondNode;
    }

    public void ShowHighlightHint()
    {
        if (hint)
        {
            NodeView firstNodeView = m_graphView.nodeViews[m_firstNode.xIndex, m_firstNode.yIndex];
            NodeView secondNodeView = m_graphView.nodeViews[m_secondNode.xIndex, m_secondNode.yIndex];

            firstNodeView.ShowHighlightCard(true);
            secondNodeView.ShowHighlightCard(true);
        }
    }
}
