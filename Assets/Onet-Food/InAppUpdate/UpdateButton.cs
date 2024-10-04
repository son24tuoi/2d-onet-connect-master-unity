using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateButton : MonoBehaviour
{
    public static event Action OnClickEvent;

    public void OnClickUpdateButton()
    {
        OnClickEvent?.Invoke();
    }
}
