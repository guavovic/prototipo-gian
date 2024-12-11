using UnityEngine;

public interface IDrivable
{
    void Move(Vector3 direction, float speed);
    void Rotate(Vector3 forward, float speed);
}
