using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))]
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews = new NodeView[0, 0];

    public Color baseColor = Color.white;
    public Color wallColor = Color.black;

    public void Init(Graph graph)
    {
        if (graph == null)
        {
            Debug.LogWarning("GRAPHVIEW No graph to initialize!");
            return;
        }

        ClearNodeView();

        nodeViews = new NodeView[graph.Width, graph.Height];

        foreach (Node n in graph.nodes)
        {
            GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity, transform);

            if (instance.TryGetComponent<NodeView>(out NodeView nodeView))
            {
                nodeView.Init(n);
                nodeViews[n.xIndex, n.yIndex] = nodeView;
                nodeView.ColorNode(n.nodeType == NodeType.Blocked ? wallColor : baseColor);
            }
        }
    }

    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];

                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            ShowNodeArrow(n, color);
        }
    }

    public void ShowNodeArrow(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                nodeView.ShowArrow(color);
                nodeView.ShowArrow2(color);
            }
        }
    }

    public void ShowNodePaths(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            ShowNodePath(n, color);
        }
    }

    public void ShowNodePath(Node node, Color color)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                nodeView.ShowPath(color);
            }
        }
    }

    public void ShowCards(List<Node> nodes, List<int> idCards)
    {
        int count = (nodes.Count <= idCards.Count) ? nodes.Count : idCards.Count;

        for (int i = 0; i < count; i++)
        {
            ShowCard(nodes[i], idCards[i]);
        }
    }

    public void ShowCard(Node node, int idCard, bool effect = true)
    {
        if (node.nodeType == NodeType.Open)
        {
            Debug.LogWarning("PATHFINDER Init error: node must be blocked!");
            return;
        }

        NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

        if (nodeView != null)
        {
            nodeView.ShowCard(idCard, effect);
        }
    }

    public void MoveCard(Node node, Vector3 start)
    {
        if (node.nodeType == NodeType.Open)
        {
            Debug.LogWarning("PATHFINDER Init error: node must be blocked!");
            return;
        }

        NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

        if (nodeView != null)
        {
            nodeView.MoveCard(start);
        }
    }

    public void ResetNodeViews(Graph graph)
    {
        foreach (Node n in graph.nodes)
        {
            NodeView nodeView = nodeViews[n.xIndex, n.yIndex];

            if (nodeView != null)
            {
                nodeView.ColorNode(n.nodeType == NodeType.Blocked ? wallColor : baseColor);
                nodeView.Reset();
            }
        }
    }

    public void ResetNodeView(Node node)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                nodeView.ColorNode(node.nodeType == NodeType.Blocked ? wallColor : baseColor);
                nodeView.Reset();
            }
        }
    }

    public void ClearCards(List<Node> nodes)
    {
        foreach (Node n in nodes)
        {
            ClearCard(n);
        }
    }

    public void ClearCard(Node node)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                nodeView.ClearCard();
            }
        }
    }

    public int GetIDCard(Node node)
    {
        if (node != null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];

            if (nodeView != null)
            {
                return nodeView.GetIDCard();
            }
        }

        Debug.LogWarning("NODEVIEW GetCard missing card");

        return -1;
    }

    public List<NodeView> GetNodeViews(List<Node> nodes)
    {
        List<NodeView> nodeViews = new List<NodeView>();

        for (int i = 0; i < nodes.Count; i++)
        {
            NodeView nodeView = this.nodeViews[nodes[i].xIndex, nodes[i].yIndex];
            if (nodeView != null)
            {
                nodeViews.Add(nodeView);
            }
        }

        return nodeViews;
    }

    public void ClearNodeView()
    {
        if (nodeViews.GetLength(0) > 0 || nodeViews.GetLength(1) > 0)
        {
            foreach (NodeView nv in nodeViews)
            {
                Destroy(nv.gameObject);
            }
        }

        nodeViews = new NodeView[0, 0];
    }
}
