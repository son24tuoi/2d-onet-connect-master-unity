using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayCanvas : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private TweenFadeImage warningEffect;
    [SerializeField] private TextMeshProUGUI levelTMP;
    [SerializeField] private TimerView timerView;
    public RectTransform playingArea;
    [SerializeField] private GameObject noInteraction;

    [Space(5)]
    [SerializeField] private GameProfileSO gameProfileSO;

    public bool Interaction
    {
        get => !noInteraction.activeSelf;
        set => noInteraction.SetActive(!value);
    }

    public void Init()
    {
        levelTMP.SetText("Level " + DataManager.Instance.GetLevelName(gameProfileSO.currentLevelIndex));
        timerView.Setup();
        Interaction = true;
    }

    public void OnClickPauseButton()
    {
        GameManager.Instance.uiController.ShowPauseCanvas();
        EventManager.Instance.Trigger(EventID.UpdateProgressLevel);
    }

    public void Win()
    {
        timerView.WinLevelCallback();
    }

    public void Warning()
    {
        warningEffect.Fade(() =>
        {
            warningEffect.ReverseFade();
        });
    }
}
