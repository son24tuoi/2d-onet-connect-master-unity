using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatCanvas : MonoBehaviour
{
    public void OnClickGPGSButton()
    {
        GPGSManager.Instance.ShowTestPopup();
    }
}
