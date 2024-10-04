using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToItem : MonoBehaviour
{
    [Header("Element")]
    public ScrollRect scrollRect;
    public RectTransform contentPanel;

    public void ScrollTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector3 viewPortLocalPosition = scrollRect.viewport.localPosition;
        Vector3 targetLocalPosition = target.localPosition;

        Vector3 newTargetLocalPosition = new Vector3(
            0 - (viewPortLocalPosition.x + targetLocalPosition.x),
            0 - (viewPortLocalPosition.y + targetLocalPosition.y)
        );

        contentPanel.localPosition = newTargetLocalPosition;
    }
}
