using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopItem : MonoBehaviour
{
    [SerializeField] private Button BuyButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private TextMeshProUGUI valueText;
    public bool IsBuy = false;

    public SkillType skillType;

    private void Start()
    {
        BuyButton.onClick.AddListener(OnBuyButtonClick);
    }

    private void OnBuyButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        if (IsBuy) return;
        EventCenter.Instance.EventTrigger<SkillType>(E_TheEvent.E_SkillBuy, skillType);
    }

    public void SetValue(int value)
    {
        valueText.text = ": " + value.ToString();
    }

    public void ConfirmBuy(bool value)
    {
        if (value)
        {
            IsBuy = true;
            buttonText.text = "ÒÑ¹ºÂò";
            BuyButton.interactable = false;
        }
        else
        {
            IsBuy = false;
            buttonText.text = "¹ºÂò";
            BuyButton.interactable = true;
        }
    }

    private void OnDestroy()
    {
        BuyButton.onClick.RemoveListener(OnBuyButtonClick);
    }

}

public enum SkillType
{
    Dash,
    DoubleJump,
    WallSlider,
    EdgeHang,
}