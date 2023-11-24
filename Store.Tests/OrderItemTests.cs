using Store.Data;

namespace Store.Tests;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => OrderItem.Factory.Create(1, 20, 0, new OrderDto()));
    }

    [Fact]
    public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => OrderItem.Factory.Create(1, 20, -20, new OrderDto()));
    }

    [Fact]
    public void OrderItem_WithPositiveCount_SetCount()
    {
        const int count = 30;
        var orderItem = OrderItem.Factory.Create(2, 20, count, new OrderDto());
        Assert.Equal(count, orderItem.Count);
    }

    
    [Fact]
    public void OrderItemCount_WithPositiveValue_SetsCount()
    {
        var orderItemDto = OrderItem.Factory.Create(1, 2, 3, new OrderDto());
        var orderItem = OrderItem.Mapper.Map(orderItemDto);

        orderItem.Count = 50;
        Assert.Equal(50, orderItem.Count);
    }

    [Fact]
    public void OrderItemCount_WithNegativeValue_ThrowsException()
    {
        var orderItemDto = OrderItem.Factory.Create(1, 2, 3, new OrderDto());
        var orderItem = OrderItem.Mapper.Map(orderItemDto);


        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            orderItem.Count = -3;
        });
    }
}