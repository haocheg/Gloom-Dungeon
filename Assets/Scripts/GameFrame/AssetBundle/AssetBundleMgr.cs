using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Title:AB包管理器
/// Description:
/// </summary>
public class AssetBundleMgr : SingletonMono<AssetBundleMgr>
{
    /// <summary>
    /// AB包存放路径
    /// </summary>
    public static string PathUrl = Application.streamingAssetsPath + "/";

    private Dictionary<string, AssetBundle> bundles = new Dictionary<string, AssetBundle>();

    //主包
    private AssetBundle mainAB = null;
    //主包名
    public static string MainABName
    {
        get
        {
#if UNITY_STANDALONE_WIN
            return "Windows";
#elif UNITY_ANDROID
            return "Android";
#elif UNITY_IOS
            return "IOS";
#endif
        }
    }

    //依赖包信息
    private AssetBundleManifest manifest = null;

    public bool TryGetAssetBundle(string abName, out AssetBundle ab)
    {
        if (bundles.TryGetValue(abName, out ab))
            return true;

        ab = null;
        return false;
    }

    public void LoadAllDependencies(string abName)
    {
        string[] strs = manifest.GetAllDependencies(abName);
        if (strs != null)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (!TryGetAssetBundle(abName, out AssetBundle ab))
                {
                    ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                    bundles[strs[i]] = ab;
                }
            }
        }
    }

    private void LoadMainBundle()
    {
        if (mainAB == null || manifest == null)
        {
            mainAB = AssetBundle.LoadFromFile(PathUrl + MainABName);
            manifest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
    }

    public void LoadBundle(string abName, UnityAction<Object> callBack, bool isAsync = true)
    {
        StartCoroutine(AtLoadBundle(abName, callBack, isAsync));
    }
    private IEnumerator AtLoadBundle(string abName, UnityAction<Object> callBack, bool isAsync)
    {
        yield return LoadBundle(abName, isAsync);
        if (bundles[abName] != null)
        {
            callBack?.Invoke(bundles[abName]);
        }
    }

    /// <summary>
    /// AssetBundle资源加载
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callBack">加载后的回调</param>
    /// <param name="isAsync">是否异步加载</param>
    public void LoadAsset(string abName, string resName, UnityAction<Object> callBack, bool isAsync = true)
    {
        StartCoroutine(AtLoadAsset(abName, resName, isAsync, callBack));
    }

    private IEnumerator AtLoadAsset(string abName, string resName, bool isAsync, UnityAction<Object> callBack)
    {
        yield return LoadBundle(abName, isAsync);
        if (bundles[abName] != null)
        {
            if (isAsync)
            {
                AssetBundleRequest abrequest = bundles[abName].LoadAssetAsync(resName);
                yield return abrequest;
                callBack?.Invoke(abrequest.asset);
            }
            else
            {
                Object obj = bundles[abName].LoadAsset(resName);
                callBack?.Invoke(obj);
            }
        }
    }

    /// <summary>
    /// AssetBundle资源加载
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="type">资源类型</param>
    /// <param name="callBack">加载后的回调</param>
    /// <param name="isAsync">是否异步加载</param>
    public void LoadAsset(string abName, string resName, System.Type type, UnityAction<Object> callBack, bool isAsync = true)
    {
        StartCoroutine(AtLoadAsset(abName, resName, type, isAsync, callBack));
    }

    private IEnumerator AtLoadAsset(string abName, string resName, System.Type type, bool isAsync, UnityAction<Object> callBack)
    {
        yield return LoadBundle(abName, isAsync);

        if (bundles[abName] != null)
        {
            if (isAsync)
            {
                AssetBundleRequest abrequest = bundles[abName].LoadAssetAsync(resName, type);
                yield return abrequest;
                callBack?.Invoke(abrequest.asset);
            }
            else
            {
                Object obj = bundles[abName].LoadAsset(resName, type);
                callBack?.Invoke(obj);
            }
        }
    }

    /// <summary>
    /// AssetBundle资源加载
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="abName">AB包名</param>
    /// <param name="resName">资源名</param>
    /// <param name="callBack">加载后的回调</param>
    /// <param name="isAsync">是否异步加载</param>
    public void LoadAsset<T>(string abName, string resName, UnityAction<T> callBack, bool isAsync = true) where T : Object
    {
        StartCoroutine(AtLoadAsset(abName, resName, isAsync, callBack));
    }

    private IEnumerator AtLoadAsset<T>(string abName, string resName, bool isAsync, UnityAction<T> callBack) where T : Object
    {
        yield return LoadBundle(abName, isAsync);

        if (bundles[abName] != null)
        {
            if (isAsync)
            {
                AssetBundleRequest abrequest = bundles[abName].LoadAssetAsync<T>(resName);
                yield return abrequest;
                callBack?.Invoke(abrequest.asset as T);
            }
            else
            {
                T t = bundles[abName].LoadAsset<T>(resName);
                callBack?.Invoke(t);
            }
        }
    }

    private IEnumerator LoadBundle(string abName, bool isAsync)
    {
        LoadMainBundle();
        AssetBundleCreateRequest abr = null;
        string[] strs = manifest.GetAllDependencies(abName);
        if (strs != null)
        {
            for (int i = 0; i < strs.Length; i++)
            {
                if (!bundles.ContainsKey(strs[i]))
                {
                    if (!isAsync)
                    {
                        AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + strs[i]);
                        bundles[strs[i]] = ab;
                        continue;
                    }
                    bundles.Add(strs[i], null);
                    abr = AssetBundle.LoadFromFileAsync(PathUrl + strs[i]);
                    yield return abr;
                    bundles[strs[i]] = abr.assetBundle;
                }
                else
                {
                    while (bundles[strs[i]] == null)
                    {
                        yield return null;
                    }
                }
            }
        }

        if (!bundles.ContainsKey(abName))
        {
            if (!isAsync)
            {
                AssetBundle ab = AssetBundle.LoadFromFile(PathUrl + abName);
                if (ab != null)
                    bundles[abName] = ab;
                else
                    Debug.LogError($"不存在{abName}AB包");
            }
            else
            {
                bundles[abName] = null;
                abr = AssetBundle.LoadFromFileAsync(PathUrl + abName);
                yield return abr;
                if (abr.assetBundle != null)
                    bundles[abName] = abr.assetBundle;
                else
                    Debug.LogError($"不存在{abName}AB包");
            }
        }
        else
        {
            while (bundles[abName] == null)
            {
                yield return null;
            }
        }
    }

    /// <summary>
    /// 指定卸载AB包
    /// </summary>
    /// <param name="abName">AB包名</param>
    /// <param name="callBack">等待卸载后的回调</param>
    /// <param name="unloadAllLoadedObjects"></param>
    public void UnLoad(string abName, UnityAction<bool> callBack = null, bool unloadAllLoadedObjects = false)
    {
        if (bundles.ContainsKey(abName))
        {
            if (bundles[abName] == null)
            {
                Debug.Log("正在异步加载中无法卸载, 请等待加载完成");
                if (callBack != null)
                    StartCoroutine(WaitToUnload(abName, (v) =>
                    {
                        callBack?.Invoke(v);
                        if (v)
                        {
                            bundles[abName].Unload(unloadAllLoadedObjects);
                            bundles.Remove(abName);
                        }
                    }));
                return;
            }
            bundles[abName].Unload(unloadAllLoadedObjects);
            bundles.Remove(abName);
        }
    }

    private IEnumerator WaitToUnload(string abName, UnityAction<bool> callBack = null)
    {
        while (bundles[abName] == null)
        {
            yield return null;
        }
        callBack?.Invoke(true);
    }

    public void ClearAllAB(bool unloadAllObjects = false)
    {
        foreach (string str in bundles.Keys)
        {
            if (bundles[str] == null)
            {
                StartCoroutine(WaitToUnload(str, (v) =>
                {
                    if (v)
                    {
                        bundles[str].Unload(unloadAllObjects);
                        bundles.Remove(str);
                    }
                }));
            }
            else
            {
                bundles[str].Unload(unloadAllObjects);
                bundles.Remove(str);
            }
        }
        mainAB = null;
        manifest = null;
    }

}
