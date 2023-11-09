﻿using System;
using System.Collections.Generic;
using Store.Mesages;

namespace Store.Contractors;

public class CashPaymentService : IPaymentService
{
    public string UniqueCode => "Cash";
    public string Title => "Оплата наличными";
    public Form CreateForm(Order order)
    {
        return new Form(UniqueCode, order.Id, 1, false, new Field[0]);
    }

    public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
    {
        if (step != 1)
        {
            throw new InvalidOperationException("Invalid payment form");
        }
        return new Form(UniqueCode, orderId, 2, true, new Field[0]);
    }

    public OrderPayment GetPayment(Form form)
    {
        if (UniqueCode != form.UniqueCode || !form.IsFinal)
        {
            throw new InvalidOperationException("Неверная форма");
        }

        return new OrderPayment(UniqueCode, "Оплата наличными", new Dictionary<string, string>());
    }
}