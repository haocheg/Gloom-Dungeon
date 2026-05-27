using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Title：编辑器资源管理器
/// Description：专用于开发时的资源管理器，避免发布时被资源打包，注意：只有开发时才可用
/// </summary>
public class EditorResManager : Singleton<EditorResManager>
{
    //资源存放路径
    private static string rootPath = "Assets/Editor/ArtRes/";

    private EditorResManager() { }

    /// <summary>
    /// 编辑器加载方法
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="suffixName">必要时请输入后缀名</param>
    /// <returns></returns>
    public T LoadEditorRes<T>(string path, string suffixName = null) where T : Object
    {
#if UNITY_EDITOR
        if (suffixName != null)
            return AssetDatabase.LoadAssetAtPath<T>(rootPath + path + suffixName);

        if (typeof(T) == typeof(GameObject))
            path += ".prefab";
        else if (typeof(T) == typeof(Scene))
            path += ".unity";
        else if (typeof(T) == typeof(Material))
            path += ".mat";
        else if (typeof(T) == typeof(AudioClip))
        {
            string[] strs = new string[] { ".mp3", ".ogg", ".wav", ".aiff" };
            for (int i = 0; i < strs.Length; i++)
            {
                T t = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + strs[i]);
                if (t != null)
                    return t;
            }
        }
        else if (typeof(T) == typeof(Texture))
        {
            string[] strs = new string[] { ".png", ".jpg", ".tga", ".psd", "bmp", ".gif" };
            for (int i = 0; i < strs.Length; i++)
            {
                T t = AssetDatabase.LoadAssetAtPath<T>(rootPath + path + strs[i]);
                if (t != null)
                    return t;
            }
        }
        return AssetDatabase.LoadAssetAtPath<T>(rootPath + path);
#else
        return null;
#endif
    }

    /// <summary>
    /// 加载图集资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="spriteName"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string path, string spriteName)
    {
#if UNITY_EDITOR
        Object[] objs = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        foreach (Object obj in objs)
        {
            if (obj.name == spriteName)
                return obj as Sprite;
        }
        Debug.LogError("未找到该图集");
        return null;
#else
        return null;
#endif
    }

    /// <summary>
    /// 加载图集中的所有子对象
    /// </summary>
    /// <param name="path"></param>
    /// <returns>返回一个(string, Sprite)字典</returns>
    public Dictionary<string, Sprite> LoadSprite(string path)
    {
#if UNITY_EDITOR
        Object[] objs = AssetDatabase.LoadAllAssetRepresentationsAtPath(rootPath + path);
        Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();
        foreach (Object item in objs)
        {
            spriteDic[item.name] = item as Sprite;
        }
        return spriteDic;
#else
        return null;
#endif
    }

}
