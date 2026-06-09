using DG.Tweening;
using TMPro;
using UnityEngine;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class Hint : MonoBehaviour
{
    public float showDuration = 0.25f;
    public float hideDuration = 0.2f;
    public float showScale = 1f;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro tmpText;
    private Color originalSpriteColor;
    private Color originalTextColor;

    private void Awake()
    {
        if (spriteRenderer != null) originalSpriteColor = spriteRenderer.color;
        if (tmpText != null) originalTextColor = tmpText.color;
        // 初始状态：透明、缩小
        spriteRenderer.color = new Color(originalSpriteColor.r, originalSpriteColor.g, originalSpriteColor.b, 0f);
        tmpText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0f);
        transform.localScale = Vector3.zero;
    }

    public void ShowHint()
    {
        // 动画：淡入 + 放大
        transform.DOScale(showScale, showDuration).SetEase(Ease.OutBack);
        if (spriteRenderer != null) spriteRenderer.DOFade(1f, showDuration);
        if (tmpText != null) tmpText.DOFade(1f, showDuration);
    }

    public void HideHint()
    {
        // 淡出 + 缩小
        transform.DOScale(0f, hideDuration).SetEase(Ease.InBack);
        if (spriteRenderer != null) spriteRenderer.DOFade(0f, hideDuration);
        if (tmpText != null) tmpText.DOFade(0f, hideDuration);
    }
}
