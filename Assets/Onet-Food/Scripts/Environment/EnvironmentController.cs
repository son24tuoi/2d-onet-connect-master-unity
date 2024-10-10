using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public static EnvironmentController Instance { get; private set; }

    public Camera mainCamera;
    public Background.Background background;

    private float m_originCameraSize;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_originCameraSize = mainCamera.orthographicSize;
        background.Init();
    }

    private void FixedUpdate()
    {
        background.CheckChangeBackground();
    }

    public void AdjustCamera(Vector3 position, float size)
    {
        SetPositionCamera(position);
        SetSizeCamera(size);
    }

    public void SetPositionCamera(Vector3 position)
    {
        mainCamera.transform.position = position;
        background.transform.position = position + Vector3.forward * 10;
    }

    public void SetSizeCamera(float size)
    {
        mainCamera.orthographicSize = size;
        background.transform.localScale = Vector3.one * (size / m_originCameraSize);
    }
}
