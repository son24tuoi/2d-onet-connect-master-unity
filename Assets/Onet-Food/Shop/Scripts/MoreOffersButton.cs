using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreOffersButton : MonoBehaviour
{
    public void OnClickButton()
    {
        EventManager.Instance.Trigger(EventID.ShopCanvas);
    }
}
