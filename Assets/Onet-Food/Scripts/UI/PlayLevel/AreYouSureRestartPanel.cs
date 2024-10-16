using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreYouSureRestartPanel : Popup
{
    public Action onYesEvent;

    public void OnClickYesButton()
    {
        ExitAndRemove(() =>
        {
            onYesEvent?.Invoke();
        });
    }

    public void OnClickNoButton()
    {
        ExitAndRemove();
    }
}
