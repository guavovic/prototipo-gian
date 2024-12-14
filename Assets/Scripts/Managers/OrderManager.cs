using Prototype.Data;
using Prototype.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Managers
{
    public sealed class OrderManager : MonoBehaviour
    {
        public List<VehicleCharacteristicsData> vr;

        private readonly List<Order> _orders = new List<Order>();

        public static event System.Action<Order> OnNewOrderGenerated;

        private void OnEnable()
        {
            GameManager.OnGameStarted += StartOrderGeneration;
        }

        private void OnDisable()
        {
            GameManager.OnGameStarted -= StartOrderGeneration;
        }

        public void StartOrderGeneration()
        {
            StartCoroutine(GenerateOrderRoutine());
        }

        public void StopOrderGeneration()
        {
            StopCoroutine(GenerateOrderRoutine());
        }

        private IEnumerator GenerateOrderRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(2f, 4f));

                while (GameManager.GameState.IsPaused)
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
            return _orders.Count < GameManager.MAXIMUM_ACTIVE_ORDERS_SIMULTANEOUSLY && OrderNotificationUIManager.HasAvailablePosition();
        }

        private void GenerateOrder()
        {
            int id = _orders.Count;

            var vehicleCharacteristicsDatas = new VehicleCharacteristicsData[Random.Range(GameManager.MIN_VEHICLE_COUNT, GameManager.MAX_VEHICLE_COUNT)];
            vr = new List<VehicleCharacteristicsData>();


            for (int i = 0; i < vehicleCharacteristicsDatas.Length; i++)
            {
                vr = DataManager.Instance.GetData<VehicleCharacteristicsData>();
                vehicleCharacteristicsDatas[i] = vr[Random.Range(0, vr.Count)];
            }

            const float minResponseTimeLimit = 10f;
            const float maxResponseTimeLimit = 20f;
            float responseTimeLimit = Random.Range(minResponseTimeLimit, maxResponseTimeLimit);

            var newOrder = new Order(id, vehicleCharacteristicsDatas, responseTimeLimit);

            _orders.Add(newOrder);
            OnNewOrderGenerated?.Invoke(newOrder);
            SubscribeOrderEvents(newOrder);
        }

        private void RemoveOrder(Order order)
        {
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
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