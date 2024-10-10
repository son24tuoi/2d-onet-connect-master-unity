using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDetector : MonoBehaviour
{
    public static Action OnChangeScreenOrientationEvent;

    private ScreenOrientation lastScreenOrientation;

    private void FixedUpdate()
    {
        if (Screen.orientation != lastScreenOrientation)
        {
            lastScreenOrientation = Screen.orientation;
            OnChangeScreenOrientationEvent?.Invoke();
        }
    }
}
