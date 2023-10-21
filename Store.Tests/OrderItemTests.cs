namespace Store.Tests;

public class OrderItemTests
{
    [Fact]
    public void OrderItem_WithZeroCount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 20, 0));
    }

    [Fact]
    public void OrderItem_WithNegativeCount_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new OrderItem(1, 20, -20));
    }

    [Fact]
    public void OrderItem_WithPositiveCount_SetCount()
    {
        const int countdownEvent = 30;
        var orderItem = new OrderItem(2, 20, countdownEvent);
        Assert.Equal(countdownEvent, orderItem.Count);
    }


    [Fact]
    public void Count_WithZeroValue_ThrowsArgumentOutOfRangeException()
    {
        var orderItem = new OrderItem(1, 2, 3);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            orderItem.Count = 0;
        });
    }

    [Fact]
    public void Count_WithNegativeValue_ThrowsArgumentOutOfRangeException()
    {
        var orderItem = new OrderItem(1, 2, 3);
        Assert.Throws<ArgumentOutOfRangeException>(() =>
        {
            orderItem.Count = -3;
        });
    }
    
    [Fact]
    public void Count_WithPositiveValue_SetsCount()
    {
        var orderItem = new OrderItem(1, 2, 3);

        orderItem.Count = 50;
        Assert.Equal(50, orderItem.Count);
    }
}