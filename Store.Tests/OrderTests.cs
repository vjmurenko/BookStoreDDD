using Store.Data;

namespace Store.Tests;

public class OrderTests
{

    [Fact]
    public void TotalPrice_With20and30Price_Return50Price()
    {
        var order = CreateEpmptyOrder();
        order.Items.Add(1, 20, 1);
        order.Items.Add(2, 30, 1);

        Assert.Equal(20 + 30, order.TotalPrice);
    }

    [Fact]
    public void TotalPrice_With10ItempriceAnd20DeliveryPrice_Return30Totalprice()
    {
        var order = new Order(Order.DtoFactory.Create)
        {
            Delivery = new OrderDelivery("postamate", "post", 30, new Dictionary<string, string>())
        };
        order.Items.Add(1, 20, 1);
        Assert.Equal(20 + 30, order.TotalPrice);
    }

    [Fact]
    public void TotalCount_With3ItemsTotal90_Return90TotalCount()
    {
        var order = CreateEpmptyOrder();
        order.Items.Add(1, 20, 30);
        order.Items.Add(2, 30, 30);
        order.Items.Add(3, 40, 30);
        Assert.Equal(30 + 30 + 30, order.TotalCount);
    }

    [Fact]
    public void TotalCount_WithEmptyOrder_ReturnZero()
    {
        var order = CreateEpmptyOrder();
        Assert.Equal(0, order.TotalCount);
    }

    [Fact]
    public void TotalPrice_WithEmptyOrder_ReturnZero()
    {
        var order = CreateEpmptyOrder();
        Assert.Equal(0m, order.TotalPrice);
    }

    private static Order CreateEpmptyOrder()
    {
        return new Order(new OrderDto())
        {

        };
    }
}