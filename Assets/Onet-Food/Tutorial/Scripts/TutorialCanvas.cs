using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvas : Popup
{
    [Header("Element")]
    [SerializeField] private TutorialView tutorialView;

    protected override void OnEnable()
    {
        base.OnEnable();

        tutorialView.SetState(0);
    }

    public void OnClickExitButton()
    {
        base.Exit();
    }
}
