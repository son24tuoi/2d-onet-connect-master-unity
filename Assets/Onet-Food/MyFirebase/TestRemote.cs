using System.Collections;
using System.Collections.Generic;
using Firebase.RemoteConfig;
using UnityEngine;

public class TestRemote : RemoteBehaviour
{
    public TestProfileSO testProfileSO;

    public override void GetConfigData(FirebaseRemoteConfig remoteConfig)
    {
        string configData = remoteConfig.GetValue("test").StringValue;

        testProfileSO.Remote(configData);
    }
}
