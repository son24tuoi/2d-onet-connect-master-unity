using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForUpdateButton : MonoBehaviour
{
    public static event Action OnClickEvent;

    public void OnClickCheckForUpdateButton()
    {
        OnClickEvent?.Invoke();
    }
}
