using System.Collections.Generic;
using UnityEngine;

public class NotificationPopUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject servicePopUpPrefab;

    private readonly List<GameObject> activePopUps = new List<GameObject>();

    public void SpawnPopUp(Service service)
    {
        GameObject popUp = Instantiate(servicePopUpPrefab, GetAvailablePosition(), Quaternion.identity);
        activePopUps.Add(popUp);

        ServiceNotificationPopUp popUpScript = popUp.GetComponent<ServiceNotificationPopUp>();
        popUpScript.SetupPopUp(service);
        popUpScript.Init();
    }

    public void HandlePopUpExpiration(Service service)
    {
        foreach (var popUp in activePopUps)
        {
            ServiceNotificationPopUp popUpScript = popUp.GetComponent<ServiceNotificationPopUp>();
            if (popUpScript.Service == service)
            {
                AnimationController animationController = popUp.GetComponent<AnimationController>();
                animationController.FadeOutAndScale();
                Destroy(popUp, fadeDuration);
                activePopUps.Remove(popUp);
                break;
            }
        }
    }

    private Vector3 GetAvailablePosition()
    {
        return NotificationPopUpSpawnPoints.ServiceNotificationPopUpSpawnPoints[Random.Range(0, NotificationPopUpSpawnPoints.ServiceNotificationPopUpSpawnPoints.Length)].GetPosition();
    }
}