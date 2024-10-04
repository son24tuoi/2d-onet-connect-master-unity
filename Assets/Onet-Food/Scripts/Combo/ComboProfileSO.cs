using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComboProfileSO", menuName = "Scriptable Object/Combo Profile")]
public class ComboProfileSO : ScriptableObject
{
    public int comboIndex;
    public int reward;

    public TimerData timerData;

    public void Reset()
    {
        comboIndex = 0;
        timerData.Reset();
    }
}
