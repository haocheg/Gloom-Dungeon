using UnityEngine;
using UnityEngine.Events;

public enum AssetPackageMode
{
    EditorResources,
    AssetBundle
}

public interface IGameAssetLoader
{
    T LoadResources<T>(string path) where T : Object;
    void LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : Object;
    void LoadPackagedAsset<T>(string packageName, string assetName, string suffixName, AssetPackageMode mode,
        UnityAction<T> callback, bool isAsync = true) where T : Object;
}

public class GameAssetLoader : Singleton<GameAssetLoader>, IGameAssetLoader
{
    private GameAssetLoader()
    {
    }

    public T LoadResources<T>(string path) where T : Object
    {
        return ResourcesMgr.Instance.Load<T>(path);
    }

    public void LoadResourcesAsync<T>(string path, UnityAction<T> callback) where T : Object
    {
        ResourcesMgr.Instance.LoadAsync(path, callback);
    }

    public void LoadPackagedAsset<T>(string packageName, string assetName, string suffixName, AssetPackageMode mode,
        UnityAction<T> callback, bool isAsync = true) where T : Object
    {
        switch (mode)
        {
            case AssetPackageMode.EditorResources:
                AssetBundleResMgr.Instance.LoadEditorAsset(packageName, assetName, suffixName, callback, isAsync);
                break;
            case AssetPackageMode.AssetBundle:
                AssetBundleResMgr.Instance.LoadBundleAsset(packageName, assetName, callback, isAsync);
                break;
            default:
                callback?.Invoke(null);
                break;
        }
    }
}