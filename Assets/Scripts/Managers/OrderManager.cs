using Prototype.Data;
using Prototype.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Managers
{
    public sealed class OrderManager : MonoBehaviour
    {
        public const int MAXIMUM_ACTIVE_ORDERS_SIMULTANEOUSLY = 3;
        public const int MIN_VEHICLE_COUNT_PER_ORDER = 2;
        public const int MAX_VEHICLE_COUNT_PER_ORDER = 6;

        private readonly List<Order> _orders = new List<Order>();
        private readonly List<VehicleCharacteristicsData> _vehicleCharacteristicsDatas = new List<VehicleCharacteristicsData>();

        public static event System.Action<Order> OnNewOrderGenerated;

        private void Start()
        {
            InitializeVehicleData();
        }

        private void OnEnable()
        {
            GameManager.OnOfficeSceneStarted += StartOrderGeneration;
        }

        private void OnDisable()
        {
            GameManager.OnOfficeSceneStarted -= StartOrderGeneration;
        }

        private void InitializeVehicleData()
        {
            _vehicleCharacteristicsDatas.AddRange(DataManager.Instance.GetData<VehicleCharacteristicsData>());
        }

        public void StartOrderGeneration()
        {
            StopAllCoroutines();
            StartCoroutine(GenerateOrderRoutine());
        }

        public void StopOrderGeneration()
        {
            StopAllCoroutines();
        }

        private IEnumerator GenerateOrderRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(2f, 4f));

                if (GameManager.GameState.IsPaused)
                {
                    yield return new WaitWhile(() => GameManager.GameState.IsPaused);
                }

                if (CanGenerateOrder())
                {
                    GenerateOrder();
                }
            }
        }

        private bool CanGenerateOrder() =>
            _orders.Count < MAXIMUM_ACTIVE_ORDERS_SIMULTANEOUSLY && OrderNotificationUIManager.HasAvailablePosition();

        private void GenerateOrder()
        {
            const float minResponseTime = 10f;
            const float maxResponseTime = 20f;

            int id = _orders.Count;
            int vehicleCount = Random.Range(MIN_VEHICLE_COUNT_PER_ORDER, MAX_VEHICLE_COUNT_PER_ORDER);

            var datas = new VehicleCharacteristicsData[vehicleCount];

            for (int i = 0; i < vehicleCount; i++)
            {
                datas[i] = _vehicleCharacteristicsDatas[Random.Range(0, _vehicleCharacteristicsDatas.Count)];
            }

            float responseTimeLimit = Random.Range(minResponseTime, maxResponseTime);

            var newOrder = new Order(id, datas, responseTimeLimit);
            _orders.Add(newOrder);

            OnNewOrderGenerated?.Invoke(newOrder);
            SubscribeOrderEvents(newOrder);
        }

        private void RemoveOrder(Order order)
        {
            if (_orders.Remove(order))
            {
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
}