using Prototype.Data;

namespace Prototype.Models
{
    public sealed class Order
    {
        public int Id { get; private set; }
        public VehicleCharacteristicsData[] Items { get; private set; }
        public float ResponseTimeLimit { get; private set; }
        public OrderStatus CurrentStatus { get; private set; }

        public delegate void Expired(Order order);
        public event Expired OnExpired;

        public delegate void Delivered(Order order);
        public event Delivered OnDelivered;

        public delegate void AlertTime(Order order);
        public event AlertTime OnAlertTime;

        public Order(int id, VehicleCharacteristicsData[] items, float responseTimeLimit)
        {
            Id = id;
            Items = items;
            ResponseTimeLimit = responseTimeLimit;
        }

        public void Delivery()
        {
            CurrentStatus = OrderStatus.Complete;
            OnDelivered?.Invoke(this);
        }

        public void Expire()
        {
            CurrentStatus = OrderStatus.Expired;
            OnExpired?.Invoke(this);
        }
    }
}