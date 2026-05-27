using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class MainPanel : BasePanel
{
    [SerializeField] private int curHP;
    [SerializeField] private Button MenuButton;
    [SerializeField] private Button ShopButton;
    [SerializeField] private Transform parent;
    [SerializeField] private TextMeshProUGUI emeraldCount;
    [SerializeField] private List<GameObject> Icons = new List<GameObject>();
    private void Start()
    {
        MenuButton.onClick.AddListener(OnMenuButtonClick);
        ShopButton.onClick.AddListener(OnShopButtonClick);
        EventCenter.Instance.AddEventListener<int>(E_TheEvent.E_PlayerHealthChange, OnHealthChange);
        EventCenter.Instance.AddEventListener<int>(E_TheEvent.E_TransmitScore, UpdateScore);
    }

    private void OnMenuButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<MenuPanel>();
    }

    private void OnShopButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<ShopPanel>();
    }

    private void OnHealthChange(int value)
    {
        curHP += value;
        UpdateHP();
    }

    private void UpdateHP()
    {
        int count = curHP / 10;
        for (int i = 0; i < Icons.Count; i++)
        {
            Destroy(Icons[i]);
        }
        Icons.Clear();
        for (int i = 0; i < count; i++)
        {
            GameAssetLoader.Instance.LoadPackagedAsset<GameObject>("UI", "HealthIcon", null, AssetPackageMode.EditorResources, (go) =>
            {
                Icons.Add(GameObject.Instantiate(go, parent));
            });
        }
    }

    private void UpdateScore(int v = 0)
    {
        emeraldCount.text = ": " + v.ToString();
    }

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        curHP = PlayerService.Instance.GetHealthValue();
        EventCenter.Instance.EventTrigger(E_TheEvent.E_GetScore);
        UpdateHP();
    }

    private void OnDestroy()
    {
        MenuButton.onClick.RemoveListener(OnMenuButtonClick);
        ShopButton.onClick.RemoveListener(OnShopButtonClick);
        EventCenter.Instance.RemoveEventListener<int>(E_TheEvent.E_PlayerHealthChange, OnHealthChange);
        EventCenter.Instance.RemoveEventListener<int>(E_TheEvent.E_TransmitScore, UpdateScore);
    }

}
