using UnityEngine;

namespace Prototype.UI
{
    public sealed class OrderNotificationUISpawnPoint : MonoBehaviour
    {
        public Vector3 GetPosition()
        {
            return transform.position;
        }
    }
}