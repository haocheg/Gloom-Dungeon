using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
[CreateAssetMenu(menuName = "Custom Setup/Dialogue Define/New DialogueNode", fileName = "DialogueNode - ")]
public class DialogueNode : ScriptableObject
{
    public int NodeID = -1;
    [TextArea] public string content;      // 对话文本
    public bool hasChoices = false;
    public List<DialogueChoice> choices;   // 分支选项
    public int nextNodeID = -1;            // 单线下一个节点
    public DialogueEventType eventTrigger; // 对话结束后触发的事件
    public bool randomNext = false;        // 是否随机选择下一个节点
    public bool isPlaceholder = false;     // 是否是占位用
    public List<int> randomNextID;         // 随机节点ID列表（优先级高于 nextNodeID）

    private void OnValidate()
    {
        if (NodeID == -1) return;

        string path = AssetDatabase.GetAssetPath(this);
        string currentFileName = System.IO.Path.GetFileNameWithoutExtension(path);
        string customName = "DialogueNode - " + NodeID.ToString();
        if (customName != currentFileName)
        {
            AssetDatabase.RenameAsset(path, customName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}


[System.Serializable]
public class DialogueChoice
{
    public string choiceText;    // 选项文字
    public int targetNodeID;  // 跳转到的节点ID
}

