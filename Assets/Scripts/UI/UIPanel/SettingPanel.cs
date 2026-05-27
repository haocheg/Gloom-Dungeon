using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class SettingPanel : BasePanel
{

    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Button BackButton;

    private void Start()
    {
        SFXSlider.onValueChanged.AddListener(OnSFXVolumnChange);
        MusicSlider.onValueChanged.AddListener(OnMusicVolumnChange);
        BackButton.onClick.AddListener(OnBackClick);
    }

    private void OnSFXVolumnChange(float v)
    {
        AudioManager.Instance.ChangeSFXVolumn(v);
    }

    private void OnMusicVolumnChange(float v)
    {
        AudioManager.Instance.ChangeBGMVolumn(v);
    }

    private void OnBackClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.HidePanel<SettingPanel>();
        AudioManager.Instance.SaveVolumnData();
    }


    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        SFXSlider.value = AudioManager.Instance.GetSFXVolumn();
        MusicSlider.value = AudioManager.Instance.GetBGMVolumn();
    }

    private void OnDestroy()
    {
        SFXSlider.onValueChanged.RemoveListener(OnSFXVolumnChange);
        MusicSlider.onValueChanged.RemoveListener(OnMusicVolumnChange);
        BackButton.onClick.RemoveListener(OnBackClick);
    }

}
