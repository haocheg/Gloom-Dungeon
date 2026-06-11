using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class ShopItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image Icon;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Price;
    [SerializeField] private int ItemID;


    private void Start()
    {

    }

    public void ShowMe(ShopViewInfo info)
    {
        this.Icon.sprite = info.Icon;
        this.Name.text = info.Name;
        this.Price.text = info.Price.ToString();
        this.ItemID = info.ItemID;
    }


    private void OnDestroy()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopManager.Instance.BuyItem(ItemID);
        AudioManager.Instance.PlaySFX("buttonClick");
    }
}
