using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class VibrationSettingView : MyMonoBehaviour
{
    [Header("Element")]
    [SerializeField] private GameObject backgroundOff;
    [SerializeField] private GameObject backgroundOn;

    [Space(5)]
    [SerializeField] private Transform handle;
    [SerializeField] private Transform handleOff;
    [SerializeField] private Transform handleOn;

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

    private void OnEnable()
    {
        SetStatus(VibrationData.Vibration);
    }

    public void SetStatus(bool vibration)
    {
        backgroundOff.SetActive(!vibration);
        backgroundOn.SetActive(vibration);

        handle.localPosition = vibration ? handleOn.localPosition : handleOff.localPosition;
    }

    public void OnClickToggleButton()
    {
        VibrationManager.ToggleVibration();

        bool vibration = VibrationData.Vibration;
        backgroundOff.SetActive(!vibration);
        backgroundOn.SetActive(vibration);

        Tween.Position(handle,
            startValue: vibration ? handleOff.position : handleOn.position,
            endValue: vibration ? handleOn.position : handleOff.position,
            duration: 0.1f,
            useUnscaledTime: true)
            .OnComplete(() =>
            {
                VibrationManager.LightImpact();
            });
    }
}
