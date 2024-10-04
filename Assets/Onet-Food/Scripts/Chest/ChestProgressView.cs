using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestProgressView : MonoBehaviour
{
    public static event Action OnChestEvent;

    [Header("Element")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI progressText;

    [Space(5)]
    [SerializeField] private GameObject notify;
    [SerializeField] private TextMeshProUGUI iconText;

    [Space(5)]
    public ChestProfileSO chestProfileSO;

    private int m_maxValue;
    private int m_value;

    private void OnEnable()
    {
        ChestProgress.OnUpdateChestEvent += Setup;
    }

    private void OnDisable()
    {
        ChestProgress.OnUpdateChestEvent -= Setup;
    }

    public void Setup(int chestIndex, int starAmount)
    {
        m_maxValue = chestProfileSO.GetRequire(chestIndex);
        m_value = starAmount;

        SetProgress(m_maxValue, m_value);

        SetNotify(m_value >= m_maxValue);
        iconText.SetText((chestIndex + 1).ToString());
    }

    public void SetProgress(int maxValue, int value)
    {
        slider.maxValue = maxValue;
        slider.value = value;

        progressText.SetText(value + "<#313E54>/" + maxValue);
    }

    public void SetNotify(bool show)
    {
        notify.SetActive(show);
    }

    public void RunEffect(int from, int to, float duration, float startDelay = 0f, Ease ease = Ease.Linear)
    {
        Sequence.Create(useUnscaledTime: true)
            .Group(
                Tween.Custom(from, to, duration,
                    onValueChange: newVal => progressText.SetText((int)newVal + "<#313E54>/" + m_maxValue),
                    ease: ease,
                    startDelay: startDelay))
            .Group(
                Tween.Custom(from, to, duration,
                    onValueChange: newVal => slider.value = (int)newVal,
                    ease: ease,
                    startDelay: startDelay))
            .OnComplete(() =>
            {
                m_value = to;
                SetNotify(m_value >= m_maxValue);
            });

    }

    public void OnClickChestButton()
    {
        if (m_value >= m_maxValue)
        {
            OnChestEvent?.Invoke();
        }
    }
}
