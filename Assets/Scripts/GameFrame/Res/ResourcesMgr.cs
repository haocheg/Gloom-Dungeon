using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ResourcesInfoBase
{
    //引用计数
    public int refCount;
}

public class ResourcesInfo<T> : ResourcesInfoBase
{
    public T asset;
    public UnityAction<T> callBack;
    public Coroutine coroutine;
    //是否需要异步删除
    public bool needAsyncDelete;
    public bool needDel;

    public void AddRefCount()
    {
        ++refCount;
    }

    public void SubRefCount()
    {
        --refCount;
        if (refCount < 0)
            Debug.LogError("引用计数小于0了, 请检查使用");
        refCount = Mathf.Max(0, refCount);
    }
}


/// <summary>
/// Title:Resources资源加载管理器类
/// Description:
/// </summary>
public class ResourcesMgr : Singleton<ResourcesMgr>
{
    private Dictionary<string, ResourcesInfoBase> resourcesDic = new();

    private ResourcesMgr() { }

    /// <summary>
    /// 同步加载资源方法
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径(Resources下)</param>
    /// <returns></returns>
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        string resName = path + "_" + typeof(T).Name;
        ResourcesInfo<T> info;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            info = (ResourcesInfo<T>)infoBase;
            info.AddRefCount();
            if (info.coroutine != null)
                MonoMgr.Instance.StopCoroutine(info.coroutine);
            if (info.asset == null)
            {
                info.asset = Resources.Load<T>(path);
                info.callBack?.Invoke(info.asset);
                info.callBack = null;
                info.coroutine = null;
            }
            return info.asset;
        }
        else
        {
            info = new ResourcesInfo<T>();
            info.asset = Resources.Load<T>(path);
            info.AddRefCount();
            resourcesDic[resName] = info;
            return info.asset;
        }
    }

    /// <summary>
    /// 异步加载资源方法
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径(Resources下)</param>
    /// <param name="callBack">加载结束后的回调</param>
    public void LoadAsync<T>(string path, UnityAction<T> callBack) where T : UnityEngine.Object
    {
        string resName = path + "_" + typeof(T).Name;
        ResourcesInfo<T> info;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            info = (ResourcesInfo<T>)infoBase;
            info.AddRefCount();
            if (info.asset == null)
                info.callBack += callBack;
            else
                callBack?.Invoke(info.asset);
        }
        else
        {
            info = new ResourcesInfo<T>();
            info.AddRefCount();
            resourcesDic.Add(resName, info);
            info.callBack += callBack;
            info.coroutine = MonoMgr.Instance.StartCoroutine(AtLoadAsync<T>(path, callBack));
        }

    }

    private IEnumerator AtLoadAsync<T>(string path, UnityAction<T> callBack) where T : UnityEngine.Object
    {
        ResourceRequest rq = Resources.LoadAsync<T>(path);
        yield return rq;

        string resName = path + "_" + typeof(T).Name;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            ResourcesInfo<T> info = infoBase as ResourcesInfo<T>;
            info.asset = rq.asset as T;
            if (info.needDel == true)
            {
                info.callBack -= callBack;
                UnloadAsset<T>(path);
            }
            else
            {
                info.callBack?.Invoke(info.asset);
                info.callBack = null;
                info.coroutine = null;
            }
        }

    }

    /// <summary>
    /// 异步加载资源方法
    /// </summary>
    /// <param name="path">资源路径(Resources下)</param>
    /// <param name="callBack">加载结束后的回调</param>
    [Obsolete("注意: 不建议使用Type方式加载, 如果一定要用, 不能和泛型一起混合加载同类型同名资源")]
    public void LoadAsync(string path, Type type, UnityAction<UnityEngine.Object> callBack)
    {
        string resName = path + "_" + type.Name;
        ResourcesInfo<UnityEngine.Object> info;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            info = (ResourcesInfo<UnityEngine.Object>)infoBase;
            info.AddRefCount();
            if (info.asset == null)
                info.callBack += callBack;
            else
                callBack?.Invoke(info.asset);
        }
        else
        {
            info = new ResourcesInfo<UnityEngine.Object>();
            info.AddRefCount();
            resourcesDic.Add(resName, info);
            info.callBack += callBack;
            info.coroutine = MonoMgr.Instance.StartCoroutine(AtLoadAsync(path, type, callBack));
        }
    }

    private IEnumerator AtLoadAsync(string path, Type type, UnityAction<UnityEngine.Object> callBack)
    {
        ResourceRequest rq = Resources.LoadAsync(path, type);
        yield return rq;

        string resName = path + "_" + type.Name;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            ResourcesInfo<UnityEngine.Object> info = infoBase as ResourcesInfo<UnityEngine.Object>;
            info.asset = rq.asset;
            if (info.needDel == true)
            {
                info.callBack -= callBack;
                UnloadAsset(path, type);
            }
            else
            {
                info.callBack?.Invoke(info.asset);
                info.callBack = null;
                info.coroutine = null;
            }
        }
    }

    /// <summary>
    /// 卸载指定资源
    /// </summary>
    /// <typeparam name="T">资源类型</typeparam>
    /// <param name="path">资源路径</param>
    /// <param name="needAsync">是否需要异步删除</param>
    /// <param name="needDel">异步加载未完成的资源是否要删除</param>
    public void UnloadAsset<T>(string path, bool needAsync = false, bool needDel = false)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            ResourcesInfo<T> info = infoBase as ResourcesInfo<T>;
            info.SubRefCount();
            info.needAsyncDelete = needAsync;
            if (info.asset != null && info.refCount == 0)
            {
                resourcesDic.Remove(resName);
                if (needAsync)
                {
                    MonoMgr.Instance.StartCoroutine(UnloadAssetAsync(info.asset as UnityEngine.Object));
                    return;
                }
                Resources.UnloadAsset(info.asset as UnityEngine.Object);
            }
            else if (info.asset == null)
            {
                info.needDel = needDel;
            }
        }
    }

    /// <summary>
    /// 卸载指定资源
    /// </summary>
    /// <param name="path">资源路径</param>
    /// <param name="type">资源类型</param>
    /// <param name="needAsync">是否需要异步删除</param>
    /// <param name="needDel">异步加载未完成的资源是否要删除</param>
    public void UnloadAsset(string path, Type type, bool needAsync = false, bool needDel = false)
    {
        string resName = path + "_" + type.Name;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase infoBase))
        {
            ResourcesInfo<UnityEngine.Object> info = infoBase as ResourcesInfo<UnityEngine.Object>;
            info.SubRefCount();
            info.needAsyncDelete = needAsync;
            if (info.asset != null && info.refCount == 0)
            {
                resourcesDic.Remove(resName);
                if (needAsync)
                {
                    MonoMgr.Instance.StartCoroutine(UnloadAssetAsync(info.asset));
                    return;
                }
                Resources.UnloadAsset(info.asset);
            }
            else if (info.asset == null)
            {
                info.needDel = needDel;
            }
        }
    }

    private IEnumerator UnloadAssetAsync(UnityEngine.Object obj)
    {
        Resources.UnloadAsset(obj);
        yield return null;
    }

    /// <summary>
    /// 异步卸载没用的Resources资源
    /// </summary>
    /// <param name="callBack">卸载后的回调</param>
    public void UnloadUnusedAssets(UnityAction callBack = null)
    {
        MonoMgr.Instance.StartCoroutine(AtUnloadUnusedAssets(callBack));
    }

    private IEnumerator AtUnloadUnusedAssets(UnityAction callBack)
    {
        List<string> list = new List<string>();
        foreach (string path in resourcesDic.Keys)
        {
            if (resourcesDic[path].refCount == 0)
                list.Add(path);
        }
        foreach (string path in list)
        {
            resourcesDic.Remove(path);
        }
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        callBack?.Invoke();
    }

    public int GetRefCount<T>(string path)
    {
        string resName = path + "_" + typeof(T).Name;
        if (resourcesDic.TryGetValue(resName, out ResourcesInfoBase info))
        {
            return (info as ResourcesInfo<T>).refCount;
        }
        return 0;
    }

    public void ClearDic(UnityAction callBack = null)
    {
        MonoMgr.Instance.StartCoroutine(AtClearDic(callBack));
    }

    private IEnumerator AtClearDic(UnityAction callBack)
    {
        resourcesDic.Clear();
        AsyncOperation ao = Resources.UnloadUnusedAssets();
        yield return ao;
        callBack?.Invoke();
    }

}
