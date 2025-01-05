using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScene : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Slider slider;

    [Header("Config")]
    [SerializeField] private SceneType sceneType;

    public static event Action OnLoadEvent;

    public enum SceneType
    {
        LoadingScene,
        MainScene
    }

    private void Start()
    {
        StartCoroutine(IELoadSceneAsync((int)sceneType));
    }

    private IEnumerator IELoadSceneAsync(int sceneId)
    {
        OnLoadEvent?.Invoke();

        float progressValue = 0f;
        slider.value = progressValue;

        float wait = Time.time + 3f;

        while (Time.time < wait)
        {
            yield return null;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progressValue;

            yield return null;
        }
    }
}
