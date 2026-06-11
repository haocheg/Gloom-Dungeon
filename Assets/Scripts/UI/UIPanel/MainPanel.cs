using System.Collections;
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
    [SerializeField] private Button MenuButton;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float smoothTime = 0.3f;
    private Coroutine updateHPCoro;


    private void Start()
    {
        MenuButton.onClick.AddListener(OnMenuButtonClick);
        EventCenter.Instance.AddEventListener<float>(E_TheEvent.E_GetPlayerHPPercent, OnHealthChange);
    }

    private void OnMenuButtonClick()
    {
        AudioManager.Instance.PlaySFX("buttonClick");
        UIManager.Instance.ShowPanel<MenuPanel>();
    }

    private void OnHealthChange(float value)
    {
        UpdateHP(value);
    }

    private void UpdateHP(float updateHP)
    {
        if (updateHPCoro != null)
            StopCoroutine(updateHPCoro);
        updateHPCoro = StartCoroutine(SmoothChangeHP(updateHP));
    }

    private IEnumerator SmoothChangeHP(float hp)
    {
        float start = healthSlider.value;
        float end = hp;
        float elapsed = 0f;

        while (elapsed < smoothTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / smoothTime;
            float newValue = Mathf.Lerp(start, end, t);
            healthSlider.value = newValue;
            yield return null;
        }
        healthSlider.value = end;
        updateHPCoro = null;
    }

    public override void HideMe()
    {

    }

    public override void ShowMe()
    {
        UpdateHP(1.0f);
    }

    private void OnDestroy()
    {
        MenuButton.onClick.RemoveListener(OnMenuButtonClick);
        EventCenter.Instance.RemoveEventListener<float>(E_TheEvent.E_GetPlayerHPPercent, OnHealthChange);
    }

}
