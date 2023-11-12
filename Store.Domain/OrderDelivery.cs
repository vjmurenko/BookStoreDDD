using System;
using System.Collections.Generic;

namespace Store;

public class OrderDelivery
{
    public string ServiceName { get; }
    public string Description { get; }
    public decimal Amount { get; }

    public IReadOnlyDictionary<string, string> Parameters { get; set; }

    public OrderDelivery(string description, string serviceName, decimal amount,
        IReadOnlyDictionary<string, string> parameters)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(nameof(description));
        }

        if (string.IsNullOrWhiteSpace(serviceName))
        {
            throw new ArgumentException(nameof(serviceName));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        ServiceName = serviceName;
        Description = description;
        Amount = amount;
        Parameters = parameters;
    }
}