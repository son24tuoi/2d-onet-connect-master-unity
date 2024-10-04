using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnapToElement : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListElement;

    public HorizontalLayoutGroup HLG;

    public float threshold;
    public float snapForce;

    private bool m_isSnapped;
    private float m_snapSpeed;

    private void Start()
    {
        m_isSnapped = false;
    }

    private void FixedUpdate()
    {
        int currentElement =
            Mathf.RoundToInt((0 - contentPanel.localPosition.x / (sampleListElement.rect.width + HLG.spacing)));

        // Debug.Log(currentElement.ToString());

        if (scrollRect.velocity.magnitude < threshold && !m_isSnapped)
        {
            scrollRect.velocity = Vector2.zero;

            m_snapSpeed += snapForce * Time.deltaTime;

            float target = 0 - (currentElement * (sampleListElement.rect.width + HLG.spacing));

            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, target, m_snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localRotation.z);

            if (contentPanel.localPosition.x == target)
            {
                m_isSnapped = true;
            }
        }

        if (scrollRect.velocity.magnitude >= threshold)
        {
            m_isSnapped = false;
            m_snapSpeed = 0;
        }
    }
}
