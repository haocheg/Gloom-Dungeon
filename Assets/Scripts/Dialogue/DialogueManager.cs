using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    public static DialogueManager Instance
    {
        get
        {
            return instance;
        }
    }
    private bool startDialogue;
    private DialoguePanel panel;
    [SerializeField] private DialogueDatabase dialogueDatabase;
    private Dictionary<int, DialogueData> dialogueDataDic;
    private DialogueData currentDialogue;
    private DialogueNode curreDialogueNode;
    private NPCDialogueInfo currentDialogueNPC;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<DialogueManager>();
            if (instance == null)
                instance = gameObject.AddComponent<DialogueManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        dialogueDataDic = new Dictionary<int, DialogueData>();
        foreach (DialogueData data in dialogueDatabase.allDialogues)
        {
            dialogueDataDic.Add(data.dialogueID, data);
        }
    }

    public void StartDialogue(NPCDialogueInfo npcInfo)
    {
        currentDialogueNPC = npcInfo;
        if (!dialogueDataDic.ContainsKey(npcInfo.DialogueID))
        {
            return;
        }
        if (dialogueDataDic[npcInfo.DialogueID])
            PlayerService.Instance.EnableInput(false);
        UIManager.Instance.ShowPanel<DialoguePanel>(E_UILayer.Middle, (panel) =>
        {
            this.panel = panel;
        });
        currentDialogue = dialogueDataDic[npcInfo.DialogueID];
        curreDialogueNode = currentDialogue.startNode;
        ContinueDialogue(currentDialogue, curreDialogueNode);
    }

    private void ContinueDialogue(DialogueData data, DialogueNode node)
    {
        if (node != null)
        {
            if (node.isPlaceholder)
            {
                HandlePlaceholder(data, node);
                return;
            }
            else if (node.randomNext)
            {
                RandomNext(node);
            }
            else if (node.nextNodeID != -1)
            {
                HandleNext(node);
            }
            else
                StopDialogue(node.eventTrigger);
        }

    }

    private void HandleDialogue(DialogueNode node)
    {
        DialogueInfo info = new DialogueInfo(currentDialogueNPC.NPCName, currentDialogueNPC.Portrait, node.content);
        panel.PlayDialogueLine(info);
        startDialogue = true;
    }

    private void HandlePlaceholder(DialogueData data, DialogueNode node)
    {
        if (node.randomNext)
        {
            RandomNext(node);
        }
        else if (node.nextNodeID != -1)
        {
            HandleNext(node);
        }
    }

    private void RandomNext(DialogueNode node)
    {
        int nextID = node.randomNextID[UnityEngine.Random.Range(0, node.randomNextID.Count)];
        curreDialogueNode = currentDialogue.allDialogueNode.Find(n => n.NodeID == nextID);
        HandleDialogue(curreDialogueNode);
    }

    private void HandleNext(DialogueNode node)
    {
        curreDialogueNode = currentDialogue.allDialogueNode.Find(n => n.NodeID == node.nextNodeID);
        HandleDialogue(curreDialogueNode);
    }

    public void DialogueChoice(int targetID)
    {
        curreDialogueNode = currentDialogue.allDialogueNode.Find(n => n.NodeID == targetID);
        HandleDialogue(curreDialogueNode);
        HideChoice();
    }

    private void HandleChoice(DialogueNode node)
    {
        for (int i = node.choices.Count - 1; i >= 0; i--)
        {
            panel.GenerateChoice(node.choices[i]);
        }
    }

    private void HideChoice()
    {
        panel.HideChoice();
    }

    public void HandleNodeEnd()
    {
        if (curreDialogueNode.hasChoices)
        {
            HandleChoice(curreDialogueNode);
        }
    }


    public void StopDialogue(DialogueEventType dialogueEventType)
    {
        panel.HideChoice();
        UIManager.Instance.HidePanel<DialoguePanel>();
        switch (dialogueEventType)
        {
            case DialogueEventType.None:
                break;
            case DialogueEventType.OpenShop:
                break;
            case DialogueEventType.OpenCraft:
                break;
            case DialogueEventType.OpenQuest:
                break;
            case DialogueEventType.GetQuestReward:
                break;
            case DialogueEventType.PlayerMakeChoices:
                break;
            case DialogueEventType.CloseDialogue:
                break;
            default:
                break;
        }
        panel = null;
        startDialogue = false;
        curreDialogueNode = null;
        currentDialogue = null;
        currentDialogueNPC = null;
    }

    private void Update()
    {
        if (startDialogue)
        {
            if (panel != null)
            {
                if (PlayerInputMgr.Instance.ListenUIInput(PlayerInputMgr.UIInputType.Interact))
                {
                    if (panel.isDialogueEnd)
                    {
                        ContinueDialogue(currentDialogue, curreDialogueNode);
                    }
                    else
                        panel.CompleteTyping();
                }
            }

        }
    }


}

public enum ChatType
{
    Chat,
    Story,
    Task,
}

public enum DialogueType
{
    Single,
    Continuous
}

public class DialogueInfo
{
    public string npcName;
    public Sprite npcPortrait;
    public string textLine;

    public DialogueInfo()
    {

    }

    public DialogueInfo(string name, Sprite portrait, string text)
    {
        npcName = name;
        npcPortrait = portrait;
        textLine = text;
    }

}