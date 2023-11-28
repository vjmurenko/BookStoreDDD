using System.Threading.Tasks;

namespace Store;

public interface IOrderRepository
{
    Task UpdateAsync(Order order);
    Task<Order> CreateAsync();
    Task<Order> GetByIdAsync(int id);
}