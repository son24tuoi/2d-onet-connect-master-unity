using System;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public static event Action OnTutorialEvent;

    public void OnClickTutorialButton()
    {
        OnTutorialEvent?.Invoke();
    }
}