using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "CheckPoint/CheckPoint DataBase")]
public class CheckPointInfo : ScriptableObject
{
    public CheckPointData[] pointDatas;
}

[System.Serializable]
public class CheckPointData
{
    public int Index;
    public string sceneName;
}