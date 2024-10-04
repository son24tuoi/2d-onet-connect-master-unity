using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboView : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI rewardTMP;
    [SerializeField] private TextMeshProUGUI titleTMP;

    [Space(5)]
    public ComboProfileSO comboProfileSO;

    private Coroutine m_updateRoutine;

    private Vector3 m_showPos;
    private Vector3 m_hidePos;

    public TimerData TimerData => comboProfileSO.timerData;

    private void Start()
    {
        m_showPos = transform.position;
        m_hidePos = m_showPos + GetComponent<RectTransform>().rect.width * Vector3.right;

        transform.position = m_hidePos;
    }

    private void OnEnable()
    {
        Combo.OnStartComboEvent += StartCountdown;
    }

    private void OnDisable()
    {
        Combo.OnStartComboEvent -= StartCountdown;
    }

    private void StartCountdown()
    {
        if (comboProfileSO.comboIndex < 2)
        {
            return;
        }

        slider.maxValue = TimerData.Duration;

        rewardTMP.SetText("+" + comboProfileSO.reward + "s");
        titleTMP.SetText("Combo " + comboProfileSO.comboIndex);

        Show();

        StartUpdateRoutine();
    }

    public void StartUpdateRoutine()
    {
        if (m_updateRoutine != null)
        {
            StopCoroutine(m_updateRoutine);
        }

        m_updateRoutine = StartCoroutine(IEUpdate());
    }

    public IEnumerator IEUpdate()
    {
        float value;

        while (TimerData.running)
        {
            value = TimerData.Remaining;

            slider.value = value;

            yield return null;
        }

        Hide();
    }

    public void Show()
    {
        if (transform.position == m_showPos)
        {
            return;
        }

        Tween.Position(transform, m_hidePos, m_showPos, 0.1f);
    }

    public void Hide()
    {
        if (transform.position == m_hidePos)
        {
            return;
        }

        Tween.Position(transform, m_showPos, m_hidePos, 0.1f);
    }
}
