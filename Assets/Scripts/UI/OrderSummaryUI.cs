using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class OrderSummaryUI : MonoBehaviour
{
    [SerializeField] private GameObject orderSummaryPanel;
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

    private void Start()
    {
        orderSummaryPanel.SetActive(false);
    }

    private void Show(Order order)
    {
        GameState.SetState(GameStatus.Paused);

        itemNametextMeshProUGUI.text = order.Item.Name;
        itemReferenceImage.sprite = order.Item.Sprite;

        orderSummaryPanel.SetActive(true);
    }

    private void Close()
    {
        GameState.SetState(GameStatus.Playing);
        orderSummaryPanel.SetActive(false);
    }

    private void StartRequest()
    {
        SceneLoaderManager.Instance.InitializeSceneTransition(SceneName.Parking);
    }
}
