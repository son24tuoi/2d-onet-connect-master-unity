using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProfileSO", menuName = "Scriptable Object/Game Profile")]
public class GameProfileSO : ScriptableObject
{
    [Header("Controller")]
    public bool enablePlayerController;

    [Header("Level")]
    public int currentLevelIndex;
    public int starsReceived;
    public float elapsedSeconds = 0;
    public int starWin;
    public bool isNewHighScore;

    [Header("Popup")]
    public int countPopupShowed;

    [Header("Cheat")]
    public bool isCheat;

    public void Init()
    {
        countPopupShowed = 0;
        isCheat = false;
    }

    public int StarsReceived
    {
        get => starsReceived;
    }

    public void AddStarsReceived(int amount)
    {
        starsReceived += amount;
        EventManager.Instance.Trigger(EventID.StarReceived);
    }

    public void SetStarsReceived(int amount)
    {
        starsReceived = amount;
        EventManager.Instance.Trigger(EventID.StarReceived);
    }

    public string GetElapsedSeconds()
    {
        int minutes = (int)elapsedSeconds / 60;
        return minutes.ToString("00") + ":" + (elapsedSeconds - minutes * 60).ToString("00");
    }

    public void ShowPopup()
    {
        Utilities.SetTimeScale(0f);

        enablePlayerController = false;

        countPopupShowed++;
    }

    public void HidePopup()
    {
        countPopupShowed--;

        if (countPopupShowed <= 0)
        {
            Utilities.SetTimeScale(1f);

            enablePlayerController = true;
            countPopupShowed = 0;
        }
    }
}
