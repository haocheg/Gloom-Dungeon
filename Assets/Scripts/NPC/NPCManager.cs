using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class NPCManager : MonoBehaviour
{
    private static NPCManager instance;
    public static NPCManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField] private NPCDefine npcDefine;
    private Dictionary<int, NPCData> npcMap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = gameObject.GetComponent<NPCManager>();
            if (instance == null)
                instance = gameObject.AddComponent<NPCManager>();
            DontDestroyOnLoad(instance.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        npcMap = new Dictionary<int, NPCData>();
        foreach (NPCData npcData in npcDefine.Datas)
        {
            npcMap.Add(npcData.NPCID, npcData);
        }
    }


    public void Dialogue(int npcId)
    {
        if (npcMap.ContainsKey(npcId))
        {
            DialogueManager.Instance.StartDialogue(GetNPCDialogueInfo(npcId));
        }
    }

    public NPCDialogueInfo GetNPCDialogueInfo(int npcId)
    {
        if (npcMap.ContainsKey(npcId))
        {
            NPCDialogueInfo info = new NPCDialogueInfo();
            info.NPCID = npcId;
            info.DialogueID = npcMap[npcId].DialogueID;
            info.NPCName = npcMap[npcId].NPCName;
            info.Portrait = npcMap[npcId].Portrait;
            return info;
        }
        return null;
    }

}

public class NPCDialogueInfo
{
    public string NPCName;
    public int NPCID;
    public int DialogueID; //开始对话id
    public Sprite Portrait;
}