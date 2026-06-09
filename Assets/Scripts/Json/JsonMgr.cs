using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
/// Json序列化模板
/// </summary>
[Serializable]
public class SheetData
{
    public string keyType;
    public Dictionary<string, object> dataDic = new Dictionary<string, object>();
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

    /// <summary>
    /// Json数据存储路径(可读写, 通常用于存储玩家数据)
    /// </summary>
    private static string SAVE_PATH = Application.persistentDataPath + "/JsonData/";

    /// <summary>
    /// Json数据存储路径(只读，通常用于Excel配置表)
    /// </summary>
    public static string DATA_JSON_PATH = Application.streamingAssetsPath + "/Json/";

    /// <summary>
    /// 用于存储所有Excel表数据的容器
    /// </summary>
    private Dictionary<string, object> tableDic = new Dictionary<string, object>();

    private JsonMgr()
    {
        JsonMapper.RegisterImporter<double, int>(Convert.ToInt32);
        JsonMapper.RegisterImporter<double, float>(Convert.ToSingle);
        JsonMapper.RegisterImporter<string, int>(Convert.ToInt32);
        JsonMapper.RegisterImporter<string, float>(Convert.ToSingle);
        JsonMapper.RegisterImporter<string, bool>(Convert.ToBoolean);
    }

    /// <summary>
    /// 存储Json数据 序列化
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public void SaveData(object data, string fileName, JsonType type = JsonType.LitJson)
    {
        if (!Directory.Exists(SAVE_PATH))
            Directory.CreateDirectory(SAVE_PATH);
        string path = SAVE_PATH + fileName + ".json";
        string jsonStr = "";
        switch (type)
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

    /// <summary>
    /// 读取指定文件中的 Json数据 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public T LoadData<T>(string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        string path = SAVE_PATH + fileName + ".json";
        if (!File.Exists(path))
            path = DATA_JSON_PATH + fileName + ".json";
        if (!File.Exists(path))
            return new T();

        string jsonStr = File.ReadAllText(path);
        T data = default(T);
        switch (type)
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

    /// <summary>
    /// 加载Excel表的Json数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="type"></param>
    public void LoadTable<T, K>(JsonType type = JsonType.LitJson)
        where T : class, new()
        where K : class, new()
    {
        string path = DATA_JSON_PATH + typeof(K).Name + ".json";
        if (!File.Exists(path))
        {
            Debug.LogError($"文件{path}不存在");
            return;
        }

        string jsonStr = File.ReadAllText(path);
        JsonData jsonData = JsonMapper.ToObject(jsonStr);
        string keyType = jsonData["keyType"].ToString();
        JsonData dataDicNode = jsonData["dataDic"];

        Type containerType = typeof(T);
        object containerObj = Activator.CreateInstance(containerType);
        object containerDic = containerType.GetField("dataDic").GetValue(containerObj);
        MethodInfo mInfo = containerDic.GetType().GetMethod("Add");
        IDictionary dict = dataDicNode;
        foreach (string strKey in dict.Keys)
        {
            object key = ParseCellValue(keyType, strKey);
            string valueStr = dataDicNode[strKey].ToJson();
            K value = JsonMapper.ToObject<K>(valueStr);
            mInfo.Invoke(containerDic, new object[] { key, value });
        }
        tableDic.Add(typeof(T).Name, containerObj);
    }

    /// <summary>
    /// 获得一张表的数据
    /// </summary>
    /// <typeparam name="T">容器类</typeparam>
    /// <returns></returns>
    public T GetTable<T>() where T : class
    {
        string tableName = typeof(T).Name;
        if (tableDic.ContainsKey(tableName))
        {
            return (T)tableDic[tableName];
        }
        return null;
    }

    /// <summary>
    /// 把任意对象追加到 JSON 数组文件，并带换行
    /// </summary>
    /// <param name="fileName">文件绝对路径</param>
    /// <param name="newItem">要追加的元素</param>
    public void AddNewData(string fileName, object newItem)
    {
        string path = SAVE_PATH + fileName + ".json";
        bool firstTime = !File.Exists(path);

        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
        {
            // 1. 如果是第一次，直接写 [ + 回车 + 内容 + 回车 + ]
            if (firstTime)
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine("[");                       // 单独一行 [
                    sw.WriteLine(JsonMapper.ToJson(newItem)); // 一条记录
                    sw.Write("]");                            // 单独一行 ]
                }
                return;
            }

            // 2. 以后每次：删掉最后的 ]，补上 , + 回车 + 新记录 + 回车 + ]
            fs.Seek(-1, SeekOrigin.End);   // 回退一个字节，正好盖住 ]
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(",");                           // 补逗号
                sw.WriteLine();                          // 换行
                sw.WriteLine(JsonMapper.ToJson(newItem)); // 新记录
                sw.Write("]");                            // 补上结尾
            }
        }
    }

    /// <summary>
    /// 存储数据List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    public void SaveAllData<T>(List<T> data, string fileName)
    {
        string path = SAVE_PATH + fileName + ".json";
        StringBuilder sb = new StringBuilder();
        sb.Append('[').Append('\n');
        sb.Append(JsonMapper.ToJson(data[0])).Append('\n');
        for (int i = 1; i < data.Count; i++)
        {
            sb.Append(",\n");
            sb.Append(JsonMapper.ToJson(data[0])).Append('\n');
        }
        sb.Append("]");
        File.WriteAllText(path, sb.ToString());
    }

    /// <summary>
    /// 在原List上添加数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="type"></param>
    public void AddNewData<T>(T data, string fileName, JsonType type = JsonType.LitJson) where T : new()
    {
        List<T> datas;
        string path = SAVE_PATH + fileName + ".json";
        if (File.Exists(path))
        {
            datas = LoadData<List<T>>(fileName);
        }
        else
        {
            datas = new List<T>();
        }

        if (datas == null)
            datas = new List<T>();
        datas.Add(data);
        SaveData(datas, fileName);
    }

    private static object ParseCellValue(string type, object value)
    {
        try
        {
            if (value == null)
            {
                return GetDefaultValue(type);
            }

            switch (type)
            {
                case "int":
                    return Convert.ToInt32(value);
                case "float":
                    return Convert.ToSingle(value);
                case "bool":
                    return Convert.ToBoolean(value);
                case "string":
                    return value.ToString();
                default:
                    return value;
            }
        }
        catch
        {
            Debug.LogError($"{type}类型转换失败");
            return GetDefaultValue(type);
        }
    }

    private static object GetDefaultValue(string type)
    {
        switch (type)
        {
            case "int": return 0;
            case "float": return 0f;
            case "bool": return false;
            case "string": return "";
            default: return null;
        }
    }

}
