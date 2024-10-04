using UnityEngine;

namespace PrimeTween
{
    public class TweenHeaving : MonoBehaviour
    {
        [Header("Setting")]
        public TweenSettings<Vector3> heavingSetting;
        public bool playOnEnable = true;

        private void OnEnable()
        {
            if (playOnEnable)
            {
                Heaving();
            }
        }

        private void OnDisable()
        {
            Tween.StopAll(transform);
            transform.localScale = heavingSetting.startValue;
        }

        public void Heaving()
        {
            Tween.Scale(transform, heavingSetting);
        }
    }
}