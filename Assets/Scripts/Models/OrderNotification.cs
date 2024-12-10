using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(CanvasGroup))]
public sealed class OrderNotification : MonoBehaviour
{
    private readonly float _fadeDuration = 0.5f;
    private CanvasGroup _canvasGroup;
    private UIImageColorChanger _uIImageColorChanger;
    private TextMeshProUGUI _timer_textMeshPro;

    public Order Order { get; private set; }
    public int AvailableIndex { get; private set; }

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _uIImageColorChanger = GetComponentInChildren<UIImageColorChanger>();
        _timer_textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Setup(Order service, int availableIndex)
    {
        Order = service;
        AvailableIndex = availableIndex;
    }

    public void Init()
    {
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

        while (elapsedTime < Order.Duration)
        {
            elapsedTime += Time.deltaTime;

            string seconds = ((int)elapsedTime).ToString("D2");
            string milliseconds = ((elapsedTime * 1000 % 1000) / 10).ToString("00");
            _timer_textMeshPro.text = $"{seconds}s{milliseconds}";
            _uIImageColorChanger.UpdateImageColor(elapsedTime / Order.Duration);
            yield return null;
        }

        yield return StartCoroutine(FadeAndReScaleAnimation.FadeOutAndScaleCoroutine(transform, _canvasGroup, _fadeDuration));
        Order.Expire();
        transform.gameObject.SetActive(false);
    }
}