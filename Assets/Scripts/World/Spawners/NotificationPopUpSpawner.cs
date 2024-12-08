using System.Collections.Generic;
using UnityEngine;

public class NotificationPopUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject servicePopUpPrefab;

    private readonly List<GameObject> activePopUps = new List<GameObject>();
    private readonly HashSet<int> occupiedPositions = new HashSet<int>();

    public void SpawnPopUp(Service service)
    {
        int availableIndex = GetAvailablePositionIndex();

        if (availableIndex != -1)
        {
            Vector3 position = NotificationPopUpSpawnPoints.ServiceNotificationPopUpSpawnPoints[availableIndex].GetPosition();

            GameObject popUp = Instantiate(servicePopUpPrefab, position, Quaternion.Euler(50, -45, 0));

            service.OnServiceCompletedOrExpired += () =>
            {
                activePopUps.Remove(popUp);
                occupiedPositions.Remove(availableIndex);
            };

            popUp.GetComponent<ServiceNotificationPopUp>().Init(service);

            activePopUps.Add(popUp);
            occupiedPositions.Add(availableIndex);
        }
    }

    private int GetAvailablePositionIndex()
    {
        for (int i = 0; i < NotificationPopUpSpawnPoints.ServiceNotificationPopUpSpawnPoints.Length; i++)
        {
            if (!occupiedPositions.Contains(i))
                return i;
        }

        return -1;
    }

    public bool HasAvailablePosition()
    {
        return GetAvailablePositionIndex() != -1;
    }
}