using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Item/ItemInfoDataBase", fileName = "ItemInfoDataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemInfo> Items;
}
