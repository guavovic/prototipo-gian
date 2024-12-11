using UnityEngine;

public abstract class Vehicle : MonoBehaviour, IDrivable
{
    protected float DriveSpeed { get; private set; } = 10f;
    protected float RotationSpeed { get; private set; } = 100f;

    public VehicleCharacteristics VehicleCharacteristics { get; private set; }

    public void Setup(VehicleCharacteristics vehicleCharacteristics)
    {
        VehicleCharacteristics = vehicleCharacteristics;
    }

    public void Move(Vector3 direction, float speed)
    {
        Vector3 movement = speed * Time.deltaTime * direction;
        transform.Translate(movement, Space.World);
    }

    public void Rotate(Vector3 forward, float speed)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.deltaTime);
    }
}
