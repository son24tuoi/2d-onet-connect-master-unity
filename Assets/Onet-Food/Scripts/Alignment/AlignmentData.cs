using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

[Serializable]
public class AlignmentData
{
    public AlignmentType[] alignmentTypes;
    public AlignmentSearchType alignmentSearchType;

    public AlignmentType GetAlignmentType(int index)
    {
        if (alignmentTypes == null)
            return AlignmentType.None;

        if (alignmentTypes.Length == 0)
            return alignmentTypes[0];

        return alignmentSearchType switch
        {
            AlignmentSearchType.Circle => GetCircle(index),
            AlignmentSearchType.Last => GetLast(index),
            AlignmentSearchType.First => GetFirst(index),
            AlignmentSearchType.Random => GetRandom(index),
            _ => GetLast(index)
        };
    }

    public AlignmentType GetCircle(int index)
    {
        return alignmentTypes[index % alignmentTypes.Length];
    }

    public AlignmentType GetLast(int index)
    {
        return alignmentTypes[Mathf.Clamp(index, 0, alignmentTypes.Length - 1)];
    }

    public AlignmentType GetFirst(int index)
    {
        if (index < alignmentTypes.Length)
            return alignmentTypes[index];
        else
            return alignmentTypes[0];
    }

    public AlignmentType GetRandom(int index)
    {
        if (index < alignmentTypes.Length)
            return alignmentTypes[index];
        else
            return alignmentTypes[Random.Range(0, alignmentTypes.Length)];
    }
}
