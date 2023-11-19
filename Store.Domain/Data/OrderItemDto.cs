namespace Store.Data
{
    public class OrderItemDto
    {
        public int BookId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public OrderDto orderDto { get; set; }
    }
}
