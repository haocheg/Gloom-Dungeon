using System;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/NPC Define/New Define")]
public class NPCDefine : ScriptableObject
{
    public NPCData[] Datas;
}

[Serializable]
public class NPCData
{
    public string NPCName;
    public int NPCID;
    public int DialogueID; //开始对话id
    public Sprite Portrait;
    public Sprite FullPortrait;
}