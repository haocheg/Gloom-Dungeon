using LitJson;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

/*
*Title:
*Description:
*
*/

/// <summary>
/// Json序列化和反序列化方案
/// </summary>
public enum JsonType
{
    JsonUtility,
    LitJson
}

/// <summary>
/// Json数据管理类
/// </summary>
public class JsonMgr
{
    private static JsonMgr instance = new JsonMgr();
    public static JsonMgr Instance
    {
        get { return instance; }
    }

    private JsonMgr()
    {

    }

    public void SaveData(object data, string fileName, JsonType jsonType = JsonType.LitJson)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".json";
        Debug.Log(path);
        string jsonStr = "";
        switch (jsonType)
        {
            case JsonType.JsonUtility:
                jsonStr = JsonUtility.ToJson(data);
                break;
            case JsonType.LitJson:
                jsonStr = JsonMapper.ToJson(data);
                break;
        }
        File.WriteAllText(path, jsonStr);
    }

    public T LoadData<T>(string fileName, JsonType jsonType = JsonType.LitJson) where T : new()
    {
        string path = Application.streamingAssetsPath + "/" + fileName + ".json";
        if (!File.Exists(path))
            path = Application.persistentDataPath + "/" + fileName + ".json";
        if (!File.Exists(path)) return new T();
        Debug.Log(path);
        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (jsonType)
        {
            case JsonType.JsonUtility:
                data = JsonUtility.FromJson<T>(jsonStr);
                break;
            case JsonType.LitJson:
                data = JsonMapper.ToObject<T>(jsonStr);
                break;
        }
        return data;
    }

}
