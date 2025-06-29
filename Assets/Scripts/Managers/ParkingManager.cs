using Prototype.Appliances;
using Prototype.Controllers;
using Prototype.Data;
using Prototype.Models;
using Prototype.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Prototype.Managers
{
    public sealed class ParkingManager : MonoBehaviour
    {
        private VehicleDeliveryArea[] _vehicleDeliveryAreas;
        private VehicleController[] _vehicleControllers;
        private Order _currentOrder;
        private int _vehiclesNeeded;
        private int _carsDelivered;

        public static List<VehicleController> CorrectVehicles { get; private set; } = new List<VehicleController>();

        private void Awake()
        {
            InitialiazeComponents();
        }

        private void InitialiazeComponents()
        {
            _vehicleDeliveryAreas = FindObjectsOfType<VehicleDeliveryArea>(true);
            _vehicleControllers = FindObjectsOfType<VehicleController>(true);
        }

        private void OnEnable()
        {
            GameManager.OnParkingSceneStarted += InitializeParking;
            VehicleDeliveryArea.OnCorrectVehicleParked += HandleVehicleDelivery;
        }

        private void OnDisable()
        {
            GameManager.OnParkingSceneStarted -= InitializeParking;
            VehicleDeliveryArea.OnCorrectVehicleParked -= HandleVehicleDelivery;
        }

        private void InitializeParking()
        {
            _currentOrder = GameManager.CurrentOrder;
            _vehiclesNeeded = _currentOrder.Items.Length;

            InitializeParkingAreas();
            ConfigureParkingVehicles();
        }

        private void InitializeParkingAreas()
        {
            foreach (var area in _vehicleDeliveryAreas)
            {
                area.gameObject.SetActive(false);
            }

            for (int i = 0; i < _vehiclesNeeded; i++)
            {
                _vehicleDeliveryAreas[i].gameObject.SetActive(true);
            }
        }

        private void ConfigureParkingVehicles()
        {
            var vehicleData = DataManager.Instance.GetData<VehicleCharacteristicsData>();

            // Filtrar os veículos disponíveis que não estão na ordem atual
            var currentOrderItems = _currentOrder.Items.Select(item => item.Name).ToHashSet();
            var availableRandomVehicles = vehicleData.Where(vehicle => !currentOrderItems.Contains(vehicle.Name)).ToList();

            // Configurar veículos aleatórios para os que restaram
            foreach (var vehicle in _vehicleControllers)
            {
                var randomCharacteristics = availableRandomVehicles[Random.Range(0, availableRandomVehicles.Count)];
                vehicle.Setup("Random", randomCharacteristics.Material);
            }

            // Selecionar os veículos necessários para a ordem atual
            var availableVehicles = _vehicleControllers.Take(_vehiclesNeeded).ToList();

            foreach (var vehicle in availableVehicles)
            {
                var requiredCharacteristics = _currentOrder.Items[Random.Range(0, _currentOrder.Items.Length)];
                vehicle.Setup(requiredCharacteristics.Name, requiredCharacteristics.Material);
                CorrectVehicles.Add(vehicle);
            }
        }


        private void HandleVehicleDelivery()
        {
            _carsDelivered++;

            if (_carsDelivered >= _vehiclesNeeded)
            {
                Debug.Log("All vehicles have been delivered successfully.");
            }
        }
    }
}
