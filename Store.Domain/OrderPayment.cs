using System;
using System.Collections.Generic;

namespace Store;

public class OrderPayment
{
	public string ServiceName { get; }
	public string Description { get; }
	public IReadOnlyDictionary<string, string> Parameters { get; }

	public OrderPayment(string serviceName, string description, IReadOnlyDictionary<string, string> parameters)
	{
		if (string.IsNullOrWhiteSpace(serviceName))
		{
			throw new ArgumentException(nameof(serviceName));
		}

		if (string.IsNullOrWhiteSpace(description))
		{
			throw new ArgumentException(nameof(description));
		}

		if (parameters == null)
		{
			throw new ArgumentNullException(nameof(parameters));
		}

		ServiceName = serviceName;
		Description = description;
		Parameters = parameters;
	}
}