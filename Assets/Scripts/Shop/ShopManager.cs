using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopManager : MonoBehaviour
{
    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            return instance;
        }
    }

    public ShopItemDataBase ShopItemDataBase;
    private Dictionary<int, ShopItemInfo> shopItemDic;
    private ShopPanel panel;
    private int PlayerGoldNum;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<ShopManager>();
            if (instance == null)
                instance = gameObject.AddComponent<ShopManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    private void Start()
    {
        shopItemDic = new Dictionary<int, ShopItemInfo>();
        foreach (ShopItemInfo item in ShopItemDataBase.shopItemInfos)
        {
            shopItemDic.Add(item.ItemID, item);
        }
        PlayerGoldNum = BagManager.Instance.GetPlayerGold();
    }

    public List<ShopViewInfo> GetShopView()
    {
        List<ShopViewInfo> views = new List<ShopViewInfo>();
        foreach (int itemID in shopItemDic.Keys)
        {
            if (shopItemDic[itemID].isProvided)
            {
                ShopViewInfo viewInfo = new ShopViewInfo();
                ItemInfo itemInfo = ItemManager.Instance.GetItemInfo(itemID);
                viewInfo.ItemID = itemID;
                viewInfo.Icon = itemInfo.Icon;
                viewInfo.Name = itemInfo.Name;
                viewInfo.Price = shopItemDic[itemID].Price;
                views.Add(viewInfo);
            }
        }
        return views;
    }

    public void OpenShop()
    {
        UIManager.Instance.ShowPanel<ShopPanel>(E_UILayer.Middle, (panel) =>
        {
            this.panel = panel;
            panel.GenerateShopItem(GetShopView());
            panel.SetShopPanel(PlayerGoldNum);
        });

    }

    public void BuyItem(int itemID)
    {
        if (shopItemDic.ContainsKey(itemID))
        {
            ShopItemInfo shopItemInfo = shopItemDic[itemID];
            if (shopItemInfo.isProvided)
            {
                PlayerGoldNum = BagManager.Instance.GetPlayerGold();
                if (PlayerGoldNum >= shopItemInfo.Price)
                {
                    PlayerGoldNum -= shopItemInfo.Price;
                    BagItemInfo bagItemInfo = new BagItemInfo();
                    bagItemInfo.ItemID = itemID;
                    bagItemInfo.Count = shopItemInfo.Count;
                    BagManager.Instance.SpendGold(shopItemInfo.Price);
                    BagManager.Instance.GetItem(bagItemInfo);
                    panel.SetShopPanel(PlayerGoldNum);
                    DecreaseItemProvided(itemID, shopItemInfo.Count);
                }
                else
                {
                    Debug.Log("¹ºÂòÊ§°Ü");
                }
            }
        }
    }

    private void DecreaseItemProvided(int ItemID, int Count)
    {
        if (shopItemDic.ContainsKey(ItemID))
        {
            shopItemDic[ItemID].ProvidedCount -= Count;
            if (shopItemDic[ItemID].ProvidedCount == -1)
            {
                return;
            }
            if (shopItemDic[ItemID].ProvidedCount == 0)
            {
                shopItemDic[ItemID].isProvided = false;
            }
        }
    }

}

public class ShopViewInfo
{
    public Sprite Icon;
    public string Name;
    public int Price;
    public int ItemID;
}