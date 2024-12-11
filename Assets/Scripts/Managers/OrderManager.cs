using Prototype.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private VehicleCharacteristicsData[] characteristicsData;

    private const int MAX_SERVICES = 3;
    private readonly List<Order> _services = new List<Order>();
    private Coroutine _generateOrderCoroutine;

    private OrderNotificationSpawner _notificationSpawner;

    private void Awake()
    {
        _notificationSpawner = FindObjectOfType<OrderNotificationSpawner>();

        if (_notificationSpawner == null)
        {
            Debug.LogError("OrderNotificationSpawner not found in the scene!");
        }
    }

    private void Start()
    {
        StartOrderGeneration();
    }

    public void StartOrderGeneration()
    {
        if (_generateOrderCoroutine == null)
        {
            _generateOrderCoroutine = StartCoroutine(GenerateOrderRoutine());
        }
    }

    public void StopOrderGeneration()
    {
        if (_generateOrderCoroutine != null)
        {
            StopCoroutine(_generateOrderCoroutine);
            _generateOrderCoroutine = null;
        }
    }

    private IEnumerator GenerateOrderRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));

            while (GameState.IsPaused)
            {
                yield return null;
            }

            if (CanGenerateOrder())
            {
                GenerateOrder();
            }
        }
    }

    private bool CanGenerateOrder()
    {
        return _services.Count < MAX_SERVICES && _notificationSpawner.HasAvailablePosition();
    }

    private void GenerateOrder()
    {
        // TODO: Melhorar logica das caracteristicas do veiculo
        var characteristicData = characteristicsData[Random.Range(0, characteristicsData.Length)];

        var newOrder = new Order(
            id: _services.Count,
            item: characteristicData,
            duration: Random.Range(30f, 60f)
        );

        _services.Add(newOrder);
        SubscribeOrderEvents(newOrder);
        _notificationSpawner.SpawnNotification(newOrder);
    }

    private void RemoveOrder(Order order)
    {
        if (_services.Contains(order))
        {
            _services.Remove(order);
            UnsubscribeOrderEvents(order);
        }
    }

    private void SubscribeOrderEvents(Order order)
    {
        order.OnDelivered += RemoveOrder;
        order.OnExpired += RemoveOrder;
    }

    private void UnsubscribeOrderEvents(Order order)
    {
        order.OnDelivered -= RemoveOrder;
        order.OnExpired -= RemoveOrder;
    }
}