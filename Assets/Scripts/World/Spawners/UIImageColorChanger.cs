using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasRenderer))]
[RequireComponent(typeof(Image))]
public sealed class UIImageColorChanger : MonoBehaviour
{
    [SerializeField] private Gradient colorGradient;

    private Gradient colorGradientCopy;
    private Image _targetImage;

    private void Awake()
    {
        _targetImage = GetComponent<Image>();
    }
    private void OnEnable()
    {
        colorGradientCopy = colorGradient;
    }

    private void OnDisable()
    {
        colorGradientCopy = colorGradient;
    }

    public void UpdateImageColor(float value)
    {
        value = Mathf.Clamp01(value);
        _targetImage.color = colorGradientCopy.Evaluate(value);
    }
}