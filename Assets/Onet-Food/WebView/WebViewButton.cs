using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebViewButton : MonoBehaviour
{
    public void OnClickButton(string url)
    {
        WebViewController.Instance.Load(url);
    }
}
