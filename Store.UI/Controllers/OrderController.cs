using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Store.Contractors;
using Store.Web.App;
using Store.Web.Contractors;

namespace Store.UI.Controllers;

public class OrderController : Controller
{
    private readonly IEnumerable<IDeliveryService> _deliveryServices;
    private readonly IEnumerable<IPaymentService> _paymentServices;
    private readonly IEnumerable<IWebContractorService> _webContractors;
    private readonly OrderService _orderService;

    public OrderController(
        IEnumerable<IDeliveryService> deliveryServices,
        IEnumerable<IPaymentService> paymentServices,
        IEnumerable<IWebContractorService> webContractors,
        OrderService orderService)
    {
        _deliveryServices = deliveryServices;
        _paymentServices = paymentServices;
        _webContractors = webContractors;
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (_orderService.TryGetModel(out OrderModel orderModel))
        {
            return View(orderModel);
        }

        return View("Empty");
    }

    [HttpPost]
    public IActionResult AddItem(int bookId, int count = 1)
    {
        _orderService.AddBook(bookId, count);
        return RedirectToAction("Index", "Book", new { id = bookId });
    }

    [HttpPost]
    public IActionResult RemoveItem(int bookId)
    {
        _orderService.DeleteBook(bookId);

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult DeleteAll()
    {
        _orderService.DeleteAllBooks();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SendConfirmationCode(string phoneNumber)
    {
        var orderModel = _orderService.SendConfirmationCode(phoneNumber);
        if (orderModel.Errors.Count > 0)
        {
            return View("Index", orderModel);
        }

        return View("Confirmation", orderModel);
    }

    [HttpPost]
    public IActionResult Confirm(string phoneNumber, int code)
    {
        var orderModel = _orderService.ConfirmPhone(phoneNumber, code);

        if (orderModel.Errors.Count > 0)
        {
            return View("Confirmation", orderModel);
        }

        var methods = _deliveryServices.ToDictionary(service => service.Name, service => service.Title);

        return View("DeliveryMethod", methods);
    }

    [HttpPost]
    public IActionResult StartDelivery(string serviceName)
    {
        var deliveryService = _deliveryServices.Single(s => s.Name == serviceName);
        var order = _orderService.GetOrder();
        var form = deliveryService.FirstForm(order);
        var webContractors = _webContractors.SingleOrDefault(w => w.Name == serviceName);

        if (webContractors == null)
        {
            return View("DeliveryStep", form);
        }

        var returnUrl = GetReturnUri(nameof(NextDelivery));
        var redirectUri = webContractors.StartSession(form.Parameters, returnUrl);
        return Redirect(redirectUri.ToString());
    }

    [HttpPost]
    public IActionResult NextDelivery(string serviceName, int step, Dictionary<string, string> values)
    {
        var deliveryService = _deliveryServices.Single(s => s.Name == serviceName);
        var form = deliveryService.NextForm(step, values);

        if (!form.IsFinal) return View("DeliveryStep", form);

        var orderDelivery = deliveryService.GetDelivery(form);
        _orderService.SetDelivery(orderDelivery);
        var methods = _paymentServices.ToDictionary(p => p.Name, p => p.Title);

        return View("PaymentMethod", methods);
    }

    [HttpPost]
    public IActionResult StartPayment(string serviceName)
    {
        var paymentService = _paymentServices.Single(p => p.Name == serviceName);
        var order = _orderService.GetOrder();
        var form = paymentService.CreateForm(order);

        var webContractor = _webContractors.SingleOrDefault(w => w.Name == serviceName);
        if (webContractor == null)
        {
            return View("PaymentStep", form);
        }

        var returnUri = GetReturnUri(nameof(NextPayment));
        var redirectUri = webContractor.StartSession(form.Parameters, returnUri);

        return Redirect(redirectUri.ToString());
    }

    [HttpPost]
    public IActionResult NextPayment(string serviceName, int step, Dictionary<string, string> values)
    {
        var paymentService = _paymentServices.Single(p => p.Name == serviceName);
        var form = paymentService.MoveNextForm(step, values);
        if (!form.IsFinal)
        {
            return View("PaymentStep", form);
        }

        var payment = paymentService.GetPayment(form);
        _orderService.SetPayment(payment);

        return View("Finish");
    }

    private Uri GetReturnUri(string action)
    {
        var builder = new UriBuilder(Request.Scheme, Request.Host.Host)
        {
            Path = Url.Action(action),
            Query = null
        };
        if (Request.Host.Port != null)
        {
            builder.Port = Request.Host.Port.Value;
        }

        return builder.Uri;
    }
}