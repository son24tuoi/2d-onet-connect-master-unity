using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DataController : MonoBehaviour
{
    private readonly string keyWord = "OnetConnect";
    private static readonly string fileName = "data.dat";
    private static string fullPath;

    [SerializeField] private bool encryptData = true;

    public Data data;

    private void Awake()
    {
        fullPath = Path.Combine(Application.persistentDataPath, fileName);

        Load();
    }

    public void Save()
    {
        string dataToStore = JsonUtility.ToJson(data);

        try
        {
            if (encryptData)
            {
                string encryptedJson = EncryptDecrypt(dataToStore);
                File.WriteAllText(fullPath, encryptedJson);
            }
            else
            {
                File.WriteAllText(fullPath, dataToStore);
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
            throw;
        }
    }

    public void Load()
    {
        data = new Data();

        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            data = JsonUtility.FromJson<Data>(encryptData ? EncryptDecrypt(jsonData) : jsonData);
        }
        else
        {
            Debug.Log("Save file does not exits!");
        }
    }

    public void DeleteData()
    {
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public void ClearData()
    {
        data = new Data();
        Save();
    }

    private string EncryptDecrypt(string data)
    {
        string result = string.Empty;

        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(DataController))]
    public class DataController_Inspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            DataController target = (DataController)base.target;

            GUILayout.Space(20f);

            if (GUILayout.Button("Save Data"))
            {
                target.Save();
            }

            if (GUILayout.Button("Load Data"))
            {
                target.Load();
            }

            if (GUILayout.Button("Clear Data"))
            {
                target.ClearData();
            }

            if (GUILayout.Button("Delete Data"))
            {
                target.DeleteData();
            }

            if (GUILayout.Button("Log Path Data"))
            {
                Debug.Log(Application.persistentDataPath);
            }
        }
    }
#endif
}
