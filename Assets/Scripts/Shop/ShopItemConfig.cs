using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Shop/ShopItem Config")]
public class ShopItemConfig : ScriptableObject
{
    public ShopItemInfo[] shopItemInfos;
}

[System.Serializable]
public class ShopItemInfo
{
    public SkillType Type;
    public int Value;
    public bool isBuy;
}