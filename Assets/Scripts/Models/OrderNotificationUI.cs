using Prototype.Managers;
using Prototype.Models;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Prototype.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class OrderNotificationUI : MonoBehaviour, IInteractable
    {
        private const float _FADE_DURATION = 0.5f;
        private CanvasGroup _canvasGroup;
        private UIImageColorChanger _imageColorChanger;
        private TextMeshProUGUI _timerTextMeshPro;

        private readonly Vector3 _directionOffset = new Vector3(0, -2, 0);

        public Order Order { get; private set; }
        public Vector3 InteractionPoint { get; set; }

        public delegate void Open(Order order);
        public static event Open OnOpen;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _imageColorChanger = GetComponentInChildren<UIImageColorChanger>();
            _timerTextMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Setup(Order order)
        {
            Order = order;
        }

        public void Init()
        {
            StartTimer();
        }

        public void StartTimer()
        {
            StartCoroutine(TimerCoroutine());
        }

        public void StopTimer()
        {
            StopCoroutine(TimerCoroutine());
        }

        public void Interact()
        {
            OnOpen?.Invoke(Order);
        }

        private IEnumerator TimerCoroutine()
        {
            yield return StartCoroutine(FadeInCoroutine());

            float elapsedTime = 0f;

            while (elapsedTime < Order.ResponseTimeLimit)
            {
                if (GameManager.GameState.IsPaused)
                {
                    yield return null;
                    continue;
                }

                elapsedTime += Time.deltaTime;
                UpdateTimerDisplay(elapsedTime);
                _imageColorChanger.UpdateImageColor(elapsedTime / Order.ResponseTimeLimit);
                yield return null;
            }

            yield return StartCoroutine(FadeOutCoroutine());

            Order.Expire();
            gameObject.SetActive(false);
        }

        private void UpdateTimerDisplay(float elapsedTime)
        {
            int seconds = (int)elapsedTime;
            int milliseconds = (int)((elapsedTime * 1000) % 1000) / 10;
            _timerTextMeshPro.text = $"{seconds:D2}s{milliseconds:00}";
        }

        private IEnumerator FadeInCoroutine()
        {
            Vector3 targetScale = transform.localScale;
            Vector3 originalPosition = transform.localPosition + (Vector3.up * 1);

            transform.localPosition += _directionOffset;
            _canvasGroup.alpha = 0f;

            float startTime = Time.time;

            while (Time.time - startTime < _FADE_DURATION)
            {
                float t = (Time.time - startTime) / _FADE_DURATION;
                _canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
                transform.localPosition = Vector3.Lerp(originalPosition + _directionOffset, originalPosition, t);
                yield return null;
            }

            transform.localPosition = originalPosition;
            transform.localScale = targetScale;
            _canvasGroup.alpha = 1f;
        }

        private IEnumerator FadeOutCoroutine()
        {
            Vector3 originalPosition = transform.localPosition;

            float startTime = Time.time;

            while (Time.time - startTime < _FADE_DURATION)
            {
                float t = (Time.time - startTime) / _FADE_DURATION;
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
                transform.localPosition = Vector3.Lerp(originalPosition, originalPosition + (Vector3.down * 1) - _directionOffset, t);
                yield return null;
            }

            transform.localPosition = originalPosition + _directionOffset;
            transform.localScale = Vector3.zero;
            _canvasGroup.alpha = 0f;
        }
    }
}