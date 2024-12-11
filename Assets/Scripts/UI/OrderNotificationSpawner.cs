using System.Collections.Generic;
using UnityEngine;

public sealed class OrderNotificationSpawner : MonoBehaviour
{
    [SerializeField] private OrderNotification orderNotificationPrefab;

    private OrderNotificationSpawnPoint[] _spawnPoints;

    private readonly List<OrderNotification> _activeNotifications = new List<OrderNotification>();
    private readonly HashSet<int> _occupiedPositions = new HashSet<int>();

    private void Awake()
    {
        _spawnPoints = FindObjectsOfType<OrderNotificationSpawnPoint>(true);
    }

    public void SpawnNotification(Order order)
    {
        int availableIndex = GetAvailablePositionIndex();

        if (availableIndex == -1)
        {
#if UNITY_EDITOR
            Debug.LogWarning("No available spawn points for order notifications.");
#endif
            return;
        }

        Vector3 position = _spawnPoints[availableIndex].GetPosition();

        GameObject notificationGO = Instantiate(
            orderNotificationPrefab.gameObject,
            position,
            Quaternion.Euler(60, -90, 0)
        );

        if (!notificationGO.TryGetComponent<OrderNotification>(out var orderNotification))
        {
#if UNITY_EDITOR
            Debug.LogError("OrderNotification component is missing on the prefab!");
#endif
            return;
        }

        orderNotification.Setup(order, availableIndex);

        _activeNotifications.Add(orderNotification);
        _occupiedPositions.Add(availableIndex);

        order.OnDelivered += (deliveredOrder) => HandleOrderCompletion(orderNotification, availableIndex);
        order.OnExpired += (expiredOrder) => HandleOrderCompletion(orderNotification, availableIndex);

        orderNotification.Init();
    }

    private void HandleOrderCompletion(OrderNotification notification, int positionIndex)
    {
        if (_activeNotifications.Contains(notification))
        {
            _activeNotifications.Remove(notification);
            _occupiedPositions.Remove(positionIndex);
        }
    }

    private int GetAvailablePositionIndex()
    {
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (!_occupiedPositions.Contains(i))
                return i;
        }

        return -1;
    }

    public bool HasAvailablePosition()
    {
        return GetAvailablePositionIndex() != -1;
    }
}
