using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[Serializable]
public class PlayerBagData
{
    public int Gold;  //玩家拥有的金币数量
    public List<BagItemInfo> bagItems; // 背包里的道具 
    public bool isFirstLoad;

    public PlayerBagData()
    {
        Gold = 100;
        bagItems = new List<BagItemInfo>();
        isFirstLoad = true;
    }

}

[Serializable]
public class BagItemInfo
{
    public int ItemID;
    public int Count;
}