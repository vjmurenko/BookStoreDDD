using System.Collections.Generic;

namespace Store.UI.Models;

public class ConfirmationModel
{
    public int OrderId { get; set; }
    public string PhoneNumber { get; set; }
    public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
}