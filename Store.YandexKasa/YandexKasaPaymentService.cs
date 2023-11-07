using Store.Contractors;
using Store.Mesages;
using Store.Web.Contractors;

namespace Store.YandexKasa;

public class YandexKasaPaymentService : IPaymentService, IWebContractor
{
    public string UniqueCode => "Card";
    public string Title => "Яндекс касса";
    public string GetUri => "/YandexKasa/";
    public Form CreateForm(Order order)
    {
        return new Form(UniqueCode, order.Id, 1, false, new Field[0]);
    }
    
    public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
    {
        return new Form(UniqueCode, orderId, 2, true, new Field[0]);
    }

    public OrderPayment GetPayment(Form form)
    {
        if (UniqueCode != form.UniqueCode || !form.IsFinal)
        {
            throw new InvalidOperationException("Неверная форма");
        }

        return new OrderPayment(UniqueCode, "Оплата картой", new Dictionary<string, string>());
    }
}