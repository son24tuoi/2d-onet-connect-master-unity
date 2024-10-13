using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NodeGrid
{
    // [Serializable]
    // public struct NodeRow
    // {
    //     public Node[] row;

    //     public NodeRow(int length)
    //     {
    //         row = new Node[length];
    //     }
    // }

    // public NodeRow[] grid;

    public Array2D<int> grid;

    // [SerializeField] private int _width;
    // [SerializeField] private int _height;

    // public int Width
    // {
    //     get => _width;
    // }

    // public int Height
    // {
    //     get => _height;
    // }

    public NodeGrid(int[,] map)
    {
        grid = new Array2D<int>(map);
    }

    public int[,] GetMap()
    {
        int[,] map = new int[grid.Width, grid.Height];

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                map[x, y] = grid.GetElement(x, y);
            }
        }

        return map;
    }
}

