using Store.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store;

public class Order
{
    private readonly OrderDto _dto;

    public int Id => _dto.Id;
    public string PhoneNumber
    {
        get => _dto.PhoneNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(PhoneNumber));
            }
            _dto.PhoneNumber = value;
        }
    }

    public OrderItemCollection Items { get; }
    public decimal TotalPrice => Items.Sum(i => i.Price * i.Count) + (Delivery?.Price ?? 0m);
    public int TotalCount => Items.Sum(i => i.Count);

    public OrderDelivery Delivery
    {
        get
        {
            if (_dto.DeliveryServiceName == null)
            {
                return null;
            }

            return new OrderDelivery(
                _dto.DeliveryDescription,
                _dto.DeliveryServiceName,
                _dto.DeliveryPrice,
                _dto.DeliveryParameters);
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentException(nameof(Delivery));
            }

            _dto.DeliveryServiceName = value.ServiceName;
            _dto.DeliveryDescription = value.Description;
            _dto.DeliveryPrice = value.Price;
            _dto.DeliveryParameters = value.Parameters.ToDictionary(p => p.Key, p => p.Value);
        }
    }

    public OrderPayment Payment
    {
        get
        {
            if (_dto.PaymentServiceName == null)
            {
                return null;
            }

            return new OrderPayment(
                _dto.PaymentServiceName,
                _dto.PaymentDescription,
                _dto.PaymentParameters);
        }
        set
        {
            if (value == null)
            {
                throw new ArgumentException(nameof(Payment));
            }

            _dto.PaymentServiceName = value.ServiceName;
            _dto.PaymentDescription = value.Description;
            _dto.PaymentParameters = value.Parameters.ToDictionary(p => p.Key, p => p.Value);
        }
    }
    
    public Order(OrderDto orderDto)
    {
        _dto = orderDto;
        Items = new OrderItemCollection(orderDto);
    }
    
    public static class DtoFactory
    {
        public static OrderDto Create => new OrderDto();
    }
    
    public static class Mapper
    {
        public static OrderDto Map(Order order) => order._dto;
        public static Order Map(OrderDto dto) => new Order(dto);
    }
}