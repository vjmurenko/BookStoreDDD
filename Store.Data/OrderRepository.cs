using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Data;

public class OrderRepository : IOrderRepository
{
    private readonly List<Order> _orders = new();
    public void Update(Order order)
    {
        
    }

    public Order Create()
    {
        var nextId = _orders.Count;
        var order = new Order(nextId + 1, Array.Empty<OrderItem>());
        _orders.Add(order);
        return order;
    }

    public Order GetById(int id)
    {
        return _orders.Single(o => o.Id == id);
    }
}