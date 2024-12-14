using UnityEngine;

namespace Prototype.Data
{
    [CreateAssetMenu(fileName = "VehicleCharacteristicsData", menuName = "ScriptableObjects/VehicleCharacteristicsData", order = 2)]
    public sealed class VehicleCharacteristicsData : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
        public Color Color;
    }
}