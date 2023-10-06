using System;

namespace Store;

public class OrderItem
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }

    public OrderItem(int id, decimal price, int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException("count must be above zero");
        }
        Id = id;
        Price = price;
        Count = count;
    }

}