using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using NaughtyAttributes;
using UnityEngine;

public class TestPos : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [Button]
    public void LogPos()
    {
        Debug.Log(Camera.main.transform.position);
        Debug.Log(Camera.main.ScreenToWorldPoint(rectTransform.transform.position));
        Debug.Log("offset x: " + (Camera.main.ScreenToWorldPoint(rectTransform.transform.position).x - Camera.main.transform.position.x));
    }
}
