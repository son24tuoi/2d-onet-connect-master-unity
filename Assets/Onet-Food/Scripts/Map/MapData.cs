using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    private static int m_width = 3;
    private static int m_height = 3;

    public static List<string> GetTextFromFile(TextAsset tAsset)
    {
        List<string> lines = new List<string>();

        if (tAsset != null)
        {
            string textData = tAsset.text;
            string[] delimiters = { "\r\n", "\n" };

            lines.AddRange(textData.Split(delimiters, StringSplitOptions.None));
            lines.Reverse();
        }
        else
        {
            Debug.LogWarning("MAPDATA GetTextFromFile Error: invalid TextAsset");
        }

        return lines;
    }

    public static void SetDimensions(List<string> textLines)
    {
        m_width = 3;
        m_height = textLines.Count;

        foreach (string line in textLines)
        {
            if (line.Length > m_width)
            {
                m_width = line.Length;
            }
        }
    }

    public static int[,] MakeMap(TextAsset textAsset)
    {
        List<string> lines = new List<string>();
        lines = GetTextFromFile(textAsset);
        SetDimensions(lines);

        int[,] map = new int[m_width, m_height];

        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                if (lines[y].Length > x)
                {
                    map[x, y] = (int)Char.GetNumericValue(lines[y][x]);
                }
            }
        }

        return map;
    }

    public static int GetCount(TextAsset textAsset)
    {
        int[,] map = MakeMap(textAsset);

        int width = map.GetLength(0);
        int height = map.GetLength(1);

        int count = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] != 0)
                {
                    count++;
                }
            }
        }

        return count;
    }
}
