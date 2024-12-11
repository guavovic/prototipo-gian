using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSummaryWindowController : MonoBehaviour
{
    [SerializeField] private GameObject orderSummaryWindowPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI itemNametextMeshProUGUI;
    [SerializeField] private Image itemReferenceImage;
    [SerializeField] private Button startRequestButton;

    private void OnEnable()
    {
        OrderNotification.OnOpen += Show;
        closeButton.onClick.AddListener(Close);
        startRequestButton.onClick.AddListener(StartRequest);
    }

    private void OnDisable()
    {
        OrderNotification.OnOpen -= Show;
        closeButton.onClick.RemoveListener(Close);
        startRequestButton.onClick.RemoveListener(StartRequest);
    }

    private void Show(Order order)
    {
        GameState.SetStatus(GameStatus.Paused);

        itemNametextMeshProUGUI.text = order.Item.Name;
        itemReferenceImage.sprite = order.Item.Sprite;

        orderSummaryWindowPanel.SetActive(true);
    }

    private void Close()
    {
        GameState.SetStatus(GameStatus.Playing);
        orderSummaryWindowPanel.SetActive(false);
    }

    private void StartRequest()
    {
        GameState.SetStatus(GameStatus.LoadingNewScene);
        //UIManager
    }
}
