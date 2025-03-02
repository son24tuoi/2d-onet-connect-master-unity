using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewController : MonoBehaviour
{
    [SerializeField] private UniWebView uniWebViewPrefab;
    [SerializeField] private RectTransform refetrenceRectTransform;

    public static WebViewController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Load(string url)
    {
        UniWebView uniWebView = Instantiate(uniWebViewPrefab, transform);
        uniWebView.ReferenceRectTransform = refetrenceRectTransform;

        uniWebView.Frame = new Rect(0, 0, Screen.width, Screen.height);
        uniWebView.Load(url);
        uniWebView.Show();
    }
}
