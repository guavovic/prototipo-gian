using Prototype.Data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Prototype.Models
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Vehicle : MonoBehaviour
    {
        [Serializable]
        public struct Wheel
        {
            public GameObject wheerModel;
            public WheelCollider wheelCollider;
            public Axel axel;
        }

        [SerializeField] private List<Wheel> wheels;

        protected string VehicleName { get; private set; }
        protected Material VehicleMaterial { get; private set; }
        protected float MaxAcceleration { get; } = 30f;
        protected float BrakeAcceleration { get; } = 10f;
        protected float TurnSensitivity { get; } = 1f;
        protected float MaxSteerAngle { get; } = 45f;
        protected Vector3 CenterOfMass { get; }
        protected List<Wheel> Wheels { get => wheels; }

        public virtual void Setup(string name, Material material)
        {
            VehicleName = name;
            VehicleMaterial = material;
        }

        protected abstract void Move();
        protected abstract void Steer();
        protected abstract void Brake(bool forceBreak = false);

        protected virtual void DebugVehicleInfo()
        {
            Debug.Log($"Vehicle: {VehicleName}");
        }
    }
}