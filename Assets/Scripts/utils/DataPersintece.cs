using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public static class DataPersistence
{
    static string path = "/recents";

    public static void saveRecentPath<T>(T data, string fileName)
    {
        string fullPath = Application.persistentDataPath + "/" + path + "/";
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        try
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(fullPath + fileName + ".json", jsonData);
            Debug.Log("Saved file!");
        }catch(Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public static T LoadRecentFile<T>(string fileName)
    {
        string fullPath = Application.persistentDataPath + "/" + path + "/" + fileName + ".json";
        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            T data = JsonUtility.FromJson<T>(jsonData);
            Debug.LogError("Loaded!");
            return data;
        }
        else
        {
            Debug.LogError("Don't loaded!");
            return default;
        }
    }
}
