using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceController : MonoBehaviour
{
    private const int MAX_SERVICES = 3;
    private readonly List<Service> _services = new List<Service>();
    private NotificationPopUpSpawner _notificationPopUpSpawner;

    private void Awake()
    {
        _notificationPopUpSpawner = FindObjectOfType<NotificationPopUpSpawner>();
    }

    private void Start()
    {
        StartCoroutine(SpawnServiceRoutine());
    }

    private IEnumerator SpawnServiceRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));

            if (_services.Count < MAX_SERVICES && _notificationPopUpSpawner.HasAvailablePosition())
            {
                GenerateService();
            }
        }
    }

    private void GenerateService()
    {
        Service newService = new Service(id: _services.Count + 1, car: new Car("Fox", Color.green), duration: Random.Range(30f, 60f));

        newService.OnServiceCompletedOrExpired += () =>
        {
            _services.Remove(newService);
        };

        _services.Add(newService);

        _notificationPopUpSpawner.SpawnPopUp(newService);
    }
}