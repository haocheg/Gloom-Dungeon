using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class FirstPanel : BasePanel
{
    [SerializeField] private Button EnterButton;
    [SerializeField] private Button ExitButton;
    [SerializeField] private Button SetButton;

    private void Start()
    {
        EnterButton.onClick.AddListener(OnClickEnter);
        ExitButton.onClick.AddListener(OnClickExit);
        SetButton.onClick.AddListener(OnClickSetButton);
    }

    void OnClickEnter()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.HidePanel<FirstPanel>();
        UIManager.Instance.HidePanel<BGPanel>(true);

        LevelManager.Instance.Reset();
        LevelManager.Instance.RespawnPlayer(RespawnType.New, () =>
        {
            GameAssetLoader.Instance.LoadPackagedAsset<GameObject>("Player", "player", ".prefab", AssetPackageMode.EditorResources, (go) =>
            {
                GameObject g = GameObject.Instantiate(go);
                PlayerService.Instance.InitPlayer();
                GameManager.Instance.SetPlayer(g.GetComponent<Player>());
            });
            GameAssetLoader.Instance.LoadPackagedAsset<GameObject>("camera", "PlayerCamera", ".prefab", AssetPackageMode.EditorResources, (go) =>
            {
                GameObject g = GameObject.Instantiate(go);
                GameManager.Instance.SetCamera(g.GetComponent<PlayerCamera>());
            });
            UIManager.Instance.ShowPanel<MainPanel>();
        });

    }

    void OnClickExit()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void OnClickSetButton()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<SettingPanel>(E_UILayer.Top);
    }

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    private void OnDestroy()
    {
        EnterButton.onClick.RemoveAllListeners();
        ExitButton.onClick.RemoveAllListeners();
        SetButton.onClick.RemoveAllListeners();
    }

}
