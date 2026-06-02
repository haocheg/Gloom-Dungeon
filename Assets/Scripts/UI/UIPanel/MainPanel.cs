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
    [SerializeField] private Transform parent;
    [SerializeField] private List<GameObject> Icons = new List<GameObject>();
    private void Start()
    {
        MenuButton.onClick.AddListener(OnMenuButtonClick);
        EventCenter.Instance.AddEventListener<int>(E_TheEvent.E_PlayerHealthChange, OnHealthChange);
    }

    private void OnMenuButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<MenuPanel>();
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

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        curHP = PlayerService.Instance.GetHealthValue();
        UpdateHP();
    }

    private void OnDestroy()
    {
        MenuButton.onClick.RemoveListener(OnMenuButtonClick);
        EventCenter.Instance.RemoveEventListener<int>(E_TheEvent.E_PlayerHealthChange, OnHealthChange);
    }

}
