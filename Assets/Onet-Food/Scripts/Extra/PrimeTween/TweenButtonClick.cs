using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PrimeTween
{
    [RequireComponent(typeof(Button))]
    public class TweenButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [Header("Setting")]
        public Vector3 defaultScale = Vector3.one;
        public Vector3 chooseScale = Vector3.one * 0.95f;

        public float duration = 0.1f;
        public Ease ease = Ease.Default;
        public bool ignoreTimeScale = true;

        public void OnPointerDown(PointerEventData eventData)
        {
            ScaleDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ScaleUp();
        }

        public void ScaleDown()
        {
            Tween.Scale(transform, defaultScale, chooseScale, duration, ease, useUnscaledTime: ignoreTimeScale);
        }

        public void ScaleUp()
        {
            Tween.Scale(transform, chooseScale, defaultScale, duration, ease, useUnscaledTime: ignoreTimeScale);
        }
    }
}
