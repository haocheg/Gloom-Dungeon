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
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        RestartButton.onClick.AddListener(OnRestartButtonClick);
        StartOverButton.onClick.AddListener(OnStartOverButtonClick);
        BackButton.onClick.AddListener(OnBackButtonClick);
        EventCenter.Instance.AddEventListener<int>(E_TheEvent.E_TransmitScore, GetScore);
        EventCenter.Instance.EventTrigger(E_TheEvent.E_GetScore);
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

    private void GetScore(int v = 0)
    {
        scoreText.text = "获得分数: " + v.ToString();
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
        EventCenter.Instance.RemoveEventListener<int>(E_TheEvent.E_TransmitScore, GetScore);
    }

}
