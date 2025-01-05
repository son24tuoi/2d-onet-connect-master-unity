using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SFXType { Click, Fail }

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Element")]
    public AudioProfileSO audioProfileSO;

    [Space(5)]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private DataManager m_dataManager;

    public DataManager DataManager
    {
        get
        {
            if (ReferenceEquals(m_dataManager, null))
            {
                m_dataManager = DataManager.Instance;
            }
            return m_dataManager;
        }
    }

    private AudioData m_audioData;

    public AudioData AudioData
    {
        get
        {
            if (ReferenceEquals(m_audioData, null))
            {
                m_audioData = DataManager.Data.audioData;
            }

            return m_audioData;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadingScene.OnLoadEvent += LoadSceneCallback;

        ChangeVolumeMusic(AudioData.VolumeMusic);
        ChangeVolumeSFX(AudioData.VolumeSFX);
        PlayMusic(AudioData.MusicIndex);
    }

    private void OnDestroy()
    {
        LoadingScene.OnLoadEvent -= LoadSceneCallback;
    }

    private void LoadSceneCallback()
    {
        ChangeVolumeMusic(AudioData.VolumeMusic);
        ChangeVolumeSFX(AudioData.VolumeSFX);
        PlayMusic(AudioData.MusicIndex);
    }

    public void PlayMusic(int index)
    {
        Sound s = audioProfileSO.GetSound(index);

        musicSource.clip = s.clip;
        musicSource.Play();

        AudioData.MusicIndex = index;
        DataManager.SaveData();
    }

    public void PlaySFX(int index)
    {
        Sound s = audioProfileSO.GetSFX(index);

        if (s == null)
        {
            Debug.LogWarning("AUDIOMANAGER PlaySFX: Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void PlaySFX(SFXType sfxType)
    {
        PlaySFX((int)sfxType);
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void ChangeVolumeMusic(float volume)
    {
        musicSource.volume = volume;

        AudioData.VolumeMusic = volume;
        DataManager.SaveData();
    }

    public void ChangeVolumeSFX(float volume)
    {
        sfxSource.volume = volume;

        AudioData.VolumeSFX = volume;
        DataManager.SaveData();
    }

    public void ChangeMusic(int index)
    {
        if (index == AudioData.MusicIndex)
            return;

        PlayMusic(index);
    }
}
