using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public Node[,] nodes;
    public List<Node> walls = new List<Node>();

    private int[,] m_mapData;
    private int m_width;
    private int m_height;

    public int Width => m_width;
    public int Height => m_height;

    public static readonly Vector2[] allDirections =
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    public void Init(int[,] mapData)
    {
        m_mapData = mapData;
        m_width = mapData.GetLength(0);
        m_height = mapData.GetLength(1);
        walls = new List<Node>();

        nodes = new Node[m_width, m_height];

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                NodeType type = (NodeType)mapData[x, y];
                Node newNode = new Node(x, y, type);
                nodes[x, y] = newNode;

                newNode.position = new Vector3(x, y, 0);

                if (type == NodeType.Blocked)
                {
                    walls.Add(newNode);
                }
            }
        }

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                nodes[x, y].neighbors = GetNeighbors(x, y);
            }
        }
    }

    public bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < m_width && y >= 0 && y < m_height);
    }

    private List<Node> GetNeighbors(int x, int y, Node[,] nodeArray, Vector2[] directions)
    {
        List<Node> neighborNodes = new List<Node>();

        foreach (Vector2 dir in directions)
        {
            int newX = x + (int)dir.x;
            int newY = y + (int)dir.y;

            if (IsWithinBounds(newX, newY) &&
                nodeArray[newX, newY] != null)
            {
                neighborNodes.Add(nodeArray[newX, newY]);
            }
        }

        return neighborNodes;
    }

    private List<Node> GetNeighbors(int x, int y)
    {
        return GetNeighbors(x, y, nodes, allDirections);
    }

    public Node GetNeighbor(int x, int y, Vector2 direction)
    {
        int nextX = x + (int)Mathf.Clamp(direction.x, -1, 1);
        int nextY = y + (int)Mathf.Clamp(direction.y, -1, 1);

        if (IsWithinBounds(nextX, nextY) &&
            nodes[nextX, nextY] != null)
        {
            return nodes[nextX, nextY];
        }

        return null;
    }

    public Node GetNeighbor(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.Up => GetNeighbor(x, y, Vector2.up),
            Direction.Right => GetNeighbor(x, y, Vector2.right),
            Direction.Down => GetNeighbor(x, y, Vector2.down),
            Direction.Left => GetNeighbor(x, y, Vector2.left),
            _ => null,
        };
    }

    public bool IsNeighborFree(Node node, Direction direction)
    {
        Node nextNode = GetNeighbor(node.xIndex, node.yIndex, direction);

        if (nextNode != null && nextNode.nodeType == NodeType.Open)
        {
            return true;
        }

        return false;
    }

    public bool IsNodesFree(List<Node> nodes)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] == null || nodes[i].nodeType == NodeType.Blocked)
            {
                return false;
            }
        }

        return true;
    }

    public bool IsNodeFree(Node node)
    {
        return (node != null) && (node.nodeType == NodeType.Open);
    }

    public List<Node> FindNodes(int startX, int startY, Vector2 searchDirection, int length)
    {
        List<Node> nodes = new List<Node>();

        int nextX;
        int nextY;
        Node nextNode;

        for (int i = 1; i <= length; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            nextNode = this.nodes[nextX, nextY];

            if (nextNode == null)
            {
                break;
            }
            else
            {
                nodes.Add(nextNode);
            }
        }

        return nodes;
    }

    public List<Node> FindNodes(int startX, int startY, Direction direction, int length)
    {
        List<Node> nodes = new List<Node>();

        switch (direction)
        {
            case Direction.Up:
                nodes = FindNodes(startX, startY, Vector2.up, length);
                break;
            case Direction.Right:
                nodes = FindNodes(startX, startY, Vector2.right, length);
                break;
            case Direction.Down:
                nodes = FindNodes(startX, startY, Vector2.down, length);
                break;
            case Direction.Left:
                nodes = FindNodes(startX, startY, Vector2.left, length);
                break;
        }

        return nodes;
    }

    public List<Node> FindNodesAlongVertical(Node startNode, Node endNode)
    {
        List<Node> nodes = new List<Node>();

        if (startNode.xIndex == endNode.xIndex) // Same x, Check y
        {
            int length = Mathf.Abs(startNode.yIndex - endNode.yIndex);

            nodes = FindNodes(startNode.xIndex,
                            startNode.yIndex,
                            (startNode.yIndex < endNode.yIndex) ? Direction.Up : Direction.Down,
                            length);
        }

        return nodes;
    }

    public List<Node> FindNodesAlongHorizontal(Node startNode, Node endNode)
    {
        List<Node> nodes = new List<Node>();

        if (startNode.yIndex == endNode.yIndex) // Same y, Check x
        {
            int length = Mathf.Abs(startNode.xIndex - endNode.xIndex);

            nodes = FindNodes(startNode.xIndex,
                            startNode.yIndex,
                            (startNode.xIndex < endNode.xIndex) ? Direction.Right : Direction.Left,
                            length);
        }

        return nodes;
    }

    public List<Node> FindNodesAlongStraight(Node startNode, Node endNode)
    {
        List<Node> nodes = new List<Node>();

        if (startNode.xIndex == endNode.xIndex) // Vertical
        {
            nodes = FindNodesAlongVertical(startNode, endNode);
        }
        else if (startNode.yIndex == endNode.yIndex) // Horizontal
        {
            nodes = FindNodesAlongHorizontal(startNode, endNode);
        }

        return nodes;
    }

    public List<Node> FindNodesAlongStraight(Node startNode, Direction dir)
    {
        List<Node> nodes = new List<Node>();

        if (dir == Direction.None)
        {
            Debug.LogWarning("GRAPH FindNodesAlongPerpendicularLine direction must not be None");
            return nodes;
        }

        Node endNode = dir switch
        {
            Direction.Up => this.nodes[startNode.xIndex, m_height - 1],
            Direction.Right => this.nodes[m_width - 1, startNode.yIndex],
            Direction.Down => this.nodes[startNode.xIndex, 0],
            Direction.Left => this.nodes[0, startNode.yIndex],
            _ => startNode,
        };

        nodes = (dir == Direction.Right || dir == Direction.Left) ?
                FindNodesAlongHorizontal(startNode, endNode) :
                FindNodesAlongVertical(startNode, endNode);

        return nodes;
    }

    public List<Node> FindNodesAlongPerpendicularLine(Node startNode, Node endNode, Direction firstDir, Direction secDir)
    {
        List<Node> nodes = new List<Node>();

        if (firstDir == Direction.None || secDir == Direction.None)
        {
            Debug.LogWarning("GRAPH FindNodesAlongPerpendicularLine direction must not be None");
            return nodes;
        }

        int midX = (firstDir == Direction.Right || firstDir == Direction.Left) ? endNode.xIndex : startNode.xIndex;
        int midY = (firstDir == Direction.Right || firstDir == Direction.Left) ? startNode.yIndex : endNode.yIndex;

        Node midNode = this.nodes[midX, midY];

        if (midNode == null)
        {
            Debug.LogWarning("GRAPH FindNodesAlongPerpendicularLine mid node is Null");
            return nodes;
        }

        List<Node> firstNodes = FindNodesAlongStraight(startNode, midNode);

        List<Node> secNodes = FindNodesAlongStraight(midNode, endNode);

        nodes.AddRange(firstNodes);
        nodes.AddRange(secNodes);

        return nodes;
    }












    public float GetNodeDistanceNeighbor(Node source, Node target)
    {
        if (source.previous != null)
        {
            Direction preDir = source.previous.GetPathDiretion(source);
            Direction dir = source.GetPathDiretion(target);

            if (source.secondPrevious != null)
            {
                Direction secondPreDir = source.secondPrevious.GetPathDiretion(source);

                if (secondPreDir == dir)
                {
                    source.previous = source.secondPrevious;
                    source.secondPrevious = null;
                    return 0.1f;
                }
            }

            source.secondPrevious = null;
            return (preDir == dir) ? 0.1f : 100f;
        }
        else
        {
            return 0.1f;
        }
    }

    public float GetNodeDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.4f * diagonalSteps + straightSteps);
    }

    public bool IsTurn(Node source, Node target)
    {
        if (source.previous != null)
        {
            Direction preDir = source.previous.GetPathDiretion(source);
            Direction dir = source.GetPathDiretion(target);

            return preDir != dir;
        }
        else
        {
            return false;
        }
    }
}
