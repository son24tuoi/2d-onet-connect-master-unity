using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeProfileSO", menuName = "Scriptable Object/Time Profile")]
public class TimeProfileSO : ScriptableObject
{
    public static event Action<int> OnMoreTimeEvent;

    public TimeSystem timeSystem;

    [Header("Timer")]
    public TimerData timerData;

    public void Setup(TimeSystem timeSystem)
    {
        this.timeSystem = timeSystem;

        timerData.Setup(timeSystem.maxPlayTime);
    }

    public void AddDuration(int seconds)
    {
        timerData.AddDuration(seconds);
        OnMoreTimeEvent?.Invoke(seconds);
    }

    public void AddElapsedSeconds(int seconds)
    {
        timerData.elapsedSeconds += seconds;
        OnMoreTimeEvent?.Invoke(seconds);
    }

    public int GetStar()
    {
        if (timerData.Remaining >= timeSystem.TimeThreeStar)
        {
            return 3;
        }
        else if (timerData.Remaining >= timeSystem.TimeTwoStar)
        {
            return 2;
        }
        else if (timerData.Remaining >= timeSystem.TimeOneStar)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public string GetRemaining()
    {
        int minutes = (int)timerData.Remaining / 60;
        return minutes.ToString("00") + ":" + ((int)timerData.Remaining - 60 * minutes).ToString("00");
    }




#if UNITY_EDITOR
    [CustomEditor(typeof(TimeProfileSO))]
    public class TimeProfileSO_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TimeProfileSO target = (TimeProfileSO)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            GUILayout.Box("Get Stars: " + target.GetStar().ToString());
            GUILayout.Box("Remaining Time 3 Star: " + target.timeSystem.TimeThreeStar.ToString());
            GUILayout.Box("Remaining Time 2 Star: " + target.timeSystem.TimeTwoStar.ToString());
            GUILayout.Box("Remaining Time 1 Star: " + target.timeSystem.TimeOneStar.ToString());
        }
    }
#endif
}
