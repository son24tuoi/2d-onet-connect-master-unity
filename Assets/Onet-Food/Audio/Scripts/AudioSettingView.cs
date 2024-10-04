using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingView : MyMonoBehaviour
{
    [Header("Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Dropdown musicDropdown;

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;

    [Header("Data")]
    public AudioProfileSO audioProfileSO;

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

    private void OnEnable()
    {
        Init();
    }

    public void Init()
    {
        musicSlider.value = AudioData.VolumeMusic;

        musicDropdown.ClearOptions();
        musicDropdown.AddOptions(audioProfileSO.GetMusicName());
        musicDropdown.value = AudioData.MusicIndex;

        sfxSlider.value = AudioData.VolumeSFX;
    }

    public void ChangeMusic(int index)
    {
        AudioManager.ChangeMusic(index);
    }

    public void ChangeMusicVolume(float volume)
    {
        AudioManager.ChangeVolumeMusic(volume);
    }

    public void ChangeSFXVolume(float volume)
    {
        AudioManager.ChangeVolumeSFX(volume);
    }
}
