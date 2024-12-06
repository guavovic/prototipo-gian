using System.Collections;
using UnityEngine;

public static class FadeAndReScaleAnimation 
{
    public static IEnumerator FadeInAndScaleCoroutine(Transform transform, CanvasGroup canvasGroup, float fadeDuration, float scaleDuration)
    {
        Vector3 originalScale = transform.localScale;
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = transform.localScale;
        transform.gameObject.SetActive(true);

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, t);
            transform.localScale = Vector3.Lerp(startScale, originalScale, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        transform.localScale = originalScale;
    }

    public static IEnumerator FadeOutAndScaleCoroutine(Transform transform, CanvasGroup canvasGroup, float fadeDuration, float scaleDuration)
    {
        float startTime = Time.time;
        float startAlpha = canvasGroup.alpha;
        Vector3 startScale = transform.localScale;

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t);
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        transform.localScale = Vector3.zero;
        transform.gameObject.SetActive(false);
    }
}