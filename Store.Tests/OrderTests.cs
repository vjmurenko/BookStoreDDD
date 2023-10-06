namespace Store.Tests;

public class OrderTests
{
    [Fact]
    public void Order_WithNullItems_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new Order(1, null));
    }

    [Fact]
    public void Order_With50CountOrderItem_Return50TotalCount()
    {
        var order = new Order(1, new List<OrderItem>()
        {
            new (1, 20, 20),
            new (2, 10, 30)
        });
        Assert.Equal(20 + 30, order.TotalCount);
    }

    [Fact]
    public void Order_With200TotalPrice_Return200TotalPrice()
    {
        var order = new Order(2, new List<OrderItem>()
        {
            new(1, 10, 15),
            new(2, 10, 5)
        });
        Assert.Equal(10*15 + 10*5, order.TotalPrice);
    }

    [Fact]
    public void Order_WithZeroItems_ReturnZeroFields()
    {
        var order = new Order(1, new OrderItem[0]);
        Assert.Equal(0, order.TotalCount);
        Assert.Equal(0, order.TotalPrice);
    }
}