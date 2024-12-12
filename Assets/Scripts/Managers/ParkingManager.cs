using Prototype.Appliances;
using System.Linq;
using UnityEngine;

namespace Prototype.Managers
{
    public sealed class ParkingManager : MonoBehaviour
    {
        public static int VehiclesNeeded { get; private set; }

        private int _carsDelivered = 0;

        private void Awake()
        {
            OrganizeParking();
        }

        private void OnEnable()
        {
            VehicleDeliveryArea.OnCorrectVehicleParked += DeliverVehicle;
        }

        private void OnDisable()
        {
            VehicleDeliveryArea.OnCorrectVehicleParked -= DeliverVehicle;
        }

        private void OrganizeParking()
        {
            VehiclesNeeded = GameManager.CurrentOrder.Items.Length;

            var vehicleDeliveryAreas = FindObjectsOfType<VehicleDeliveryArea>();

            vehicleDeliveryAreas.ToList().ForEach(vehicleDeliveryArea => vehicleDeliveryArea.gameObject.SetActive(false));

            for (int i = 0; i < VehiclesNeeded; i++)
            {
                vehicleDeliveryAreas[i].gameObject.SetActive(true);
            }
        }

        private void DeliverVehicle()
        {
            _carsDelivered++;

            if (_carsDelivered >= VehiclesNeeded)
            {
                Debug.Log("Entregou todos os carros");
            }
        }
    }
}