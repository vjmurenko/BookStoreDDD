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

    public string UniqueCode => "Postamate";
    public string Title => "Доставка по Москве и Санкт-Петербургу";

    public Form CreateForm(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order));
        }

        return new Form(UniqueCode, order.Id, 1, false, new[]
        {
            new SelectionField("Город", "city", "1", cities)
        });
    }

    public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
    {
        if (step == 1)
        {
            if (values["city"] == "1")
            {
                return new Form(UniqueCode, orderId, 2, false, new Field[]
                {
                    new HiddenField("Город", "city", "1"),
                    new SelectionField("Постамат", "postomate", "1", postamates["1"])
                });
            }

            if (values["city"] == "2")
            {
                return new Form(UniqueCode, orderId, 2, false, new Field[]
                {
                    new HiddenField("Город", "city", "2"),
                    new SelectionField("Постамат", "postomate", "4", postamates["2"])
                });
            }
            else
            {
                throw new InvalidOperationException("Invalid postamate city");
            }
        }

        if (step == 2)
        {
            return new Form(UniqueCode, orderId, 3, true, new Field[]
            {
                new HiddenField("Город", "city", values["city"]),
                new HiddenField("постамат", "postomate", values["postomate"])
            });
            
        }
        else
        {
            throw new InvalidOperationException("Invalid postomate step");
        }
    }

    public OrderDelivery GetDelivery(Form form)
    {
        if (form.UniqueCode != UniqueCode || !form.IsFinal)
        {
            throw new InvalidOperationException("Invalid Form");
        }

        var cityId = form.Fields.Single(s => s.Name == "city").Value;
        var cityName = cities[cityId];
        var postomateId = form.Fields.Single(s => s.Name == "postomate").Value;
        var postomateName = postamates[cityId][postomateId];


        var parameters = new Dictionary<string, string>()
        {
            { nameof(cityId), cityId },
            { nameof(cityName), cityName },
            { nameof(postomateId), postomateId },
            { nameof(postomateName), postomateName }
        };


        var description = $"Город доставки :{cityName}, Постамат: {postomateName}";
        return new OrderDelivery(description, UniqueCode, 150m, parameters);
    }
}