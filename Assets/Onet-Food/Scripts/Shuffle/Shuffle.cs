using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    private Graph m_graph;
    private GraphView m_graphView;

    private List<Node> m_blockedNodes;
    private List<NodeView> m_blockedNodeViews;

    private List<NodeView> m_firstHalf;
    private List<NodeView> m_secondHalf;

    public void Init(Graph graph, GraphView graphView)
    {
        if (graph == null || graphView == null)
        {
            Debug.LogWarning("SHUFFLE Init error: missing component(s)!");
            return;
        }

        m_graph = graph;
        m_graphView = graphView;
    }

    public void SetupShuffle()
    {
        InitNodes();

        ShuffleNodeViews(m_blockedNodeViews);
    }

    private void InitNodes()
    {
        m_blockedNodes = new List<Node>();
        m_blockedNodeViews = new List<NodeView>();

        for (int y = 0; y < m_graph.Height; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                Node node = m_graph.nodes[x, y];

                if (node.nodeType == NodeType.Blocked)
                {
                    m_blockedNodes.Add(node);

                    NodeView nodeView = m_graphView.nodeViews[node.xIndex, node.yIndex];

                    if (nodeView != null)
                    {
                        m_blockedNodeViews.Add(nodeView);
                    }
                }
            }
        }
    }

    private void ShuffleNodeViews(List<NodeView> nodeViews)
    {
        List<NodeView> shuffleNodeViews = new List<NodeView>(nodeViews);

        if (nodeViews.Count == 4)
        {
            Shuffle4NodeViews(shuffleNodeViews);
            return;
        }

        ListShuffler.Shuffle<NodeView>(shuffleNodeViews);

        int count = shuffleNodeViews.Count / 2;

        m_firstHalf = shuffleNodeViews.GetRange(0, count);
        m_secondHalf = shuffleNodeViews.GetRange(count, count);

        for (int i = 0; i < count; i++)
        {
            int idCard = m_firstHalf[i].GetIDCard();
            m_firstHalf[i].ChangeCard(m_secondHalf[i].GetIDCard());
            m_secondHalf[i].ChangeCard(idCard);
        }
    }

    private void Shuffle4NodeViews(List<NodeView> nodeViews)
    {
        if (nodeViews.Count < 4)
        {
            Debug.LogWarning("SHUFFLE Shuffle4NodeView count must be 4");
            return;
        }

        int idCard = nodeViews[1].GetIDCard();
        nodeViews[1].ChangeCard(nodeViews[3].GetIDCard());
        nodeViews[3].ChangeCard(idCard);
    }
}
