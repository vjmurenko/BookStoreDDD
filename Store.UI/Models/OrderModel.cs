using System.Collections.Generic;

namespace Store.UI.Models;

public class OrderModel
{
	public int Id { get; set; }
	public decimal TotalPrice { get; set; }
	public int TotalCount { get; set; }
	public List<OrderItemModel> OrderItems { get; set; }
	public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}