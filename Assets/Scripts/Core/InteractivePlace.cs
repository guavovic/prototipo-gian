using UnityEngine;

public sealed class InteractivePlace : MonoBehaviour, IInteractable
{
    public Vector3 InteractionPoint { get; set; }

    public void Interact() { }
}
