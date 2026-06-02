using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Dialogue Define/New Data", fileName = "Dialogue - ")]
public class DialogueData : ScriptableObject
{
    public int dialogueID;
    public int npcID;                      // 关联 NPCDefine 中的 NPC
    public DialogueNode startNode;
    public List<DialogueNode> allDialogueNode;

}

public enum DialogueEventType
{
    None,
    OpenShop,
    OpenCraft,
    OpenQuest,
    GetQuestReward,
    PlayerMakeChoices,
    CloseDialogue,
}