using Prototype.Controllers;
using Prototype.Managers;
using System;
using UnityEngine;

namespace Prototype.Appliances
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(BoxCollider))]
    public sealed class VehicleDeliveryArea : MonoBehaviour
    {
        [Header("State Materials")]
        [SerializeField] private Material noVehicleParkedMaterial;
        [SerializeField] private Material wrongVehicleParkeMaterial;
        [SerializeField] private Material correctVehicleParkedMaterial;

        private MeshRenderer _meshRenderer;

        public bool IsOccupied { get; private set; }

        public static event Action OnCorrectVehicleParked;

        private void Start()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            SetMaterial(noVehicleParkedMaterial);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOccupied)
                return;

            if (other.TryGetComponent<VehicleController>(out var vehicle))
            {
                HandleVehicleEntry(vehicle);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsOccupied)
                return;

            if (other.TryGetComponent<VehicleController>(out var vehicle))
            {
                HandleVehicleStay(vehicle);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<VehicleController>(out var vehicle))
            {
                HandleVehicleExit(vehicle);
            }
        }

        private void HandleVehicleEntry(VehicleController vehicle)
        {
            if (IsCorrectVehicle(vehicle))
            {
                SetMaterial(correctVehicleParkedMaterial);
#if UNITY_EDITOR
                Debug.Log("Veículo correto na área!");
#endif
            }
            else
            {
                SetMaterial(wrongVehicleParkeMaterial);
#if UNITY_EDITOR
                Debug.Log("Veículo incorreto na área.");
#endif
            }
        }

        private void HandleVehicleStay(VehicleController vehicle)
        {
            if (IsCorrectVehicle(vehicle) && vehicle.IsDriving)
            {
                IsOccupied = true;
                vehicle.BlockEntry();
                OnCorrectVehicleParked?.Invoke();
#if UNITY_EDITOR
                Debug.Log("Veículo correto estacionado!");
#endif
            }
        }

        private void HandleVehicleExit(VehicleController vehicle)
        {
            if (IsCorrectVehicle(vehicle))
            {
#if UNITY_EDITOR
                Debug.Log("Veículo correto saiu da área!");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Veículo incorreto saiu da área.");
#endif
            }

            IsOccupied = false;
            SetMaterial(noVehicleParkedMaterial);
        }

        private bool IsCorrectVehicle(VehicleController vehicle)
        {
            foreach (var correctVehicle in ParkingManager.CorrectVehicles)
            {
                if (vehicle.Equals(correctVehicle))
                    return true;
            }

            return false;
        }

        private void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
    }
}