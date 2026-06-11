using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private Player player;
    [SerializeField] private PlayerCamera playerCamera;
    private IGameAssetLoader assetLoader = GameAssetLoader.Instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<GameManager>();
            if (instance == null)
                instance = gameObject.AddComponent<GameManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    private void Start()
    {
        UIManager.Instance.ShowPanel<FirstPanel>();
        UIManager.Instance.ShowPanel<BGPanel>(E_UILayer.Bottom);
        LoadManager("LevelManager");
        LoadManager("AudioManager", () => AudioManager.Instance.PlayBGM("musicRegular"));
        LoadManager("ShopManager");
        LoadManager("NPCManager");
        LoadManager("DialogueManager");
        LoadManager("ItemManager");
        BagManager.Instance.Init();
        EventCenter.Instance.AddEventListener<bool>(E_TheEvent.E_PlayerDeath, OnPlayerDeath);
        EventCenter.Instance.AddEventListener(E_TheEvent.E_GameOver, GameOver);
    }

    private void LoadManager(string managerName, System.Action afterLoad = null)
    {
        assetLoader.LoadPackagedAsset<GameObject>("Manager", managerName, ".prefab", AssetPackageMode.EditorResources, (go) =>
        {
            GameObject.Instantiate(go);
            afterLoad?.Invoke();
        });
    }
    private void OnPlayerDeath(bool isDead)
    {
        UIManager.Instance.HidePanel<MainPanel>();
        UIManager.Instance.ShowPanel<PostGamePanel>();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetCamera(PlayerCamera camera)
    {
        this.playerCamera = camera;
    }

    public void ChangeScene()
    {
        playerCamera.GetCollider();
    }

    public void BackToMainMenu()
    {
        PlayerService.Instance.ShowOrHide(false);
        Destroy(player.gameObject);
        player = null;
        Destroy(playerCamera.gameObject);
        playerCamera = null;
    }

    public void GameOver()
    {
        UIManager.Instance.HidePanel<MainPanel>();
        UIManager.Instance.ShowPanel<PostGamePanel>();
    }

    public void Restart()
    {
        PlayerService.Instance.Restart();
    }

}
