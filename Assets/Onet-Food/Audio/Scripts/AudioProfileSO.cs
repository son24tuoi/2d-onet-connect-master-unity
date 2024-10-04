using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioProfileSO", menuName = "Scriptable Object/Audio Profile")]
public class AudioProfileSO : ScriptableObject
{
    public Sound[] musicSounds;

    [Space(5)]
    public Sound[] sfxSounds;

    public Sound GetSound(string name)
    {
        return Array.Find(musicSounds, x => x.name == name);
    }

    public Sound GetSound(int index)
    {
        index = Mathf.Clamp(index, 0, musicSounds.Length - 1);
        return musicSounds[index];
    }

    public Sound GetSFX(string name)
    {
        return Array.Find(sfxSounds, x => x.name == name);
    }

    public Sound GetSFX(int index)
    {
        index = Mathf.Clamp(index, 0, sfxSounds.Length - 1);
        return sfxSounds[index];
    }

    public List<string> GetMusicName()
    {
        List<string> name = new List<string>();

        for (int i = 0; i < musicSounds.Length; i++)
        {
            name.Add(musicSounds[i].name);
        }

        return name;
    }
}
