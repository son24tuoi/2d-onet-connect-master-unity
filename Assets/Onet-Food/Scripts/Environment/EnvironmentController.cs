using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public static EnvironmentController Instance { get; private set; }

    public Camera mainCamera;
    public Transform background;

    private float _originCameraSize;
    private float _originBackgroundScale;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _originCameraSize = mainCamera.orthographicSize;
        _originBackgroundScale = background.localScale.x;
    }

    public void AdjustCamera(Vector3 position, float size)
    {
        SetPositionCamera(position);
        SetSizeCamera(size);
    }

    public void SetPositionCamera(Vector3 position)
    {
        mainCamera.transform.position = position;
        background.position = position + Vector3.forward * 10;
    }

    public void SetSizeCamera(float size)
    {
        mainCamera.orthographicSize = size;
        background.localScale = _originBackgroundScale * (size / _originCameraSize) * Vector3.one;
    }
}
