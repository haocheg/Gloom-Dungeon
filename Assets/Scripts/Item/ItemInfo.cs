using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Item/ItemInfo Config", fileName = "ItemInfo - ")]
public class ItemInfo : ScriptableObject
{
    public int ItemID = -1;
    public string Name;
    [TextArea] public string Description;
    public Sprite Icon;
    public ItemType Type;

    private void OnValidate()
    {
        if (ItemID == -1) return;

        string path = AssetDatabase.GetAssetPath(this);
        string currentFileName = System.IO.Path.GetFileNameWithoutExtension(path);
        string customName = "ItemInfo - " + ItemID.ToString();
        if (customName != currentFileName)
        {
            AssetDatabase.RenameAsset(path, customName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}

public enum ItemType
{
    Consumable,
    Equipment,
    Material,
    QuestItem,
    Currency,
    UtilityItem,
    Collectible,
}