using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public sealed class ServiceNotificationPopUp : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float scaleDuration = 0.5f;

    private CanvasGroup canvasGroup;

    public Service Service { get; private set; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetService(Service service)
    {
        Service = service;
    }

    public void Init()
    {
        StartFadeInAndScaleCoroutine();
        StartTimer();
    }

    public void Finish()
    {
        StartFadeOutAndScaleCoroutine();
    }


    public void StartFadeInAndScaleCoroutine()
    {
        StartTimer();
        StartCoroutine(FadeAndReScaleAnimation.FadeInAndScaleCoroutine(transform, canvasGroup, fadeDuration, scaleDuration));
    }

    public void StartFadeOutAndScaleCoroutine()
    {
        StopTimer();
        StartCoroutine(FadeAndReScaleAnimation.FadeOutAndScaleCoroutine(transform, canvasGroup, fadeDuration, scaleDuration));
    }


    public void StartTimer()
    {
        StartCoroutine(ServiceTimerCoroutine());
    }

    public void StopTimer()
    {
        StopCoroutine(ServiceTimerCoroutine());
    }

    private IEnumerator ServiceTimerCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < Service.Duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Service.ExpireService();
    }
}