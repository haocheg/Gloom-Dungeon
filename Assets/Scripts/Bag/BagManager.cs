using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class BagManager : Singleton<BagManager>
{
    private PlayerBagData curBagData;
    private Dictionary<int, BagItemInfo> bagItemDic;

    private BagManager()
    {

    }

    public void Init()
    {
        LoadBagData();
        bagItemDic = new Dictionary<int, BagItemInfo>();
        foreach (BagItemInfo itemInfo in curBagData.bagItems)
        {
            if (bagItemDic.ContainsKey(itemInfo.ItemID))
                bagItemDic[itemInfo.ItemID].Count += itemInfo.Count;
            else
                bagItemDic.Add(itemInfo.ItemID, itemInfo);
        }
        curBagData.bagItems = new List<BagItemInfo>(bagItemDic.Values);
    }

    public void LoadBagData()
    {
        PlayerBagData bagData = JsonMgr.Instance.LoadData<PlayerBagData>("PlayerBagData");
        if (bagData.isFirstLoad)
        {
            bagData.isFirstLoad = false;
            curBagData = bagData;
            SaveBagData();
            return;
        }
        curBagData = bagData;

    }

    public void SaveBagData()
    {
        curBagData.bagItems = new List<BagItemInfo>(bagItemDic.Values);
        JsonMgr.Instance.SaveData(curBagData, "PlayerBagData");
    }

    public int GetPlayerGold()
    {
        return curBagData.Gold;
    }

    public void SpendGold(int gold)
    {
        if (curBagData.Gold >= gold)
        {
            curBagData.Gold -= gold;
        }
        else
            curBagData.Gold = 0;
    }

    public void GetItem(BagItemInfo info)
    {
        if (bagItemDic.ContainsKey(info.ItemID))
        {
            bagItemDic[info.ItemID].Count += info.Count;
        }
        else
            bagItemDic.Add(info.ItemID, info);

    }

    public void RemoveItem(BagItemInfo info)
    {
        if (bagItemDic.ContainsKey(info.ItemID))
        {
            bagItemDic[info.ItemID].Count -= info.Count;
            if (bagItemDic[info.ItemID].Count <= 0)
                bagItemDic.Remove(info.ItemID);
        }
    }

}
