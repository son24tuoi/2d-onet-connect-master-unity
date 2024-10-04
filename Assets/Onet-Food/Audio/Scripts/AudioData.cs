using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioData
{
    [SerializeField] private int musicIndex;
    [SerializeField] private float musicVolume;

    [Space(5)]
    [SerializeField] private float sfxVolume;

    public int MusicIndex
    {
        get => musicIndex;
        set => musicIndex = value;
    }

    public float VolumeMusic
    {
        get => musicVolume;
        set => musicVolume = value;
    }

    public float VolumeSFX
    {
        get => sfxVolume;
        set => sfxVolume = value;
    }

    public AudioData()
    {
        musicIndex = 0;
        musicVolume = 1f;
        sfxVolume = 1f;
    }
}
