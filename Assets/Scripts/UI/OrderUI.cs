using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.UI
{
    public sealed class OrderUI : MonoBehaviour
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private TextMeshProUGUI itemNametextMeshProUGUI;
        [SerializeField] private Image iconImage;

        public void Setup(string name, Sprite icon)
        {
            itemNametextMeshProUGUI.text = name;
            iconImage.sprite = icon;
        }
    }
}