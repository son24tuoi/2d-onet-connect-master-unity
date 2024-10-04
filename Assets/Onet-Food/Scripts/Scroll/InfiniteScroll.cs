using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public VerticalLayoutGroup verticalLayoutGroup;

    public RectTransform[] items;

    private void Start()
    {
        int itemToAdd = Mathf.CeilToInt(viewPortTransform.rect.height / (items[0].rect.height + verticalLayoutGroup.spacing));

        for (int i = 0; i < itemToAdd; i++)
        {
            RectTransform rt = Instantiate(items[i % items.Length], contentPanelTransform);
            rt.SetAsLastSibling();
        }

        contentPanelTransform.localPosition = new Vector3(0 - (items[0].rect.height +verticalLayoutGroup.spacing) * itemToAdd,
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);
    }
}
