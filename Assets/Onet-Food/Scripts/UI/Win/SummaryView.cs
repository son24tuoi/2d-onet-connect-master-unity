using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using PrimeTween;

public class SummaryView : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Image[] starImages;
    [SerializeField] private TextMeshProUGUI levelTMP;
    public TweenFadeCanvasGroup tweenFadeCanvasGroup;
    public GameProfileSO gameProfileSO;

    [Header("Config")]
    public Color normalStarColor = Color.white;
    public Color greyStarColor = Color.gray;

    public void Init(int levelIndex, bool win = true)
    {
        if (win)
        {
            InitWin(levelIndex);
        }
        else
        {
            InitLose(levelIndex);
        }
    }

    public void InitWin(int levelIndex)
    {
        levelTMP.SetText("Level " + DataManager.Instance.GetLevelName(levelIndex));

        SetupGrayStars();
        ShowStarTask().Forget();
    }

    public void InitLose(int levelIndex)
    {
        levelTMP.SetText("Level " + DataManager.Instance.GetLevelName(levelIndex));

        SetupGrayStars();
    }

    public void LoadStar()
    {
        int star = gameProfileSO.starWin;

        for (int i = 0; i < 3; i++)
        {
            starImages[i].color = (i < star) ? normalStarColor : greyStarColor;
        }
    }

    public void SetupGrayStars()
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            starImages[i].color = greyStarColor;
        }
    }

    public async UniTask ShowStarTask()
    {
        int star = gameProfileSO.starWin;

        if (star <= 0)
        {
            await UniTask.Yield();
        }

        float scaleUp = 3f;
        float scaleTime = 0.2f;
        float interval = 0.01f;
        await Tween.Delay(tweenFadeCanvasGroup.duration, useUnscaledTime: true);
        for (int i = 0; i < star; i++)
        {
            starImages[i].transform.localScale = Vector3.one * scaleUp;
            starImages[i].color = normalStarColor;
            await Tween.Scale(starImages[i].transform, 1f, scaleTime, useUnscaledTime: true);
            await Tween.Delay(interval, useUnscaledTime: true);
        }
    }
}
