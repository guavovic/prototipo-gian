using System;

public class Service
{
    public int Id { get; private set; }
    public Car Car { get; private set; }
    public float Duration { get; private set; }
    public Status CurrentStatus { get; private set; } = Status.Stopped;

    public event Action OnServiceCompletedOrExpired;

    public Service(int id, Car car, float duration)
    {
        Id = id;
        Car = car;
        Duration = duration;
    }

    public void CompleteService()
    {
        SetStatus(Status.Complete);
        OnServiceCompletedOrExpired?.Invoke();
    }

    public void ExpireService()
    {
        SetStatus(Status.Expired);
        OnServiceCompletedOrExpired?.Invoke();
    }

    public void SetStatus(Status newStatus)
    {
        CurrentStatus = newStatus;
    }
}