using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEditor;

public class TweenShakeRotation : MonoBehaviour
{
    [Header("Setting")]
    public ShakeSettings shakeSettings;
    public bool playOnEnable = true;

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Shake();
        }
    }

    public void Shake()
    {
        Tween.ShakeLocalRotation(transform, shakeSettings);
    }

    public void Stop()
    {
        Tween.CompleteAll(onTarget: transform);
    }





#if UNITY_EDITOR
    [CustomEditor(typeof(TweenShakeRotation))]
    public class TweenShakeRotation_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TweenShakeRotation target = (TweenShakeRotation)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button("Shake"))
            {
                target.Shake();
            }

            if (GUILayout.Button("Stop"))
            {
                target.Stop();
            }
        }
    }
#endif
}
