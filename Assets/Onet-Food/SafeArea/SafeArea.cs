using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform m_rectTransform;
    private Rect m_safeArea;
    private Vector2 m_minAnchor;
    private Vector2 m_maxAnchor;

    [ContextMenu(nameof(Awake))]
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_safeArea = Screen.safeArea;
        m_minAnchor = m_safeArea.position;
        m_maxAnchor = m_minAnchor + m_safeArea.size;

        m_minAnchor.x /= Screen.width;
        m_minAnchor.y /= Screen.height;
        m_maxAnchor.x /= Screen.width;
        m_maxAnchor.y /= Screen.height;

        m_rectTransform.anchorMin = m_minAnchor;
        m_rectTransform.anchorMax = m_maxAnchor;
    }
}
