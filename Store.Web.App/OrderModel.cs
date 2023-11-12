namespace Store.Web.App;

public class OrderModel
{
	public int Id { get; set; }
	public decimal TotalPrice { get; set; }
	public int TotalCount { get; set; }
	public string PhoneNumber { get; set; }
	public string DeliveryDescription { get; set; }
	public string PaymentDescription { get; set; }
	public OrderItemModel[] OrderItems { get; set; } = Array.Empty<OrderItemModel>();
	public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}