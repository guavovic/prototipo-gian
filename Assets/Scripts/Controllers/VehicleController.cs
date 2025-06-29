using Prototype.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype.Controllers
{
    public sealed class VehicleController : Models.Vehicle, IInteractable
    {
        private Vector3 _inputDirection;

        private Rigidbody _rigidbody;

        public bool IsBlocked { get; private set; }
        public bool IsDriving { get; private set; }
        public VehicleBody VehicleBody { get; private set; }
        public Vector3 InteractionPoint { get; set; }

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _rigidbody = GetComponent<Rigidbody>();
            VehicleBody = GetComponentInChildren<VehicleBody>();
        }

        public override void Setup(string name, Material material)
        {
            base.Setup(name, material);

            // Set game object name
            gameObject.name = $"Vehicle_{name}";

            VehicleBody.SetMaterial(base.VehicleMaterial);
        }

        public void Interact()
        {
            base.DebugVehicleInfo();

            if (IsBlocked)
                return;

            if (IsDriving)
            {
                Exit();
                return;
            }

            IsDriving = true;
        }

        public void OnDrive(InputAction.CallbackContext context)
        {
            if (!IsDriving && IsBlocked)
                return;

            Vector2 input = context.ReadValue<Vector2>();
            _inputDirection = new Vector3(input.x, 0, input.y);
        }

        private void LateUpdate()
        {
            if (IsBlocked)
                return;

            Move();
            Steer();
            Brake();
        }

        protected override void Move()
        {
            foreach (var wheel in base.Wheels)
            {
                wheel.wheelCollider.motorTorque = _inputDirection.z * 100 * MaxAcceleration * Time.deltaTime;
            }
        }

        protected override void Steer()
        {
            foreach (var wheel in base.Wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var steerAngle = _inputDirection.x * TurnSensitivity * MaxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                }
            }

            AnimateWheels();
        }

        protected override void Brake(bool forceBreak = false)
        {
            if ((_inputDirection.z < 0 && _rigidbody.velocity.z > 0.1f) || forceBreak)
            {
                foreach (var wheel in base.Wheels)
                {
                    wheel.wheelCollider.brakeTorque = 25 * BrakeAcceleration * Time.deltaTime;
                }
            }
            else
            {
                foreach (var wheel in base.Wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }

                if (_rigidbody.velocity.z < 0.1f && _inputDirection.z < 0)
                {
                    foreach (var wheel in base.Wheels)

                    {
                        wheel.wheelCollider.motorTorque = _inputDirection.z * 15 * MaxAcceleration * Time.deltaTime;
                    }
                }
            }
        }

        public void BlockEntry()
        {
            IsBlocked = true;
            Exit();
        }

        private void Exit()
        {
            IsDriving = false;
            _inputDirection = Vector3.zero;
        }

        private void AnimateWheels()
        {
            foreach (var wheel in base.Wheels)
            {
                wheel.wheelCollider.GetWorldPose(out Vector3 position, out Quaternion quaternion);
                wheel.wheerModel.transform.SetPositionAndRotation(position, Quaternion.Euler(90, 0, 0));
            }
        }
    }
}