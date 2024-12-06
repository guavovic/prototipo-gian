using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceController : MonoBehaviour
{
    private const float _MAX_SERVICES = 3; 
    private readonly List<Service> _services = new List<Service>();
    private NotificationPopUpSpawner _notificationPopUpSpawner;

    private void Awake()
    {
        _notificationPopUpSpawner = FindObjectOfType<NotificationPopUpSpawner>();
    }

    private void Update()
    {
        if (_services.Count < _MAX_SERVICES)
        {
            GenerateService();
        }
    }

    private void GenerateService()
    {
        Service newService = new Service(id: _services.Count + 1, car: new Car("Fox", Color.green), duration: 10f);

        newService.OnServiceCompletedOrExpired += RemoveService;
        _services.Add(newService);

        _notificationPopUpSpawner.SpawnPopUp(newService);
    }

    private void RemoveService(Service service)
    {
        service.OnServiceCompletedOrExpired -= RemoveService;
        _services.Remove(service);
    }
}