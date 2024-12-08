using System.Collections;
using UnityEngine;

public static class FadeAndReScaleAnimation
{
    private static readonly Vector3 _directionOffset= new Vector3(0, -2, 0);

    public static IEnumerator FadeInAndScaleCoroutine(Transform transform, CanvasGroup canvasGroup, float fadeDuration)
    {
        Vector3 targetScale = transform.localScale;
        Vector3 originalPosition = transform.localPosition + (Vector3.up * 2);

        transform.localPosition += _directionOffset;
        canvasGroup.alpha = 0f;

        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            transform.localPosition = Vector3.Lerp(originalPosition + _directionOffset, originalPosition, t);
            yield return null;
        }

        transform.localPosition = originalPosition;
        transform.localScale = targetScale;
        canvasGroup.alpha = 1f;
    }

    public static IEnumerator FadeOutAndScaleCoroutine(Transform transform, CanvasGroup canvasGroup, float fadeDuration)
    {
        Vector3 originalPosition = transform.localPosition;

        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
            transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + (Vector3.down * 2) - _directionOffset, t);
            yield return null;
        }

        transform.localPosition = originalPosition + _directionOffset;
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0f;
    }
}