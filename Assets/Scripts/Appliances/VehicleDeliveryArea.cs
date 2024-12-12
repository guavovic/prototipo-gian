using Prototype.Controllers;
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

        public bool IsOccupied { get; private set; }

        private MeshRenderer _meshRenderer;

        public static event Action OnCorrectVehicleParked;

        private void Start()
        {
            InitializeComponents();
            SetMaterial(noVehicleParkedMaterial);
        }

        private void InitializeComponents()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (IsOccupied)
            {
                return;
            }

            if (other.TryGetComponent<VehicleController>(out var vehicle))
            {
                HandleVehicleEntry(vehicle);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (IsOccupied)
            {
                return;
            }

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
            else
            {
                SetMaterial(noVehicleParkedMaterial);
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
            if (IsCorrectVehicle(vehicle) && !vehicle.IsDriving)
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
                if (IsOccupied)
                {
                    IsOccupied = false;
                }

                SetMaterial(correctVehicleParkedMaterial);
#if UNITY_EDITOR
                Debug.Log("Veículo correto saiu da área!");
#endif
            }
            else
            {
                SetMaterial(wrongVehicleParkeMaterial);
#if UNITY_EDITOR
                Debug.Log("Veículo incorreto saiu da área.");
#endif
            }
        }

        private bool IsCorrectVehicle(VehicleController vehicle)
        {
            foreach (var item in Managers.GameManager.CurrentOrder.Items)
            {
                if (vehicle.Characteristics.Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        private void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }
    }
}