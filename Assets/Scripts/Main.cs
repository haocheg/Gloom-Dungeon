using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Main : MonoBehaviour
{

    private void Start()
    {
        SceneMgr.Instance.LoadSceneAsync("MainMenu", () =>
        {
            GameAssetLoader.Instance.LoadPackagedAsset<GameObject>("Manager", "GameManager", ".prefab", AssetPackageMode.EditorResources,
            (go) =>
            {
                GameObject.Instantiate(go);
            });
        });
    }
}
