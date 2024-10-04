using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VersionText : MonoBehaviour
{
    public TextMeshProUGUI versionText;

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        versionText.SetText("Version:" + Application.version);
    }
}
