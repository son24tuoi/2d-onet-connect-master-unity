using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FirebaseUtilities
{
    public static string FolderPath = "Onet-Food/Resources";

    public static void SaveJsonData<T>(T generic, string fileName)
    {
        string json = JsonUtility.ToJson(generic);

        Debug.Log(json);

        string fullPath = Path.Combine(Application.dataPath, FolderPath, fileName);
        Debug.Log(fullPath);

        File.WriteAllText(fullPath, json);
    }
}
