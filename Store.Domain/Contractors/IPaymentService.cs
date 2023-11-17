using System.Collections.Generic;
using Store.Mesages;

namespace Store.Contractors;

public interface IPaymentService
{
    string Name { get;  }
    string Title { get; }
    Form CreateForm(Order order);
    Form MoveNextForm(int step, IReadOnlyDictionary<string, string> values);
    OrderPayment GetPayment(Form form);
}