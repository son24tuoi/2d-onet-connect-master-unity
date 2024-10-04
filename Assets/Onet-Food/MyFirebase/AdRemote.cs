using System.Collections;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using UnityEngine;

public class AdRemote : RemoteBehaviour
{
    public AdProfileSO adProfileSO;

    public override void GetConfigData(FirebaseRemoteConfig remoteConfig)
    {
        string configData = remoteConfig.GetValue("ad").StringValue;

        adProfileSO.Remote(configData);
    }
}
