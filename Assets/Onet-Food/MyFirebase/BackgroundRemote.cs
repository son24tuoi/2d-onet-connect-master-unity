using Background;
using Firebase.RemoteConfig;
using UnityEngine;

public class BackgroundRemote : RemoteBehaviour
{
    public BackgroundProfileSO backgroundProflieSO;

    public override void GetConfigData(FirebaseRemoteConfig remoteConfig)
    {
        string configData = remoteConfig.GetValue("background").StringValue;

        backgroundProflieSO.Remote(configData);
    }
}