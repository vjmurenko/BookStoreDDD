using Microsoft.AspNetCore.Mvc;
using Store.YandexKasa.Areas.YandexKasa.Models;

namespace Store.YandexKasa.Areas.YandexKasa.Controllers;

[Area("YandexKasa")]
public class HomeController : Controller
{
    public IActionResult Index(int orderId, string returnUri)
    {
        var model = new ExampleModel()
        {
            OrderId = orderId,
            ReturnUri = returnUri
        };
        return View(model);
    }

    public IActionResult Callback(int orderId, string returnUri)
    {
        var model = new ExampleModel()
        {
            OrderId = orderId,
            ReturnUri = returnUri
        };
        return View("Callback", model);
    }
}