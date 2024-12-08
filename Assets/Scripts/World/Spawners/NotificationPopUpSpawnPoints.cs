using UnityEngine;

public sealed class NotificationPopUpSpawnPoints : MonoBehaviour
{
    public static NotificationPopUpSpawnPoint[] ServiceNotificationPopUpSpawnPoints { get; private set; }

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            NotificationPopUpSpawnPoint[] serviceNotificationPopUpSpawnPoints = GetComponentsInChildren<NotificationPopUpSpawnPoint>();

            if (serviceNotificationPopUpSpawnPoints != null)
            {
                ServiceNotificationPopUpSpawnPoints = new NotificationPopUpSpawnPoint[transform.childCount];
                ServiceNotificationPopUpSpawnPoints = serviceNotificationPopUpSpawnPoints;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("N�o foi encontrando nenhum ponto de spawn setado.");
#endif
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("N�o h� pontos de spawn setados.");
#endif
        }
    }
}