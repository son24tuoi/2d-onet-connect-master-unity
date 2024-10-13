using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alignment : MonoBehaviour
{
    private int[,] m_mapData;
    private Graph m_graph;
    private GraphView m_graphView;

    public void Init(int[,] mapData, Graph graph, GraphView graphView)
    {
        m_mapData = mapData;
        m_graph = graph;
        m_graphView = graphView;
    }

    public void SetAlign(AlignmentType type)
    {
        switch (type)
        {
            case AlignmentType.Up:
                AlignUp();
                break;
            case AlignmentType.Right:
                AlignRight();
                break;
            case AlignmentType.Down:
                AlignDown(); ;
                break;
            case AlignmentType.Left:
                AlignLeft();
                break;
            case AlignmentType.LeftAndRight:
                AlignLeftAndRight();
                break;
            case AlignmentType.UpAndDown:
                AlignUpAndDown();
                break;
            case AlignmentType.CenterHorizontally:
                AlignCenterHorizontally();
                break;
            case AlignmentType.CenterVertically:
                AlignCenterVertically();
                break;
            default:
                break;
        }
    }

    private Node GetTargetNode(Node source, Direction direction)
    {
        return direction switch
        {
            Direction.Up => GetTargetNode(source, Vector2.up, m_graph.Height - 1 - source.yIndex),
            Direction.Right => GetTargetNode(source, Vector2.right, m_graph.Width - 1 - source.xIndex),
            Direction.Down => GetTargetNode(source, Vector2.down, source.yIndex),
            Direction.Left => GetTargetNode(source, Vector2.left, source.xIndex),
            _ => source,
        };
    }

    private Node GetVerticallyCenterTargetNode(Node source)
    {
        int halfWidth = m_graph.Width / 2;

        if (source.xIndex < halfWidth - 1)
        {
            return GetTargetNode(source, Vector2.right, halfWidth - 1 - source.xIndex);
        }
        else if (source.xIndex > halfWidth)
        {
            return GetTargetNode(source, Vector2.left, source.xIndex - halfWidth);
        }
        else
        {
            return source;
        }
    }

    private Node GetHorizontallyCenterTargetNode(Node source)
    {
        int halfHeight = m_graph.Height / 2;

        if (source.yIndex < halfHeight - 1)
        {
            return GetTargetNode(source, Vector2.up, halfHeight - 1 - source.yIndex);
        }
        else if (source.yIndex > halfHeight)
        {
            return GetTargetNode(source, Vector2.down, source.yIndex - halfHeight);
        }
        else
        {
            return source;
        }
    }

    private Node GetTargetNode(Node source, Vector2 searchDirection, int length)
    {
        int nextX = source.xIndex;
        int nextY = source.yIndex;
        Node nextNode = source;
        Node targetNode = source;

        for (int i = 1; i <= length; i++)
        {
            nextX = source.xIndex + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = source.yIndex + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!m_graph.IsWithinBounds(nextX, nextY))
            {
                break;
            }

            nextNode = m_graph.nodes[nextX, nextY];

            if (nextNode == null ||
                nextNode.nodeType == NodeType.Blocked ||
                m_mapData[nextNode.xIndex, nextNode.yIndex] == 0)
            {
                break;
            }

            targetNode = nextNode;
        }

        return targetNode;
    }

    private void AlignUp()
    {
        for (int y = m_graph.Height - 1; y >= 0; y--)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Up);
            }
        }
    }

    private void AlignDown()
    {
        for (int y = 0; y < m_graph.Height; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Down);
            }
        }
    }

    private void AlignLeft()
    {
        for (int x = 0; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Left);
            }
        }
    }

    private void AlignRight()
    {
        for (int x = m_graph.Width - 1; x >= 0; x--)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Right);
            }
        }
    }

    private void AlignLeftAndRight()
    {
        int halfWidth = m_graph.Width / 2;

        for (int x = 0; x < halfWidth; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Left);
            }
        }

        for (int x = m_graph.Width - 1; x >= halfWidth; x--)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Right);
            }
        }
    }

    private void AlignUpAndDown()
    {
        int halfHeight = m_graph.Height / 2;

        for (int y = 0; y < halfHeight; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Down);
            }
        }

        for (int y = m_graph.Height - 1; y >= halfHeight; y--)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCard(m_graph.nodes[x, y], Direction.Up);
            }
        }
    }

    private void AlignCenterHorizontally()
    {
        int halfHeight = m_graph.Height / 2;

        for (int y = halfHeight; y < m_graph.Height; y++)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCardHorizontallyCenter(m_graph.nodes[x, y]);
            }
        }

        for (int y = halfHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < m_graph.Width; x++)
            {
                MoveCardHorizontallyCenter(m_graph.nodes[x, y]);
            }
        }
    }

    private void AlignCenterVertically()
    {
        int halfWidth = m_graph.Width / 2;

        for (int x = halfWidth; x < m_graph.Width; x++)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCardVerticallyCenter(m_graph.nodes[x, y]);
            }
        }

        for (int x = halfWidth - 1; x >= 0; x--)
        {
            for (int y = 0; y < m_graph.Height; y++)
            {
                MoveCardVerticallyCenter(m_graph.nodes[x, y]);
            }
        }
    }

    private void MoveCard(Node source, Direction direction)
    {
        if (source != null && source.nodeType == NodeType.Blocked)
        {
            Node target = GetTargetNode(source, direction);

            if (target == null || target == source)
            {
                return;
            }

            MoveCard(source, target);
        }
    }

    private void MoveCardVerticallyCenter(Node source)
    {
        if (source != null && source.nodeType == NodeType.Blocked)
        {
            Node target = GetVerticallyCenterTargetNode(source);

            if (target == null || target == source)
            {
                return;
            }

            MoveCard(source, target);
        }
    }

    private void MoveCardHorizontallyCenter(Node source)
    {
        if (source != null && source.nodeType == NodeType.Blocked)
        {
            Node target = GetHorizontallyCenterTargetNode(source);

            if (target == null || target == source)
            {
                return;
            }

            MoveCard(source, target);
        }
    }

    private void MoveCard(Node source, Node target)
    {
        source.nodeType = NodeType.Open;
        target.nodeType = NodeType.Blocked;

        int idCard = m_graphView.GetIDCard(source);

        m_graphView.ResetNodeView(source);
        m_graphView.ClearCard(source);
        m_graphView.ShowCard(target, idCard, false);
        m_graphView.MoveCard(target, source.position);
    }
}
