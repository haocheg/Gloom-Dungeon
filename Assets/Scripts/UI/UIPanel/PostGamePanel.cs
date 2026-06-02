using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class PostGamePanel : BasePanel
{
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button StartOverButton;
    [SerializeField] private Button BackButton;

    private void Start()
    {
        RestartButton.onClick.AddListener(OnRestartButtonClick);
        StartOverButton.onClick.AddListener(OnStartOverButtonClick);
        BackButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnRestartButtonClick()
    {
        PlayerService.Instance.Respawn();
        LevelManager.Instance.RespawnPlayer(RespawnType.Death, () =>
        {
            UIManager.Instance.HidePanel<PostGamePanel>(true);
            UIManager.Instance.ShowPanel<MainPanel>();
        });
    }

    private void OnStartOverButtonClick()
    {
        GameManager.Instance.Restart();
        LevelManager.Instance.Reset();
        LevelManager.Instance.RespawnPlayer(RespawnType.New, () =>
        {
            UIManager.Instance.HidePanel<PostGamePanel>(true);
            UIManager.Instance.ShowPanel<MainPanel>();
        });
    }

    private void OnBackButtonClick()
    {
        SceneMgr.Instance.LoadSceneAsync("MainMenu", () =>
        {
            UIManager.Instance.HidePanel<PostGamePanel>();
            UIManager.Instance.HidePanel<MainPanel>();
            GameManager.Instance.BackToMainMenu();
            UIManager.Instance.ShowPanel<FirstPanel>();
            UIManager.Instance.ShowPanel<BGPanel>(E_UILayer.Bottom);
        });
    }


    public override void HideMe()
    {

    }

    public override void ShowMe()
    {

    }

    private void OnDestroy()
    {
        RestartButton.onClick.RemoveListener(OnRestartButtonClick);
        StartOverButton.onClick.RemoveListener(OnStartOverButtonClick);
        BackButton.onClick.RemoveListener(OnBackButtonClick);
    }

}
