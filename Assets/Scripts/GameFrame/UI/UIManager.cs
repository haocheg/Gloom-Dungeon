using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Title:
/// Description:
/// </summary>
public class UIManager : Singleton<UIManager>
{
    private abstract class BasePanelInfo
    {
        public bool needHide;
        public Coroutine cleanUpCoroutine;
    }

    private class PanelInfo<T> : BasePanelInfo where T : BasePanel
    {
        public T panel;
        public UnityAction<T> callBack;

        public PanelInfo(UnityAction<T> callBack)
        {
            this.callBack = callBack;
        }
    }

    private readonly Dictionary<string, BasePanelInfo> panelDic = new Dictionary<string, BasePanelInfo>();
    private IGameAssetLoader assetLoader = GameAssetLoader.Instance;

    private Camera uiCamera;
    private Canvas uiCanvas;
    private EventSystem uiEventSystem;

    private Transform bottomLayer;
    private Transform middleLayer;
    private Transform topLayer;
    private Transform systemLayer;

    public float cleanUpTimeOut = 60f;

    private UIManager()
    {
        InitRootObjects();
    }

    public void SetAssetLoader(IGameAssetLoader loader)
    {
        assetLoader = loader ?? GameAssetLoader.Instance;
    }

    private void InitRootObjects()
    {
        uiCamera = CreatePersistentComponent<Camera>("UI/UICamera");

        uiCanvas = CreatePersistentComponent<Canvas>("UI/Canvas");
        uiCanvas.worldCamera = uiCamera;

        bottomLayer = uiCanvas.transform.Find("Bottom");
        middleLayer = uiCanvas.transform.Find("Middle");
        topLayer = uiCanvas.transform.Find("Top");
        systemLayer = uiCanvas.transform.Find("System");

        uiEventSystem = CreatePersistentComponent<EventSystem>("UI/EventSystem");
    }

    private T CreatePersistentComponent<T>(string resourcesPath) where T : Component
    {
        GameObject prefab = assetLoader.LoadResources<GameObject>(resourcesPath);
        GameObject instance = GameObject.Instantiate(prefab);
        GameObject.DontDestroyOnLoad(instance);
        return instance.GetComponent<T>();
    }

    /// <summary>
    /// 获取指定层级的对象
    /// </summary>
    /// <param name="layer">层级枚举</param>
    /// <returns></returns>
    public Transform GetLayerTransform(E_UILayer layer)
    {
        switch (layer)
        {
            case E_UILayer.Bottom:
                return bottomLayer;
            case E_UILayer.Middle:
                return middleLayer;
            case E_UILayer.Top:
                return topLayer;
            case E_UILayer.System:
                return systemLayer;
            default:
                return null;
        }
    }

    public Transform GetLayerTramsform(E_UILayer layer)
    {
        return GetLayerTransform(layer);
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <typeparam name="T">面板类型，注意：脚本名一定要和面板名是一致的</typeparam>
    /// <param name="layer">面板层级</param>
    /// <param name="callBack">显示后的回调</param>
    /// <param name="isAsync">是否异步加载</param>
    public void ShowPanel<T>(E_UILayer layer = E_UILayer.Middle, UnityAction<T> callBack = null,
        bool isAsync = false) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (panelDic.TryGetValue(panelName, out BasePanelInfo baseInfo))
        {
            ShowCachedPanel(panelName, baseInfo as PanelInfo<T>, callBack);
            return;
        }

        PanelInfo<T> info = new PanelInfo<T>(callBack);
        panelDic.Add(panelName, info);
        LoadPanel(panelName, layer, info, isAsync);
    }

    private void ShowCachedPanel<T>(string panelName, PanelInfo<T> info, UnityAction<T> callBack) where T : BasePanel
    {
        if (info == null)
            return;

        if (info.panel == null)
        {
            if (callBack != null)
                info.callBack += callBack;
            return;
        }

        StopCleanUpCoroutine(panelName, info);
        info.panel.gameObject.SetActive(true);
        info.panel.ShowMe();
        callBack?.Invoke(info.panel);
    }

    private void LoadPanel<T>(string panelName, E_UILayer layer, PanelInfo<T> info, bool isAsync) where T : BasePanel
    {
        assetLoader.LoadPackagedAsset<GameObject>("UI", panelName, null, AssetPackageMode.EditorResources, (obj) =>
        {
            if (!panelDic.ContainsKey(panelName) || info.needHide)
            {
                panelDic.Remove(panelName);
                return;
            }

            Transform parent = GetLayerTransform(layer);
            if (parent == null)
                parent = middleLayer;

            GameObject panelObject = GameObject.Instantiate(obj, parent, false);
            T panel = panelObject.GetComponent<T>();
            panel.ShowMe();
            info.panel = panel;
            info.callBack?.Invoke(panel);
            info.callBack = null;
        }, isAsync);
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    public void HidePanel<T>(bool needDestory = false) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (!panelDic.TryGetValue(panelName, out BasePanelInfo baseInfo))
        {
            Debug.Log("不存在该面板");
            return;
        }

        PanelInfo<T> info = baseInfo as PanelInfo<T>;
        if (info == null)
            return;

        if (info.panel == null)
        {
            info.needHide = true;
            info.callBack = null;
            return;
        }

        info.panel.HideMe();
        if (needDestory)
        {
            GameObject.Destroy(info.panel.gameObject);
            panelDic.Remove(panelName);
            return;
        }

        info.panel.gameObject.SetActive(false);
        StartCleanUpCoroutine(panelName, info);
    }

    private void StartCleanUpCoroutine<T>(string panelName, PanelInfo<T> info) where T : BasePanel
    {
        StopCleanUpCoroutine(panelName, info);
        info.cleanUpCoroutine = MonoMgr.Instance.StartCoroutine(CleanUp(panelName, info));
    }

    private IEnumerator CleanUp<T>(string panelName, PanelInfo<T> info) where T : BasePanel
    {
        yield return new WaitForSeconds(cleanUpTimeOut);
        if (panelDic.ContainsKey(panelName) && info.panel != null)
        {
            panelDic.Remove(panelName);
            GameObject.Destroy(info.panel.gameObject);
        }
    }

    private void StopCleanUpCoroutine<T>(string panelName, PanelInfo<T> info) where T : BasePanel
    {
        if (!panelDic.ContainsKey(panelName) || info.cleanUpCoroutine == null)
            return;

        MonoMgr.Instance.StopCoroutine(info.cleanUpCoroutine);
        info.cleanUpCoroutine = null;
    }

    /// <summary>
    /// 获取指定面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    /// <returns></returns>
    public void GetPanel<T>(UnityAction<T> callBack) where T : BasePanel
    {
        string panelName = typeof(T).Name;
        if (!panelDic.TryGetValue(panelName, out BasePanelInfo baseInfo))
            return;

        PanelInfo<T> info = baseInfo as PanelInfo<T>;
        if (info == null || info.needHide)
        {
            Debug.Log($"{panelName}需要隐藏，但又要获取？请检查代码合理性");
            return;
        }

        if (info.panel == null)
        {
            info.callBack += callBack;
            return;
        }

        callBack?.Invoke(info.panel);
    }

    /// <summary>
    /// 为控件添加自定义事件
    /// </summary>
    /// <param name="control">控件</param>
    /// <param name="triggerType">事件类型</param>
    /// <param name="callBack">触发回调</param>
    public static void AddCusEventListener(UIBehaviour control, EventTriggerType triggerType, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = triggerType;
        entry.callback.AddListener(callBack);
        trigger.triggers.Add(entry);
    }
}

public enum E_UILayer
{
    Bottom,
    Middle,
    Top,
    System
}