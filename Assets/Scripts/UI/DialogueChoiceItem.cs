using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class DialogueChoiceItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    private void OnDestroy()
    {
        choiceButton.onClick.RemoveListener(OnChoiceButtonClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        choiceText.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        choiceText.color = Color.white;
    }
}
