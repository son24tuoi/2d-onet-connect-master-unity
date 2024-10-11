using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using PrimeTween;

public class TimerView : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Slider m_slider;
    [SerializeField] private RectTransform m_starContainer;
    [SerializeField] private Image[] m_starImages;
    [SerializeField] private TextMeshProUGUI m_timerTMP;
    [SerializeField] private Transform clock;

    [Header("Config")]
    public TimeProfileSO timeProfileSO;

    public Color normalStarColor = Color.white;
    public Color grayStarColor = Color.gray;

    public TimeSystem TimeSystem => timeProfileSO.timeSystem;

    private Coroutine m_updateRoutine;

    private float m_timeOneStar;
    private float m_timeTwoStar;
    private float m_timeThreeStar;

    public TimerData TimerData => timeProfileSO.timerData;

    private void Awake()
    {
        TimeProfileSO.OnMoreTimeEvent += MoreTime;
    }

    private void OnDestroy()
    {
        TimeProfileSO.OnMoreTimeEvent -= MoreTime;
    }

    private void OnDisable()
    {
        StopUpdateTimer();
    }

    public void Setup()
    {
        SetupSlider();
        SetupStar();

        m_updateRoutine = StartCoroutine(IEUpdate());
    }

    public void WinLevelCallback()
    {
        StopUpdateTimer();

        m_updateRoutine = StartCoroutine(IEWin());
    }

    private void MoreTime(int seconds)
    {
        m_slider.maxValue = TimerData.Duration;

        m_timeOneStar = timeProfileSO.TimeOneStar;
        m_timeTwoStar = timeProfileSO.TimeTwoStar;
        m_timeThreeStar = timeProfileSO.TimeThreeStar;

        TextPrefab textPrefab = ObjectPool.Instance.GetTextObject().GetComponent<TextPrefab>();

        if (textPrefab != null)
        {
            textPrefab.text.SetText("+" + Mathf.Abs(seconds) + "s");
            textPrefab.text.color = Color.green;
            // textPrefab.text.fontStyle = FontStyles.Bold;
            textPrefab.gameObject.SetActive(true);
            Tween.Position(textPrefab.transform, clock.transform.position, clock.transform.position + Vector3.up * 80f, 1f)
                .OnComplete(() =>
                {
                    textPrefab.gameObject.SetActive(false);
                });
        }
    }

    public void StopUpdateTimer()
    {
        if (m_updateRoutine != null)
        {
            StopCoroutine(m_updateRoutine);
        }
    }

    public void SetupStar()
    {
        float height = m_starContainer.rect.height;

        m_starImages[0].rectTransform.anchoredPosition = new Vector2(0, TimeSystem.oneStar * height);
        m_starImages[1].rectTransform.anchoredPosition = new Vector2(0, TimeSystem.twoStar * height);
        m_starImages[2].rectTransform.anchoredPosition = new Vector2(0, TimeSystem.threeStar * height);

        EventManager.Instance.Trigger(new EventData<Vector3[]>(
            EventID.ThreeStarsPosition,
            new Vector3[3] {
                m_starImages[0].transform.position,
                m_starImages[1].transform.position,
                m_starImages[2].transform.position
            }
        ));

        ShowStar(timeProfileSO.timerData.Duration);
        ChangeStar(timeProfileSO.timerData.Duration);
    }

    public void SetupSlider()
    {
        m_slider.maxValue = TimerData.Duration;
        m_slider.value = TimerData.Duration;

        m_timeOneStar = timeProfileSO.TimeOneStar;
        m_timeTwoStar = timeProfileSO.TimeTwoStar;
        m_timeThreeStar = timeProfileSO.TimeThreeStar;
    }

    public IEnumerator IEUpdate()
    {
        float value;

        while (TimerData.running)
        {
            value = TimerData.Remaining;

            m_slider.value = value;

            m_timerTMP.SetText(timeProfileSO.GetRemaining());

            ChangeStar(value);

            yield return null;
        }
    }

    public IEnumerator IEWin()
    {
        float value;
        while (!TimerData.running)
        {
            value = TimerData.Remaining;
            m_slider.value = value;
            ShowStar(value);
            yield return null;
        }
    }

    public void ChangeStar(float value)
    {
        m_starImages[2].color = (value < m_timeThreeStar) ? grayStarColor : normalStarColor;
        m_starImages[1].color = (value < m_timeTwoStar) ? grayStarColor : normalStarColor;
        m_starImages[0].color = (value < m_timeOneStar) ? grayStarColor : normalStarColor;
    }

    public void ShowStar(float value)
    {
        Debug.Log("aaaaaaaaaaaaaa");
        m_starImages[2].gameObject.SetActive(value >= m_timeThreeStar);
        m_starImages[1].gameObject.SetActive(value >= m_timeTwoStar);
        m_starImages[0].gameObject.SetActive(value >= m_timeOneStar);
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(TimerView))]
    public class TimerView_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(20);

            TimerView target = (TimerView)base.target;

            if (GUILayout.Button("Setup Star"))
            {
                target.SetupStar();
            }
        }
    }
#endif
}
