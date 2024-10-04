using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingCanvas : Popup
{
    [Header("Element")]
    [SerializeField] public InputField deviceId;

    private int m_countClick = 0;

    protected override void OnEnable()
    {
        base.OnEnable();

        deviceId.gameObject.SetActive(false);
        m_countClick = 0;
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }

    public void OnClickVersionText()
    {
        m_countClick++;

        if (m_countClick >= 5)
        {
            deviceId.SetTextWithoutNotify(SystemInfo.deviceUniqueIdentifier);
            deviceId.gameObject.SetActive(true);
            GameManager.Instance.Test();
        }
    }
}
