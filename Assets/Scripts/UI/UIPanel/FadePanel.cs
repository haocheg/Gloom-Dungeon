using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Title:
/// Description:
/// </summary>
public class FadePanel : BasePanel
{
    [SerializeField] private Image fadeImage;
    public Coroutine fadeEffectCo { get; private set; }

    public void DoFadeIn(float duration = 1) // black > transperent
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        FadeEffect(1f, duration);
    }

    public void DoFadeOut(float duration = 1) // transperent > black 
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        FadeEffect(0f, duration);
    }

    private void FadeEffect(float targetAlpha, float duration)
    {
        if (fadeEffectCo != null)
            StopCoroutine(fadeEffectCo);

        fadeEffectCo = StartCoroutine(FadeEffectCo(targetAlpha, duration));
    }

    private IEnumerator FadeEffectCo(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float time = 0f;

        while (time < duration)
        {
            time = time + Time.deltaTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, time / duration);

            fadeImage.color = color;

            yield return null;
        }

        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, targetAlpha);
    }


    public override void HideMe()
    {
        DoFadeOut();
    }

    public override void ShowMe()
    {
        DoFadeIn();
    }
}
