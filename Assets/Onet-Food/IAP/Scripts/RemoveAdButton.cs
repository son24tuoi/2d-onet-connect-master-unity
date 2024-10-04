using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdButton : MonoBehaviour
{
    public static Action OnClick;

    public void OnClickButton()
    {
        OnClick?.Invoke();
    }
}
