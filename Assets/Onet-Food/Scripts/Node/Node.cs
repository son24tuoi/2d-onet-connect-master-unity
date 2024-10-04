using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    Open = 0,
    Blocked = 1
}

[Serializable]
public class Node : IComparable<Node>
{
    public NodeType nodeType = NodeType.Open;

    public int xIndex = -1;
    public int yIndex = -1;

    public Vector3 position;
    public float distanceTraveled = Mathf.Infinity;

    [SerializeField] public Node previous;
    [SerializeField] public Node secondPrevious;

    public float priority;

    [NonSerialized] public List<Node> neighbors = new List<Node>();

    public Node(int xIndex, int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    public void Reset()
    {
        previous = null;
        secondPrevious = null;
        distanceTraveled = Mathf.Infinity;
    }
    
    public Direction GetPathDiretion(Node target)
    {
        if (target == null)
        {
            return Direction.None;
        }

        Vector2 dir = new Vector2(target.xIndex - this.xIndex, target.yIndex - this.yIndex);

        if (dir == Vector2.up)
        {
            return Direction.Up;
        }
        else if (dir == Vector2.right)
        {
            return Direction.Right;
        }
        else if (dir == Vector2.down)
        {
            return Direction.Down;
        }
        else
        {
            return Direction.Left;
        }
    }

    public override string ToString()
    {
        return "Node(" + xIndex + ", " + yIndex + ") priority: " + priority;
    }

    public int CompareTo(Node other)
    {
        if (this.priority < other.priority)
        {
            return -1;
        }
        else if (this.priority > other.priority)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public bool IsNeighborBlocked()
    {
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i].nodeType != NodeType.Blocked)
            {
                return false;
            }
        }

        return true;
    }
}
