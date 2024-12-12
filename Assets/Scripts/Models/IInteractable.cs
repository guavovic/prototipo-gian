using UnityEngine;

namespace Prototype.Models
{
    public interface IInteractable
    {
        Vector3 InteractionPoint { get; set; }

        void Interact();
    }
}