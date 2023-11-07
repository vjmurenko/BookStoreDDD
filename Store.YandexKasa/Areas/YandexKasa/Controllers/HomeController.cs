using Microsoft.AspNetCore.Mvc;

namespace Store.YandexKasa.Areas.Yandex.Kasa.Controllers;

[Area("YandexKasa")]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Callback()
    {
        return View("Callback");
    }
}