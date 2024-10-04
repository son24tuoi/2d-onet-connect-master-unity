using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinitieScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public RectTransform[] itemList;

    private Vector2 m_oldVelocity;
    private bool m_isUpdated;

    private void Start()
    {
        m_isUpdated = false;
        m_oldVelocity = Vector2.zero;


        int itemToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (itemList[0].rect.width + HLG.spacing));

        for (int i = 0; i < itemToAdd; i++)
        {
            RectTransform rt = Instantiate(itemList[i & itemList.Length], contentPanelTransform);
            rt.SetAsLastSibling();
        }

        for (int i = 0; i < itemToAdd; i++)
        {
            int num = itemList.Length - i - 1;
            while (num < 0)
            {
                num += itemList.Length;
            }
            RectTransform rt = Instantiate(itemList[num], contentPanelTransform);
            rt.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3(0 - (itemList[0].rect.width + HLG.spacing) * itemToAdd,
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);
    }

    private void FixedUpdate()
    {
        if (m_isUpdated)
        {
            m_isUpdated = false;
            scrollRect.velocity = m_oldVelocity;
        }

        if (contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            m_oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(itemList.Length * (itemList[0].rect.width + HLG.spacing), 0, 0);
            m_isUpdated = true;
        }

        if (contentPanelTransform.localPosition.x < 0 - itemList.Length * (itemList[0].rect.width + HLG.spacing))
        {
            Canvas.ForceUpdateCanvases();
            m_oldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(itemList.Length * (itemList[0].rect.width + HLG.spacing), 0, 0);
            m_isUpdated = true;
        }
    }
}
