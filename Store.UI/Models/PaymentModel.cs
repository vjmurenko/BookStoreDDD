using System.Collections.Generic;

public class PaymentModel
{
    public int  OrderId { get; set; }
    public Dictionary<string, string> Methods { get; set; }
}