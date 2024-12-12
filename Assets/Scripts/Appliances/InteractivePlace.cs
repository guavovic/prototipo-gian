using UnityEngine;

namespace Prototype.Appliances
{
    public sealed class InteractivePlace : MonoBehaviour, Models.IInteractable
    {
        public Vector3 InteractionPoint { get; set; }

        public void Interact() { }
    }
}