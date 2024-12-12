using Prototype.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Managers
{
    public sealed class OrderNotificationUIManager : MonoBehaviour
    {
        [SerializeField] private OrderNotificationUI orderNotificationUIPrefab;

        private static OrderNotificationUISpawnPoint[] _spawnPoints;

        private readonly List<OrderNotificationUI> _activeNotifications = new List<OrderNotificationUI>();
        private static readonly HashSet<int> _occupiedPositions = new HashSet<int>();

        private void Awake()
        {
            _spawnPoints = FindObjectsOfType<OrderNotificationUISpawnPoint>(true);
        }

        private void OnEnable()
        {
            OrderManager.OnNewOrderGenerated += SpawnNotification;
        }

        private void OnDisable()
        {
            OrderManager.OnNewOrderGenerated -= SpawnNotification;
        }

        public void SpawnNotification(Models.Order order)
        {
            int availableIndex = GetAvailablePositionIndex();

            if (availableIndex == -1)
            {
#if UNITY_EDITOR
                Debug.LogWarning("No available spawn points for order notifications.");
#endif
                return;
            }

            // TODO: Organizar e melhorar essa logica aqui
            GameObject orderNotificationUIGameObject = Instantiate(
                original: orderNotificationUIPrefab.gameObject,
                position: _spawnPoints[availableIndex].GetPosition(),
                rotation: Quaternion.Euler(60, -90, 0),
                parent: _spawnPoints[availableIndex].transform
                );

            orderNotificationUIGameObject.SetActive(true);

            if (!orderNotificationUIGameObject.TryGetComponent<OrderNotificationUI>(out var orderNotificationUI))
            {
#if UNITY_EDITOR
                Debug.LogError("OrderNotification component is missing on the prefab!");
#endif
                return;
            }

            orderNotificationUI.Setup(order);

            _activeNotifications.Add(orderNotificationUI);
            _occupiedPositions.Add(availableIndex);

            order.OnDelivered += (deliveredOrder) => HandleOrderCompletion(orderNotificationUI, availableIndex);
            order.OnExpired += (expiredOrder) => HandleOrderCompletion(orderNotificationUI, availableIndex);

            orderNotificationUI.Init();
        }

        private void HandleOrderCompletion(OrderNotificationUI notification, int positionIndex)
        {
            if (_activeNotifications.Contains(notification))
            {
                _activeNotifications.Remove(notification);
                _occupiedPositions.Remove(positionIndex);
            }
        }

        private static int GetAvailablePositionIndex()
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (!_occupiedPositions.Contains(i))
                    return i;
            }

            return -1;
        }

        public static bool HasAvailablePosition()
        {
            return GetAvailablePositionIndex() != -1;
        }
    }
}