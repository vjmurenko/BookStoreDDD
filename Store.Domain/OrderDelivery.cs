using System;
using System.Collections.Generic;

namespace Store;

public class OrderDelivery
{
    public string UniqueCode { get; }
    public string Description { get; }
    public decimal Amount { get; }

    public IReadOnlyDictionary<string, string> Parameters { get; set; }

    public OrderDelivery(string description, string uniqueCode, decimal amount,
        IReadOnlyDictionary<string, string> parameters)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(nameof(description));
        }

        if (string.IsNullOrWhiteSpace(uniqueCode))
        {
            throw new ArgumentException(nameof(uniqueCode));
        }

        if (parameters == null)
        {
            throw new ArgumentNullException(nameof(parameters));
        }

        UniqueCode = uniqueCode;
        Description = description;
        Amount = amount;
        Parameters = parameters;
    }
}