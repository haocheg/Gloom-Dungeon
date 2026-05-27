using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class AssetBundleResMgr : Singleton<AssetBundleResMgr>
{
    /// <summary>
    /// 如果为true则通过EditorResMgr加载，false则通过AssetBundleResMgr加载
    /// </summary>
    public bool isDebug = true;

    private AssetBundleResMgr() { }

    public void LoadAsset<T>(string abName, string resName, string suffixName = null, UnityAction<T> callBack = null, bool isAsync = true) where T : Object
    {
        LoadAssetByMode(abName, resName, suffixName, callBack, isAsync, isDebug);
    }

    public void LoadEditorAsset<T>(string abName, string resName, string suffixName = null, UnityAction<T> callBack = null, bool isAsync = true) where T : Object
    {
        LoadAssetByMode(abName, resName, suffixName, callBack, isAsync, true);
    }

    public void LoadBundleAsset<T>(string abName, string resName, UnityAction<T> callBack = null, bool isAsync = true) where T : Object
    {
        LoadAssetByMode(abName, resName, null, callBack, isAsync, false);
    }

    private void LoadAssetByMode<T>(string abName, string resName, string suffixName, UnityAction<T> callBack, bool isAsync, bool useEditorResources) where T : Object
    {
#if UNITY_EDITOR
        if (useEditorResources)
        {
            // 注意：路径规则是包名就是文件夹名(Assets/Editor/ArtRes下)，资源名就是资源名
            T t = EditorResManager.Instance.LoadEditorRes<T>($"{abName}/{resName}", suffixName);
            callBack?.Invoke(t);
        }
        else
        {
            AssetBundleMgr.Instance.LoadAsset(abName, resName, callBack, isAsync);
        }
#else
        AssetBundleMgr.Instance.LoadAsset(abName, resName, callBack, isAsync);
#endif
    }
}