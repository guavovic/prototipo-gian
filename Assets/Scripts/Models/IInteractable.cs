using UnityEngine;

public interface IInteractable
{
    Vector3 InteractionPoint { get; set; }

    void Interact();
}