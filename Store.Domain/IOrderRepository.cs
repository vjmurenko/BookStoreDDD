namespace Store;

public interface IOrderRepository
{
    void Update(Order order);
    Order Create();
    Order GetById(int id);
}