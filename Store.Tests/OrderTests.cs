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

    [Fact]
    public void GetItem_NotExistBook_ThrowsException()
    {
        var order = new Order(1, new []{new OrderItem(1,10,30)});


        Assert.Throws<ArgumentException>(() =>
        {
            order.GetItem(10);
        });
    }
    
    [Fact]
    public void GetItem_ExistBook_ReturnBook()
    {
        var order = new Order(1, new []{new OrderItem(1,10,30)});


        var book = order.GetItem(1);
        Assert.Equal(1, book.BookId);
    }

    [Fact]
    public void RemoveItem_NotExistBook_ThrowsException()
    {
        var order = new Order(2, new[] { new OrderItem(2, 30, 40) });
        Assert.Throws<ArgumentException>(() =>
        {
            order.RemoveItem(30);
        });
    }
    
    [Fact]
    public void RemoveItem_WithExistingItem_RemovesItem()
    {
        var order = new Order(2, new[]
        {
            new OrderItem(2, 30, 40),
            new OrderItem(3,40,50),
            new OrderItem(5,60,30)
        });

        order.RemoveItem(5);
        Assert.Equal(2, order.Items.Count);
    }

    [Fact]
    public void AddOrUpdateItem_WithNullBook_ThrowsArgumentNullException()
    {
        var order = new Order(1, new[] { new OrderItem(3, 4, 5) });
        Assert.Throws<ArgumentNullException>(() =>
        {
            order.AddOrUpdateItem(null, 20);
        });
    }
    
    [Fact]
    public void AddOrUpdateItem_WithExistItem_UpdateCount()
    {
        var order = new Order(1, new[]
        {
            new OrderItem(1, 4, 1),
            new OrderItem(2, 4, 1),
            new OrderItem(5,6,1)
        });
    
        order.AddOrUpdateItem(new Book(5,"","","","",20), 10);
        Assert.Equal(11, order.GetItem(5).Count);
    }
    
    [Fact]
    public void AddOrUpdateItem_WithNotExistItem_AddNewItem()
    {
        var order = new Order(1, new[]
        {
            new OrderItem(1, 4, 1),
            new OrderItem(2, 4, 2),
            new OrderItem(3, 4, 3),
        });
    
        order.AddOrUpdateItem(new Book(5,"","","","",20), 1);
        Assert.Equal(4, order.Items.Count);
    }
}