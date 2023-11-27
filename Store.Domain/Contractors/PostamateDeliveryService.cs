using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Contractors;

public class PostamateDeliveryService : IDeliveryService
{
    private static IReadOnlyDictionary<string, string> cities = new Dictionary<string, string>()
    {
        { "1", "Москва" },
        { "2", "Санкт-Петербург" }
    };

    private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> postamates =
        new Dictionary<string, IReadOnlyDictionary<string, string>>()
        {
            {
                "1", new Dictionary<string, string>()
                {
                    { "1", "Казанский вокзал" },
                    { "2", "Охотный ряд" },
                    { "3", "Савелевский рынок" }
                }
            },
            {
                "2", new Dictionary<string, string>()
                {
                    { "4", "Московский вокзал" },
                    { "5", "Гостиный двор" },
                    { "6", "Петропавловская крепость" }
                }
            }
        };

    public string Name => "Postamate";
    public string Title => "Доставка по Москве и Санкт-Петербургу";

    public Form FirstForm(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        return Form.CreateFirst(Name)
            .AddParameter("orderId", order.Id.ToString())
            .AddField(new SelectionField("Город", "city", "1", cities));
    }

    public Form NextForm(int step, IReadOnlyDictionary<string, string> values)
    {
        if (step == 1)
        {
            if (values["city"] == "1")
            {
                return Form.CreateNext(Name, 2, values)
                    .AddField(new SelectionField("Постамат", "postomate", "1", postamates["1"]));
            }

            if (values["city"] == "2")
            {

                return Form.CreateNext(Name, 2, values)
                    .AddField(new SelectionField("Постамат", "postomate", "4", postamates["2"]));
            }
            else
            {
                throw new InvalidOperationException("Invalid postamate city");
            }
        }

        if (step == 2)
        {

            return Form.CreateLast(Name, 3, values);
        }

        throw new InvalidOperationException("Invalid postomate step");
    }

    public OrderDelivery GetDelivery(Form form)
    {
        if (form.ServiceName != Name || !form.IsFinal)
        {
            throw new InvalidOperationException("Invalid Form");
        }

        var cityId = form.Parameters.Single(s => s.Key == "city").Value;
        var cityName = cities[cityId];
        var postomateId = form.Parameters.Single(s => s.Key == "postomate").Value;
        var postomateName = postamates[cityId][postomateId];
        var priceParameter = form.Parameters.Single(p => p.Key == "price").Value;
        var priceValue = decimal.Parse(priceParameter);
         
        
        var parameters = new Dictionary<string, string>()
        {
            { nameof(cityId), cityId },
            { nameof(cityName), cityName },
            { nameof(postomateId), postomateId },
            { nameof(postomateName), postomateName }
        };
        
        var description = $"Город доставки :{cityName}, Постамат: {postomateName}";
        return new OrderDelivery(description, Name, priceValue, parameters);
    }
}