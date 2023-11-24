using Store.Data;

namespace Store.Tests;

public class OrderItemCollectionTests
{
    [Fact]
    public void AddItem_WithNotExistItem_AddNewItem()
    {
        var order = new Order(Order.DtoFactory.Create);
        order.Items.Add(1,4,1);
        order.Items.Add(2, 4, 1);
        order.Items.Add(3, 4, 1);

        Assert.Equal(3, order.Items.Count);
    }

    [Fact]
    public void AddItem_WithExistItem_ThrowException()
    {
        var order = new Order(Order.DtoFactory.Create);
        order.Items.Add(1, 4, 1);
        order.Items.Add(2, 4, 1);

        Assert.Throws<ArgumentException>(() => order.Items.Add(2, 4,1));
    }

    [Fact]
    public void RemoveItem_WithExistingItem_RemovesItem()
    {
        var order = CreateOrder();
        order.Items.RemoveItem(1);
        Assert.Single(order.Items);
    }

    [Fact]
    public void RemoveItem_WithNotExistingItem_ThrowsException()
    {
        var order = CreateOrder();
        Assert.Throws<ArgumentException>(() => order.Items.RemoveItem(10));
    }

    [Fact]
    public void GetItem_NotExistBook_ThrowsException()
    {
        var order = CreateOrder();

        Assert.Throws<ArgumentException>(() =>
        {
            order.Items.Get(10);
        });
    }

    [Fact]
    public void GetItem_ExistBook_ReturnBook()
    {
        var order = CreateOrder();

        var book = order.Items.Get(1);
        Assert.Equal(1, book.BookId);
    }

    [Fact]
    public void Order_WithZeroItems_ReturnZeroFields()
    {
        var order = new Order(Order.DtoFactory.Create);
        Assert.Equal(0, order.TotalCount);
        Assert.Equal(0, order.TotalPrice);
    }

    [Fact]
    public void Order_With200TotalPrice_Return200TotalPrice()
    {
        var order = new Order(Order.DtoFactory.Create);
        order.Items.Add(1, 10, 15);
        order.Items.Add(2, 10, 5);

        Assert.Equal(10 * 15 + 10 * 5, order.TotalPrice);
    }

    [Fact]
    public void Order_With50CountOrderItem_Return50TotalCount()
    {
        var order = new Order(Order.DtoFactory.Create);
        order.Items.Add(1, 20, 20);
        order.Items.Add(2, 10, 30);

        Assert.Equal(20 + 30, order.TotalCount);
    }

    [Fact]
    public void RemoveAll_Set0CountItems()
    {
        var order = CreateOrder();

        Assert.NotEmpty(order.Items);

        order.Items.RemoveAll();

        Assert.Empty(order.Items);
    }

    private static Order CreateOrder()
    {
        var order = new Order(Order.DtoFactory.Create);
        order.Items.Add(1, 1, 1);
        order.Items.Add(2, 1, 1);
        return order;
    }
}