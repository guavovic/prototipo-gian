using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Prototype.Controllers
{

    public sealed class VehicleController : Models.Vehicle, Models.IInteractable
    {
        private Vector3 _inputDirection;

        public bool IsBlocked { get; private set; }
        public bool IsDriving { get; private set; }
        public Vector3 InteractionPoint { get; set; }

        public void Interact()
        {
            if (IsBlocked)
                return;

            IsDriving = true;
        }

        public void OnDrive(InputAction.CallbackContext context)
        {
            if (!IsDriving)
                return;

            Vector2 input = context.ReadValue<Vector2>();
            _inputDirection = new Vector3(input.x, 0, input.y);
        }

        private void Update()
        {
            base.AnimateWheels();
        }

        private void LateUpdate()
        {
            if (IsBlocked)
            {
                return;
            }

            base.Move(_inputDirection);
            base.Steer(_inputDirection);
            base.Brake(_inputDirection);
        }

        public void ExitVehicle()
        {
            IsDriving = false;
            _inputDirection = Vector3.zero;
        }

        public void BlockEntry()
        {
            IsBlocked = true;
            ExitVehicle();
        }
    }
}
