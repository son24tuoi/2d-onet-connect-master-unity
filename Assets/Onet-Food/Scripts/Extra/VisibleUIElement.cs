using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleUIElement : MonoBehaviour
{
    [SerializeField] private RectTransform boundsRT;
    [SerializeField] private RectTransform objectRT;

    private Rect boundsWorldRect;
    private Rect objectWorldRect;
    private Vector3 distance;

    public void Setup(RectTransform boundsRT, RectTransform objectRT)
    {
        this.boundsRT = boundsRT;
        this.objectRT = objectRT;
        Setup();
    }

    public void Setup()
    {
        if (boundsRT == null || objectRT == null)
        {
            Debug.LogWarning("VISIBLE UI ELEMENT: object is NULL!");
            return;
        }

        boundsWorldRect = boundsRT.GetWorldRect();
        objectWorldRect = objectRT.GetWorldRect();
    }

    // Kiểm tra phần tử nằm trong phần bao
    public bool IsObjectHidden()
    {
        objectWorldRect = objectRT.GetWorldRect();

        return !RectTransformUtility.RectangleContainsScreenPoint(boundsRT, objectWorldRect.min) ||
                !RectTransformUtility.RectangleContainsScreenPoint(boundsRT, objectWorldRect.max);
    }

    public void ComparePos()
    {
        boundsWorldRect = boundsRT.GetWorldRect();
        objectWorldRect = objectRT.GetWorldRect();

        distance = objectWorldRect.center - boundsWorldRect.center;
        Debug.Log(distance);
        if (distance.y > 0)
        {
            Debug.Log("Up");
        }
        else if (distance.y < 0)
        {
            Debug.Log("Down");
        }

        if (distance.x > 0)
        {
            Debug.Log("Right");
        }
        else if (distance.x < 0)
        {
            Debug.Log("Left");
        }
    }




















    // -------------------------------------------------------------------------------------------------

#if UNITY_EDITOR

    [UnityEditor.CustomEditor(typeof(VisibleUIElement))]
    public class VisibleUIElement_Inspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            VisibleUIElement target = (VisibleUIElement)base.target;

            GUILayout.Space(20f);

            GUIStyle labelStyle = new GUIStyle()
            {
                fontSize = 15,
                fontStyle = FontStyle.Bold
            };

            GUILayout.Label("Quick Access", labelStyle);

            if (GUILayout.Button(nameof(target.Setup)))
            {
                target.Setup();
            }

            if (GUILayout.Button(nameof(target.IsObjectHidden)))
            {
                Debug.Log(nameof(target.IsObjectHidden) + ": " + target.IsObjectHidden());
            }

            if (GUILayout.Button(nameof(target.ComparePos)))
            {
                target.ComparePos();
            }
        }
    }

#endif

    // -------------------------------------------------------------------------------------------------
}
