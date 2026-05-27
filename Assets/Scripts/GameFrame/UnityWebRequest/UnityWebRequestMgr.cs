using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class UnityWebRequestMgr : SingletonMono<UnityWebRequestMgr>
{
    /// <summary>
    /// UnityWebRequest加载资源
    /// </summary>
    /// <typeparam name="T">资源类型，只能是：string, byte[], Texture, AssetBundle.不支持其他类型</typeparam>
    /// <param name="path">路径</param>
    /// <param name="callBack">加载后回调</param>
    /// <param name="failCallBack">加载失败后的回调</param>
    public void LoadResources<T>(string path, UnityAction<T> callBack, UnityAction failCallBack) where T : class
    {
        StartCoroutine(AtLoadResources(path, callBack, failCallBack));
    }

    private IEnumerator AtLoadResources<T>(string path, UnityAction<T> callBack, UnityAction failCallBack) where T : class
    {
        Type type = typeof(T);
        UnityWebRequest req = null;
        if (type == typeof(string) ||
            type == typeof(byte[]))
        {
            req = UnityWebRequest.Get(path);
        }
        else if (type == typeof(Texture))
        {
            req = UnityWebRequestTexture.GetTexture(path);
        }
        else if (type == typeof(AssetBundle))
        {
            req = UnityWebRequestAssetBundle.GetAssetBundle(path);
        }
        else
        {
            failCallBack?.Invoke();
            yield break;
        }
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            if (type == typeof(string))
            {
                callBack?.Invoke(req.downloadHandler.text as T);
            }
            else if (type == typeof(byte[]))
            {
                callBack?.Invoke(req.downloadHandler.data as T);
            }
            else if (type == typeof(Texture))
            {
                callBack?.Invoke(DownloadHandlerTexture.GetContent(req) as T);
            }
            else if (type == typeof(AssetBundle))
            {
                callBack?.Invoke(DownloadHandlerAssetBundle.GetContent(req) as T);
            }
        }
        else
        {
            failCallBack?.Invoke();
        }
        req.Dispose();
    }

}
