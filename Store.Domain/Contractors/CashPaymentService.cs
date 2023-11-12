using System;
using System.Collections.Generic;
using Store.Mesages;

namespace Store.Contractors;

public class CashPaymentService : IPaymentService
{
    public string Name => "Cash";
    public string Title => "Оплата наличными";
    public Form CreateForm(Order order)
    {
        return Form.CreateFirst(Name).AddParameter("orderId", order.Id.ToString());
    }

    public Form MoveNextForm(int step, IReadOnlyDictionary<string, string> values)
    {
        if (step != 1)
        {
            throw new InvalidOperationException("Invalid payment form");
        }

        return Form.CreateLast(Name, step + 1, values);
    }

    public OrderPayment GetPayment(Form form)
    {
        if (Name != form.ServiceName || !form.IsFinal)
        {
            throw new InvalidOperationException("Неверная форма");
        }

        return new OrderPayment(Name, "Оплата наличными", new Dictionary<string, string>());
    }
}