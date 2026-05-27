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

    public ShopItemConfig ShopItemConfig;
    public Dictionary<SkillType, ShopItemInfo> itemInfos;
    public int Money;

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
        itemInfos = new Dictionary<SkillType, ShopItemInfo>();
        foreach (ShopItemInfo itemInfo in ShopItemConfig.shopItemInfos)
        {
            itemInfo.isBuy = false;
            itemInfos.Add(itemInfo.Type, itemInfo);
        }
        EventCenter.Instance.AddEventListener<SkillType>(E_TheEvent.E_SkillBuy, BuyItem);
        EventCenter.Instance.AddEventListener<int>(E_TheEvent.E_TransmitScore, GetValue);
    }

    public void BuyItem(SkillType skillType)
    {
        EventCenter.Instance.EventTrigger(E_TheEvent.E_GetScore);
        if (itemInfos.ContainsKey(skillType))
        {
            if (itemInfos[skillType].isBuy == false && Money >= itemInfos[skillType].Value)
            {
                EventCenter.Instance.EventTrigger<ShopItemInfo>(E_TheEvent.E_PlayerGetSkill, itemInfos[skillType]);
                itemInfos[skillType].isBuy = true;
            }
        }
    }

    public void Restart(PlayerStats playerStats)
    {
        foreach (SkillType type in itemInfos.Keys)
        {
            switch (type)
            {
                case SkillType.Dash:
                    itemInfos[type].isBuy = playerStats.canDash;
                    break;
                case SkillType.DoubleJump:
                    itemInfos[type].isBuy = playerStats.canDoubleJump;
                    break;
                case SkillType.WallSlider:
                    itemInfos[type].isBuy = playerStats.canWallSlider;
                    break;
                case SkillType.EdgeHang:
                    itemInfos[type].isBuy = playerStats.canEdgeHang;
                    break;
                default:
                    break;
            }
        }
    }

    private void GetValue(int value)
    {
        Money = value;
    }

    public Dictionary<SkillType, ShopItemInfo> CheckItem()
    {
        return itemInfos;
    }

    private void OnDestroy()
    {
        EventCenter.Instance.RemoveEventListener<SkillType>(E_TheEvent.E_SkillBuy, BuyItem);
        EventCenter.Instance.RemoveEventListener<int>(E_TheEvent.E_TransmitScore, GetValue);
    }

}
