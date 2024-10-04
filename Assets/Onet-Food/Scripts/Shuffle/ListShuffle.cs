using System.Collections.Generic;
using UnityEngine;

public class ListShuffler
{
    public static void Shuffle<T>(List<T> list)
    {
        int randomIndex;
        T value;

        for (int i = 0; i < list.Count; i++)
        {
            randomIndex = Random.Range(i, list.Count);
            value = list[randomIndex];
            list[randomIndex] = list[i];
            list[i] = value;
        }
    }

    public static void Swap<T>(List<T> list, int x, int y)
    {
        T tmp = list[x];
        list[x] = list[y];
        list[y] = tmp;
    }
}