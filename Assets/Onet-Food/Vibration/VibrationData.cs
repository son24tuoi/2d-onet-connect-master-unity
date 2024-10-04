using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VibrationData
{
    [SerializeField] private bool vibration;

    public bool Vibration
    {
        get => vibration;
        set => vibration = value;
    }

    public VibrationData()
    {
        vibration = true;
    }
}
