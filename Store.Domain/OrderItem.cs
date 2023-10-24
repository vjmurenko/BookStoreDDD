using System;

namespace Store;

public class OrderItem
{

    private int _count;
    public int BookId { get; set; }
    public decimal Price { get; set; }

    public int Count
    {
        get => _count;
        set
        {
            ThrowIfInvalidCount(value);
            _count = value;
        }
    }

    public OrderItem(int bookId, decimal price, int count)
    {
        ThrowIfInvalidCount(count);
        
        BookId = bookId;
        Price = price;
        Count = count;
    }

    private static void ThrowIfInvalidCount(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException("Count must be greater than zero");
        }
    }
}