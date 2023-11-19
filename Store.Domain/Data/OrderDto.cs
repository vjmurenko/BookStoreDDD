using System.Collections.Generic;

namespace Store.Data
{
    public class OrderDto
    {
        public int Id { get; set; }
        public IList<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public decimal TotalPrice { get; set; }
        public int TotalCount { get; set; }
        public string DeliveryServiceName { get; set; }
        public string DeliveryDescription { get; set; }
        public decimal DeliveryPrice { get; set; }
        public Dictionary<string, string> DeliveryParameters { get; set; }
        public string PaymentServiceName { get; set; }
        public string PaymentDescription { get; set; }
        public Dictionary<string, string> PaymentParameters { get; set; }
        public string PhoneNumber { get; set; }
    }
}
