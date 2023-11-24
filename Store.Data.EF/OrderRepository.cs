using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextFactory _factory;
        private StoreDbContext DbContext => _factory.Create(typeof(OrderRepository));

        public OrderRepository(DbContextFactory factory)
        {
            _factory = factory;
        }

        public Order Create()
        {

            var orderDto = Order.DtoFactory.Create;
            DbContext.Orders.Add(orderDto); ;
            DbContext.SaveChanges();

            return Order.Mapper.Map(orderDto);

        }

        public Order GetById(int id)
        {
            var orderDto = DbContext.Orders
                .Include(o => o.Items)
                .Single(o => o.Id == id);

            return Order.Mapper.Map(orderDto);

        }

        public void Update(Order order)
        {

            var orderFormDb = DbContext.Orders.FirstOrDefault(s => s.Id == order.Id);

            DbContext.Orders.Update(Order.Mapper.Map(order));
            DbContext.SaveChanges();
        }
    }
}