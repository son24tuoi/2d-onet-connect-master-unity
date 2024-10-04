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
        mainCamera.transform.position = position;
        mainCamera.orthographicSize = size;

        background.transform.position = position + Vector3.forward * 10;
        background.transform.localScale = Vector3.one * (size / m_originCameraSize);
    }
}
