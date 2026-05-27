using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopPanel : BasePanel
{
    [SerializeField] private ShopItem[] items;
    [SerializeField] private Button BackButton;
    private Dictionary<SkillType, ShopItemInfo> infos;

    private void Start()
    {
        BackButton.onClick.AddListener(OnBackButtonClick);
    }

    private void OnBackButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.HidePanel<ShopPanel>();
    }

    public void SetItem()
    {
        infos = ShopManager.Instance.CheckItem();
        foreach (SkillType type in infos.Keys)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].skillType == type)
                {
                    items[i].ConfirmBuy(infos[type].isBuy);
                    items[i].SetValue(infos[type].Value);
                }
            }
        }
    }

    public override void HideMe()
    {
        PlayerService.Instance.EnableInput(true);
    }

    public override void ShowMe()
    {
        SetItem();
        PlayerService.Instance.EnableInput(false);
    }

    private void OnDestroy()
    {
        BackButton.onClick.RemoveListener(OnBackButtonClick);
    }
}
