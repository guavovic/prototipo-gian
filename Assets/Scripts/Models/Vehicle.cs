using Prototype.Data;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Prototype.Models
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Vehicle : MonoBehaviour
    {
        public enum Axel
        {
            Front,
            Rear
        }

        [Serializable]
        public struct Wheel
        {
            public GameObject wheerModel;
            public WheelCollider wheelCollider;
            public Axel axel;
        }

        [SerializeField] private List<Wheel> wheels;

        private Rigidbody _rigidbody;

        public VehicleCharacteristicsData Characteristics { get; private set; }

        public float MaxAcceleration { get; private set; } = 30f;
        public float BrakeAcceleration { get; private set; } = 10f;
        public float TurnSensitivity { get; private set; } = 1f;
        public float MaxSteerAngle { get; private set; } = 45f;
        public Vector3 CenterOfMass { get; private set; }

        public void Setup(VehicleCharacteristicsData characteristics)
        {
            Characteristics = characteristics;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected void Move(Vector3 inputDirection)
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = inputDirection.z * 100 * MaxAcceleration * Time.deltaTime;

            }
        }

        protected void Steer(Vector3 inputDirection)
        {
            foreach (var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var steerAngle = inputDirection.x * TurnSensitivity * MaxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                }
            }
        }

        protected void Brake(Vector3 inputDirection, bool forceBreak = false)
        {
            if ((inputDirection.z < 0 && _rigidbody.velocity.z > 0.1f) || forceBreak)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 25 * BrakeAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }

                if (_rigidbody.velocity.z < 0.1f && inputDirection.z < 0)
                {
                    foreach (var wheel in wheels)

                    {
                        wheel.wheelCollider.motorTorque = inputDirection.z * 15 * MaxAcceleration * Time.deltaTime;
                    }
                }
            }
        }

        protected void AnimateWheels()
        {
            foreach (var wheel in wheels)
            {
                wheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion quaternion);
                wheel.wheerModel.transform.SetPositionAndRotation(position, quaternion);
            }
        }
    }
}