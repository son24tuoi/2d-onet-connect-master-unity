using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAdCanvas : Popup
{

    protected override void OnEnable()
    {
        base.OnEnable();

        IAPManager.OnRemoveAdEvent += RemoveAdCallback;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        IAPManager.OnRemoveAdEvent -= RemoveAdCallback;
    }

    private void RemoveAdCallback()
    {
        OnClickExitButton();
    }


    public void OnClickExitButton()
    {
        base.Exit();
    }

    public void OnClickLaterButton()
    {
        base.Exit();
    }
}
