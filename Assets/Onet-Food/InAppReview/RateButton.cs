using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateButton : MonoBehaviour
{
    public static event Action OnClickEvent;

    public void OnClickRateButton()
    {
        OnClickEvent?.Invoke();
    }


}
