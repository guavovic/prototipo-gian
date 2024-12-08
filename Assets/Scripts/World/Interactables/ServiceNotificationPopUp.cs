using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public sealed class ServiceNotificationPopUp : MonoBehaviour
{
    private readonly float _fadeDuration = 0.5f;
    private CanvasGroup _canvasGroup;
    private UIImageColorChanger _uIImageColorChanger;
    private TextMeshProUGUI _timer_textMeshPro;

    public Service Service { get; private set; }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _uIImageColorChanger = GetComponentInChildren<UIImageColorChanger>();
        _timer_textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(Service service)
    {
        Service = service;
        StartTimer();
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
        yield return StartCoroutine(FadeAndReScaleAnimation.FadeInAndScaleCoroutine(transform, _canvasGroup, _fadeDuration));

        float elapsedTime = 0f;

        while (elapsedTime < Service.Duration)
        {
            elapsedTime += Time.deltaTime;

            string seconds = ((int)elapsedTime).ToString("D2");
            string milliseconds = ((elapsedTime * 1000 % 1000) / 10).ToString("00");
            _timer_textMeshPro.text = $"{seconds}s{milliseconds}";
            _uIImageColorChanger.UpdateImageColor(elapsedTime / Service.Duration);
            yield return null;
        }

        yield return StartCoroutine(FadeAndReScaleAnimation.FadeOutAndScaleCoroutine(transform, _canvasGroup, _fadeDuration));
        Service.ExpireService();
        transform.gameObject.SetActive(false);
    }
}