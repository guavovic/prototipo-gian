using System.Collections;
using UnityEngine;

public sealed class SceneTransitionUI : MonoBehaviour
{
    [SerializeField] private RectTransform circleMask;
    [Space(5f)]
    [SerializeField] private float zoomDuration = 2f;
    [SerializeField] private float maxScale = 5.0f;
    [Space(5f)]
    [SerializeField] private AnimationCurve zoomInCurve;
    [SerializeField] private AnimationCurve zoomOutCurve;

    private void Start()
    {
        if (circleMask != null)
        {
            circleMask.localScale = Vector3.zero;
        }
    }

    public IEnumerator ZoomInCoroutine()
    {
        yield return StartCoroutine(ZoomCoroutine(Vector3.one * maxScale, zoomInCurve));
    }

    public IEnumerator ZoomOutCoroutine()
    {
        yield return StartCoroutine(ZoomCoroutine(Vector3.zero, zoomInCurve));
    }

    private IEnumerator ZoomCoroutine(Vector3 targetScale, AnimationCurve animationCurve)
    {
        if (circleMask == null)
        {
            yield break;
        }

        Vector3 initialScale = circleMask.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / zoomDuration);
            float curveValue = animationCurve.Evaluate(progress);
            circleMask.localScale = Vector3.LerpUnclamped(initialScale, targetScale, curveValue);
            yield return null;
        }

        circleMask.localScale = targetScale;
    }
}