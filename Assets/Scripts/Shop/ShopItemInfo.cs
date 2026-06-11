using UnityEditor;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Shop/ShopItemInfo Config", fileName = "ShopItemInfo - ")]
public class ShopItemInfo : ScriptableObject
{
    public int ItemID = -1;
    public int Count;              // 单次购买数量
    public int ProvidedCount = -1; // 商店提供总数量， -1代表无限
    public int Price;
    public bool isProvided = true;

    private void OnValidate()
    {
        if (ItemID == -1) return;

        string path = AssetDatabase.GetAssetPath(this);
        string currentFileName = System.IO.Path.GetFileNameWithoutExtension(path);
        string customName = "ShopItemInfo - " + ItemID.ToString();
        if (customName != currentFileName)
        {
            AssetDatabase.RenameAsset(path, customName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}