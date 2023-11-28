using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextFactory _factory;
    private StoreDbContext DbContext => _factory.Create(typeof(OrderRepository));

    public OrderRepository(DbContextFactory factory)
    {
        _factory = factory;
    }

    public async Task<Order> CreateAsync()
    {
        var orderDto = Order.DtoFactory.Create;
        DbContext.Orders.Add(orderDto); ;
        await DbContext.SaveChangesAsync();

        return Order.Mapper.Map(orderDto);
    }

    public  async Task<Order> GetByIdAsync(int id)
    {
        var orderDto = await DbContext.Orders
            .Include(o => o.Items)
            .SingleAsync(o => o.Id == id);

        return Order.Mapper.Map(orderDto);
    }

    public async Task UpdateAsync(Order order)
    {
        DbContext.Orders.Update(Order.Mapper.Map(order));
        await DbContext.SaveChangesAsync();
    }
}