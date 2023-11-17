using Store.Contractors;
using Store.Mesages;
using Store.Web.Contractors;

namespace Store.YandexKasa;

public class YandexKasaPaymentService : IPaymentService, IWebContractorService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public string Name => "YandexKasa";

    public string Title => "Яндекс касса";

    public HttpRequest Request => _httpContextAccessor.HttpContext.Request;

    public YandexKasaPaymentService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Uri StartSession(IReadOnlyDictionary<string, string> parameters, Uri returnUri)
    {
        var queryString = QueryString.Create(parameters);
        queryString += QueryString.Create("returnUri", returnUri.ToString());
        var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
        {
            Path = "YandexKasa/",
            Query = queryString.ToString()
        };
        if (Request.Host.Port != null)
        {
            builder.Port = Request.Host.Port.Value;
        }

        return builder.Uri;
    }

    public Form CreateForm(Order order)
    {
        return Form.CreateFirst(Name)
            .AddParameter("orderId", order.Id.ToString());
    }

    public Form MoveNextForm(int step, IReadOnlyDictionary<string, string> values)
    {
        return Form.CreateLast(Name, step + 1, values);
    }

    public OrderPayment GetPayment(Form form)
    {
        if (Name != form.ServiceName || !form.IsFinal)
        {
            throw new InvalidOperationException("Неверная форма");
        }

        return new OrderPayment(Name, "Оплата картой", new Dictionary<string, string>());
    }
}