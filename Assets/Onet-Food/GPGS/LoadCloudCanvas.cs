using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCloudCanvas : Popup
{
    public void OnClickExitButton()
    {
        base.ExitAndRemove();
    }

    public void OnClickYesButton()
    {
        base.ExitAndRemove();
        SceneManager.LoadScene(0);
    }
}
