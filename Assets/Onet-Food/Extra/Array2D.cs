using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Array2D<T>
{
    public Row<T>[] array2d;

    [SerializeField] private int width;
    [SerializeField] private int height;

    public int Width
    {
        get => width;
    }

    public int Height
    {
        get => height;
    }

    public Array2D(T[,] array2d)
    {
        width = array2d.GetLength(0);
        height = array2d.GetLength(1);

        this.array2d = new Row<T>[Height];

        for (int i = 0; i < Height; i++)
        {
            this.array2d[i] = new Row<T>(Width);
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                this.array2d[y].row[x] = array2d[x, y];
            }
        }
    }

    public T GetElement(int x, int y)
    {
        if (!IsValid(x, y))
            return default;

        return array2d[y].row[x];
    }

    public bool IsValid(int x, int y)
    {
        return (x >= 0) && (x <= Width) && (y >= 0) && (y <= Height);
    }
}

[Serializable]
public struct Row<T>
{
    public T[] row;

    public Row(int length)
    {
        row = new T[length];
    }
}