using System;

public class Order : IStatus
{
    public int Id { get; private set; }
    public VehicleCharacteristics Item { get; private set; }
    public float Duration { get; private set; }
    public Status CurrentStatus { get; set; }

    public delegate void Expired(Order order);
    public event Expired OnExpired;

    public delegate void Delivered(Order order);
    public event Delivered OnDelivered;

    public delegate void AlertTime(Order order);
    public event AlertTime OnAlertTime;

    public Order(int id, VehicleCharacteristics item, float duration)
    {
        Id = id;
        Item = item;
        Duration = duration;
    }

    public void Delivery()
    {
        CurrentStatus = Status.Complete;
        OnDelivered?.Invoke(this);
    }

    public void Expire()
    {
        CurrentStatus = Status.Expired;
        OnExpired?.Invoke(this);
    }
}