using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Title:缓存池管理器
/// Description:
/// </summary>
public class PoolManager : Singleton<PoolManager>
{
    private readonly Dictionary<string, PoolObject> poolDic = new Dictionary<string, PoolObject>();
    private readonly Dictionary<string, PoolDataBase> poolDataDic = new Dictionary<string, PoolDataBase>();
    private IGameAssetLoader assetLoader = GameAssetLoader.Instance;
    private GameObject poolRootObj;

    /// <summary>
    /// 默认60秒会清除缓存池中的对象
    /// </summary>
    public float cleanUpTimeOut = 60f;
    public static bool needLayout = true;
    public static bool needLimit = false;

    private PoolManager() { }

    public void SetAssetLoader(IGameAssetLoader loader)
    {
        assetLoader = loader ?? GameAssetLoader.Instance;
    }

    /// <summary>
    /// 获取对象
    /// </summary>
    /// <param name="path">对象存储路径(文件夹名)</param>
    /// <param name="name">缓存池的名字同时也是对象的名字</param>
    /// <returns>从缓冲池取出的对象</returns>
    public GameObject GetGameObject(string path, string name, E_LoadWay loadWay, bool isAsync = false, UnityAction<GameObject> callBack = null)
    {
        EnsurePoolRoot();

        if (TryPop(name, callBack, out GameObject pooledObject))
            return pooledObject;

        GameObject createdObject = null;
        LoadPrefab(path, name, loadWay, isAsync, (prefab) =>
        {
            createdObject = CreateAndRegisterObject(name, prefab);
            callBack?.Invoke(createdObject);
        });

        return createdObject;
    }

    private void EnsurePoolRoot()
    {
        if (poolRootObj == null && needLayout)
            poolRootObj = new GameObject("Pool");
    }

    private bool TryPop(string name, UnityAction<GameObject> callBack, out GameObject go)
    {
        go = null;
        if (!poolDic.TryGetValue(name, out PoolObject pool) || pool.Count == 0)
            return false;

        go = pool.Pop();
        callBack?.Invoke(go);
        return true;
    }

    private void LoadPrefab(string path, string name, E_LoadWay loadWay, bool isAsync, UnityAction<GameObject> callback)
    {
        switch (loadWay)
        {
            case E_LoadWay.ResourcesLoad:
                LoadFromResources(BuildResourcesPath(path, name), isAsync, callback);
                break;
            case E_LoadWay.EditorResources:
                if (isAsync)
                {
                    Debug.LogError("该方式不能异步加载");
                    callback?.Invoke(null);
                    return;
                }
                assetLoader.LoadPackagedAsset(path, name, ".prefab", AssetPackageMode.EditorResources, callback, false);
                break;
            case E_LoadWay.AssetBundle:
                assetLoader.LoadPackagedAsset(path, name, null, AssetPackageMode.AssetBundle, callback, isAsync);
                break;
            default:
                LoadFromResources(path, isAsync, callback);
                break;
        }
    }

    private void LoadFromResources(string resourcesPath, bool isAsync, UnityAction<GameObject> callback)
    {
        if (isAsync)
        {
            assetLoader.LoadResourcesAsync(resourcesPath, callback);
            return;
        }

        callback?.Invoke(assetLoader.LoadResources<GameObject>(resourcesPath));
    }

    private GameObject CreateAndRegisterObject(string name, GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError($"Pool object not found: {name}");
            return null;
        }

        GameObject go = GameObject.Instantiate(prefab);
        go.name = name;
        if (!poolDic.ContainsKey(name))
        {
            poolDic.Add(name, new PoolObject(name, null, go));
        }
        else if (!needLimit || !poolDic[name].isLimit)
        {
            poolDic[name].PushUsedList(go);
        }
        else
        {
            GameObject.Destroy(go);
            go = null;
        }

        return go;
    }

    private string BuildResourcesPath(string path, string name)
    {
        return string.IsNullOrEmpty(path) ? name : path + "/" + name;
    }

    /// <summary>
    /// 往缓冲池中存放对象
    /// </summary>
    /// <param name="go">存放的对象</param>
    /// <param name="needAutoCleanUp">是否需要自动清除</param>
    public void PushGameObject(GameObject go, bool needAutoCleanUp = false)
    {
        if (go == null)
            return;

        string name = go.name;
        if (poolDic.ContainsKey(name))
        {
            poolDic[name].Push(go, needAutoCleanUp);
            return;
        }

        poolDic[name] = new PoolObject(name, go);
    }

    public void PushGameObject(GameObject go, bool needAutoCleanUp, float waitTimeToPush)
    {
        MonoMgr.Instance.StartCoroutine(WaitTimeToPushGO(go, needAutoCleanUp, waitTimeToPush));
    }

    private IEnumerator WaitTimeToPushGO(GameObject go, bool needAutoCleanUp, float waitTimeToPush)
    {
        yield return new WaitForSeconds(waitTimeToPush);
        PushGameObject(go, needAutoCleanUp);
    }

    /// <summary>
    /// 获取自定义的数据结构类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPoolData<T>(string nameSpace = "") where T : class, IPoolData, new()
    {
        string name = BuildPoolDataName<T>(nameSpace);
        if (!poolDataDic.TryGetValue(name, out PoolDataBase dataBase))
            return new T();

        PoolData<T> data = dataBase as PoolData<T>;
        return data == null ? new T() : data.Dequeue();
    }

    public void PushPoolData<T>(T t, string nameSpace = "") where T : class, IPoolData, new()
    {
        if (t == null)
        {
            Debug.LogError("数据为空，请检查代码合理性!");
            return;
        }

        string name = BuildPoolDataName<T>(nameSpace);
        if (!poolDataDic.TryGetValue(name, out PoolDataBase dataBase))
        {
            dataBase = new PoolData<T>();
            poolDataDic.Add(name, dataBase);
        }

        PoolData<T> data = dataBase as PoolData<T>;
        data?.Enqueue(t);
    }

    private string BuildPoolDataName<T>(string nameSpace)
    {
        return nameSpace + "_" + typeof(T).Name;
    }

    public void ClearAllPool()
    {
        foreach (PoolObject poolObject in poolDic.Values)
        {
            poolObject.Clear();
        }
        poolDic.Clear();
        poolDataDic.Clear();
        if (poolRootObj != null)
            GameObject.Destroy(poolRootObj);
        poolRootObj = null;
    }

    public void ClearPool(string name)
    {
        if (!poolDic.ContainsKey(name))
            return;

        poolDic[name].Clear();
        poolDic.Remove(name);
    }
}

public abstract class PoolDataBase
{
}

public class PoolData<T> : PoolDataBase where T : class, IPoolData, new()
{
    public Queue<T> poolDatas = new Queue<T>();
    public float cleanUpTimeOut = 60f;
    public int Count { get { return poolDatas.Count; } }
    private Coroutine cleanUpCoroutine;

    public T Dequeue()
    {
        T t = Count > 0 ? poolDatas.Dequeue() : new T();
        if (t.needAutoCleanUp)
            StopCleanUp();
        return t;
    }

    public void Enqueue(T t)
    {
        t.ResetInfo();
        poolDatas.Enqueue(t);
        if (t.needAutoCleanUp)
            StartCleanUp();
    }

    public void StartCleanUp()
    {
        StopCleanUp();
        cleanUpCoroutine = MonoMgr.Instance.StartCoroutine(CleanUpTimer());
    }

    private IEnumerator CleanUpTimer()
    {
        while (poolDatas.Count > 0)
        {
            yield return new WaitForSeconds(cleanUpTimeOut);
            poolDatas.Dequeue();
        }
        cleanUpCoroutine = null;
    }

    public void StopCleanUp()
    {
        if (cleanUpCoroutine != null)
            MonoMgr.Instance.StopCoroutine(cleanUpCoroutine);
    }
}

public interface IPoolData
{
    bool needAutoCleanUp { get; }

    /// <summary>
    /// 重置数据的方法
    /// </summary>
    void ResetInfo();
}

/// <summary>
/// 缓存池对象
/// </summary>
public class PoolObject
{
    private readonly List<GameObject> dataList = new List<GameObject>();
    private readonly List<GameObject> usedDataList = new List<GameObject>();
    private readonly Dictionary<GameObject, Coroutine> cleanUpCorountines = new Dictionary<GameObject, Coroutine>();
    private GameObject rootObj;
    private int maxNum = 100;

    public bool needAutoClearUp = false;
    public int Count { get { return dataList.Count; } }
    public int UsedCount { get { return usedDataList.Count; } }
    public bool isLimit { get { return UsedCount >= maxNum; } }

    public PoolObject(string name, GameObject go, GameObject usedObj = null)
    {
        if (PoolManager.needLayout)
            rootObj = new GameObject(name);

        if (usedObj != null)
        {
            PushUsedList(usedObj);
            PoolGameObject poolGO = usedObj.GetComponent<PoolGameObject>();
            if (poolGO == null)
            {
                Debug.LogError("需要挂载PoolGameObject脚本");
                return;
            }
            maxNum = poolGO.maxNum;
            return;
        }

        if (go != null)
            Push(go, needAutoClearUp);
    }

    public GameObject Pop()
    {
        GameObject go;
        if (dataList.Count > 0)
        {
            go = dataList[0];
            dataList.RemoveAt(0);
            usedDataList.Add(go);
        }
        else if (usedDataList.Count > 0)
        {
            go = usedDataList[0];
            usedDataList.RemoveAt(0);
            usedDataList.Add(go);
        }
        else
        {
            return null;
        }

        go.SetActive(true);
        if (PoolManager.needLayout)
            go.transform.parent = null;
        if (needAutoClearUp)
            StopCleanUpCoroutine(go);
        return go;
    }

    public void Push(GameObject go, bool needToCleanUp)
    {
        if (go == null)
            return;

        needAutoClearUp = needToCleanUp;
        go.SetActive(false);
        if (PoolManager.needLayout)
            go.transform.parent = rootObj.transform;
        usedDataList.Remove(go);
        if (!dataList.Contains(go))
            dataList.Add(go);
        if (needAutoClearUp)
            StartCleanupCoroutine(go);
    }

    public void PushUsedList(GameObject go)
    {
        if (go == null || usedDataList.Contains(go))
            return;

        dataList.Remove(go);
        usedDataList.Add(go);
    }

    public void Clear()
    {
        foreach (Coroutine coroutine in cleanUpCorountines.Values)
        {
            if (coroutine != null)
                MonoMgr.Instance.StopCoroutine(coroutine);
        }
        cleanUpCorountines.Clear();

        foreach (GameObject go in dataList)
        {
            if (go != null)
                GameObject.Destroy(go);
        }
        dataList.Clear();
        usedDataList.Clear();

        if (rootObj != null)
            GameObject.Destroy(rootObj);
    }

    public void StartCleanupCoroutine(GameObject go)
    {
        StopCleanUpCoroutine(go);
        IEnumerator coroutine = CleanupTimer(go);
        cleanUpCorountines[go] = MonoMgr.Instance.StartCoroutine(coroutine);
    }

    private IEnumerator CleanupTimer(GameObject go)
    {
        yield return new WaitForSeconds(PoolManager.Instance.cleanUpTimeOut);
        if (go != null && dataList.Contains(go))
        {
            dataList.Remove(go);
            cleanUpCorountines.Remove(go);
            GameObject.Destroy(go);
        }
    }

    private void StopCleanUpCoroutine(GameObject go)
    {
        if (!cleanUpCorountines.ContainsKey(go))
            return;

        if (cleanUpCorountines[go] != null)
            MonoMgr.Instance.StopCoroutine(cleanUpCorountines[go]);
        cleanUpCorountines.Remove(go);
    }
}

public enum E_LoadWay
{
    ResourcesLoad,
    EditorResources,
    AssetBundle,
}