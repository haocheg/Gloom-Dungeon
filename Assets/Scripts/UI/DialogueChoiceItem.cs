using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class DialogueChoiceItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI choiceText;
    [SerializeField] private Button choiceButton;
    private int targetID;

    private void Start()
    {
        choiceButton.onClick.AddListener(OnChoiceButtonClick);
    }

    private void OnChoiceButtonClick()
    {
        DialogueManager.Instance.DialogueChoice(targetID);
    }

    public void ShowMe(string text, int id)
    {
        choiceText.text = text;
        targetID = id;
        this.gameObject.SetActive(true);
        choiceText.gameObject.SetActive(true);
    }

    public void HideMe()
    {
        choiceText.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }


    private void OnMouseEnter()
    {
        choiceText.color = Color.yellow;
    }

    private void OnMouseExit()
    {
        choiceText.color = Color.white;
    }

    private void OnDestroy()
    {
        choiceButton.onClick.RemoveListener(OnChoiceButtonClick);
    }

}
