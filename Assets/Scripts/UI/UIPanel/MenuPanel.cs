using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class MenuPanel : BasePanel
{
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button SettingButton;
    [SerializeField] private Button BackButton;

    private void Start()
    {
        ContinueButton.onClick.AddListener(OnContinueButtonClick);
        SettingButton.onClick.AddListener(OnSettingButtonClick);
        BackButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnContinueButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.HidePanel<MenuPanel>();
    }

    private void OnSettingButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<SettingPanel>();
    }

    private void OnBackButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        SceneMgr.Instance.LoadSceneAsync("MainMenu", () =>
        {
            UIManager.Instance.HidePanel<MenuPanel>();
            UIManager.Instance.HidePanel<MainPanel>();
            GameManager.Instance.BackToMainMenu();
            UIManager.Instance.ShowPanel<FirstPanel>();
            UIManager.Instance.ShowPanel<BGPanel>(E_UILayer.Bottom);
        });
    }


    public override void HideMe()
    {
        PlayerService.Instance.EnableInput(true);
    }

    public override void ShowMe()
    {
        PlayerService.Instance.EnableInput(false);
    }
}
