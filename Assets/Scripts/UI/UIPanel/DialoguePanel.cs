using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class DialoguePanel : BasePanel
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI speakerName;
    [SerializeField] private Image speakerPortrait;
    [SerializeField] private GameObject ChoiceContent;
    [SerializeField] private GameObject ChoicePrefab;

    [SerializeField] private float textSpeed = 0.1f;
    private string fullText;
    private Coroutine typingCo;
    private List<DialogueChoiceItem> dialogueChoiceItems;
    public bool isDialogueEnd { get; private set; }

    private void Start()
    {
        dialogueChoiceItems = new List<DialogueChoiceItem>();
    }

    public void PlayDialogueLine(DialogueInfo dialogueInfo)
    {
        speakerName.text = dialogueInfo.npcName;
        speakerPortrait.sprite = dialogueInfo.npcPortrait;
        fullText = dialogueInfo.textLine;
        typingCo = StartCoroutine(TypeTextCO(fullText));
    }

    public void CompleteTyping()
    {
        if (typingCo != null)
        {
            StopCoroutine(typingCo);
            dialogueText.text = fullText;
        }
        isDialogueEnd = true;
        DialogueManager.Instance.HandleNodeEnd();
    }

    private IEnumerator TypeTextCO(string text)
    {
        dialogueText.text = "";
        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isDialogueEnd = true;
        DialogueManager.Instance.HandleNodeEnd();
    }

    public void GenerateChoice(DialogueChoice dialogueChoice)
    {
        DialogueChoiceItem item = GameObject.Instantiate(ChoicePrefab, ChoiceContent.transform).GetComponent<DialogueChoiceItem>();
        item.ShowMe(dialogueChoice.choiceText, dialogueChoice.targetNodeID);
        dialogueChoiceItems.Add(item);
    }

    public void HideChoice()
    {
        if (dialogueChoiceItems.Count == 0 || dialogueChoiceItems == null)
            return;
        for (int i = 0; i < dialogueChoiceItems.Count; i++)
        {
            Destroy(dialogueChoiceItems[i].gameObject);
        }
        dialogueChoiceItems.Clear();
    }

    public override void HideMe()
    {
        PlayerService.Instance.EnableInput(true);
    }

    public override void ShowMe()
    {
        isDialogueEnd = false;
    }
}
