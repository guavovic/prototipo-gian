using System.Collections.Generic;
using UnityEngine;

namespace Prototype.Models
{
    public sealed class VehicleBody : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetMaterial(Material material)
        {
            List<Material> vehicleBodyMaterials = new List<Material>() { material, _meshRenderer.materials[1] }; // Specific order for this material
            _meshRenderer.SetSharedMaterials(vehicleBodyMaterials);
        }
    }
}