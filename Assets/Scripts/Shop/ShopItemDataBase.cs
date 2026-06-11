using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Shop/ShopItemDataBase", fileName = "ShopItemDataBase")]
public class ShopItemDataBase : ScriptableObject
{
    public List<ShopItemInfo> shopItemInfos;
}
