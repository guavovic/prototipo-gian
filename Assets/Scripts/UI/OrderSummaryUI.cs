using Prototype.Managers;
using Prototype.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Prototype.UI
{
    public sealed class OrderSummaryUI : MonoBehaviour
    {
        [SerializeField] private OrderUI orderUIPrefab;
        [Space(10f)]
        [SerializeField] private GameObject orderSummaryPanel;
        [SerializeField] private Transform orderUIsPanelParent;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button startRequestButton;

        private Order _currentOrderSelected;
        private OrderUI[] _orderUIs;

        private void OnEnable()
        {
            OrderNotificationUI.OnOpen += Show;
            closeButton.onClick.AddListener(Close);
            startRequestButton.onClick.AddListener(StartRequest);
        }

        private void OnDisable()
        {
            OrderNotificationUI.OnOpen -= Show;
            closeButton.onClick.RemoveListener(Close);
            startRequestButton.onClick.RemoveListener(StartRequest);
        }

        private void Awake()
        {
            _orderUIs = new OrderUI[6];

            for (int i = 0; i < _orderUIs.Length; i++)
            {
                _orderUIs[i] = Instantiate(orderUIPrefab, orderUIsPanelParent);
            }
        }

        private void Start()
        {
            orderSummaryPanel.SetActive(false);
        }

        private void Show(Order order)
        {
            GameManager.GameState.SetState(GameStatus.Paused);

            _currentOrderSelected = order;

            for (int i = 0; i < _orderUIs.Length; i++)
            {
                var orderIU = _orderUIs[i];

                if (i < order.Items.Length)
                {
                    var item = order.Items[i];
                    orderIU.Setup(item.name, item.Sprite);
                    orderIU.gameObject.SetActive(true);
                }
                else
                {
                    orderIU.gameObject.SetActive(false);
                }
            }

            orderSummaryPanel.SetActive(true);
        }

        private void Close()
        {
            GameManager.GameState.SetState(GameStatus.Playing);
            orderSummaryPanel.SetActive(false);
        }

        private void StartRequest()
        {
            GameManager.SetCurrentOrderSelected(_currentOrderSelected);
            GameManager.SwitchScene(SceneName.Parking);
        }
    }
}