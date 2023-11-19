using Store.Data;
using System;

namespace Store;

public class OrderItem
{
    private readonly OrderItemDto _dto;
    public int BookId { get => _dto.BookId; }
    public decimal Price { get => _dto.Price; set => _dto.Price = value; }

    public int Count
    {
        get => _dto.Count;
        set
        {
            ThrowIfInvalidCount(value);
            _dto.Count = value;
        }
    }

    public OrderItem(OrderItemDto orderItemDto)
    {
        _dto = orderItemDto;
    }

    public static class Factory
    {
        public static OrderItemDto Create(int bookId, decimal price, int count, OrderDto orderDto)
        {
            if (orderDto == null)
            {
                throw new ArgumentNullException(nameof(Order));
            }
            ThrowIfInvalidCount(count);

            return new OrderItemDto
            {
                orderDto = orderDto,
                BookId = bookId,
                Count = count,
                Price = price
            };
        }
    }

    private static void ThrowIfInvalidCount(int count)
    {
        if (count <= 0)
        {
            throw new ArgumentOutOfRangeException("Count must be greater than zero");
        }
    }

    public static class Mapper
    {
        public static OrderItemDto Map(OrderItem orderItem) => orderItem._dto;
        public static OrderItem Map(OrderItemDto orderItemDto) => new(orderItemDto);
    }
}