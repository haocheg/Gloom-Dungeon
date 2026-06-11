
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopPanel : BasePanel
{
    [SerializeField] private TextMeshProUGUI moneyNum;
    [SerializeField] private ScrollRect sv;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject ShopItemPrefab;
    private List<ShopItem> shopItems;


    private void Start()
    {
        closeButton.onClick.AddListener(OnCloseButtonClick);
    }

    public void GenerateShopItem(List<ShopViewInfo> viewInfos)
    {
        if (shopItems == null)
            shopItems = new List<ShopItem>();
        else if (shopItems.Count > 0)
        {
            for (int i = 0; i < shopItems.Count; i++)
            {
                GameObject.Destroy(shopItems[i].gameObject);
            }
            shopItems.Clear();
        }

        foreach (ShopViewInfo info in viewInfos)
        {
            GameObject go = GameObject.Instantiate(ShopItemPrefab);
            go.transform.SetParent(sv.content.transform, false);
            ShopItem shopItem = go.GetComponent<ShopItem>();
            shopItem.ShowMe(info);
            shopItems.Add(shopItem);
        }
    }

    public void SetShopPanel(int gold)
    {
        moneyNum.text = gold.ToString();
    }

    private void OnCloseButtonClick()
    {
        UIManager.Instance.HidePanel<ShopPanel>();
        AudioManager.Instance.PlaySFX("buttonClick");
    }

    public override void HideMe()
    {
        PlayerService.Instance.EnableInput(true);
    }

    public override void ShowMe()
    {
        PlayerService.Instance.EnableInput(false);
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(OnCloseButtonClick);
    }
}
