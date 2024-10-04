using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance { get; private set; }

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

    private VibrationData m_vibrationData;

    public VibrationData VibrationData
    {
        get
        {
            if (ReferenceEquals(m_vibrationData, null))
            {
                m_vibrationData = DataManager.Data.vibrationData;
            }

            return m_vibrationData;
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

    public void LightImpact()
    {
        if (!VibrationData.Vibration)
            return;

        MMVibrationManager.Haptic(HapticTypes.LightImpact);
    }

    public void Failure()
    {
        if (!VibrationData.Vibration)
            return;

        MMVibrationManager.Haptic(HapticTypes.Failure);
    }

    public void ToggleVibration()
    {
        VibrationData.Vibration = !VibrationData.Vibration;
        DataManager.SaveData();
    }
}
