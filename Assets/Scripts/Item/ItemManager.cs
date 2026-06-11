using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ItemManager : MonoBehaviour
{
    private static ItemManager instance;
    public static ItemManager Instance
    {
        get
        {
            return instance;
        }
    }

    public ItemDataBase ItemDataBase;
    private Dictionary<int, ItemInfo> itemDic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<ItemManager>();
            if (instance == null)
                instance = gameObject.AddComponent<ItemManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    private void Start()
    {
        itemDic = new Dictionary<int, ItemInfo>();
        foreach (ItemInfo item in ItemDataBase.Items)
        {
            itemDic.Add(item.ItemID, item);
        }
    }

    public ItemInfo GetItemInfo(int itemID)
    {
        if (itemDic.ContainsKey(itemID))
        {
            return itemDic[itemID];
        }
        return null;
    }

}
