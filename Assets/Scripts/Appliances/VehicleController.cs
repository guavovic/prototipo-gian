using UnityEngine;
using UnityEngine.InputSystem;

public sealed class VehicleController : Vehicle, IInteractable
{
    private InputAction driveAction;
    private bool isDriving;

    public Vector3 InteractionPoint { get; set; }

    public void Interact()
    {
        EnableControls();
    }

    private void OnDisable()
    {
        DisableControls();
    }

    private void EnableControls()
    {
        isDriving = true;
        driveAction = new InputAction(binding: "ssas");
        driveAction.performed += OnDrive;
        driveAction.canceled += OnDrive;
        driveAction.Enable();
    }

    private void DisableControls()
    {
        if (driveAction != null)
        {
            driveAction.performed -= OnDrive;
            driveAction.canceled -= OnDrive;
        }

        isDriving = false;
    }

    private void OnDrive(InputAction.CallbackContext context)
    {
        if (!isDriving)
            return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0, input.y);

        base.Move(direction, base.DriveSpeed);

        if (input != Vector2.zero)
        {
            base.Rotate(direction, base.RotationSpeed);
        }
    }

    public void ExitVehicle()
    {
        DisableControls();
        InputController.ReleaseControl();
    }
}