using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "ItemSpawn/ItemSpawnInfo DataBase")]
public class ItemSpawnInfo : ScriptableObject
{
    public ItemSpawnData[] itemSpawnDatas;
}

[System.Serializable]
public class ItemSpawnData
{
    public int count;
    public string sceneName;
}
