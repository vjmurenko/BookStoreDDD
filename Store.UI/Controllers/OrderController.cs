using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Mesages;
using Store.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Store.Contractors;
using Store.Web.Contractors;

namespace Store.Web.Controllers;

public class OrderController : Controller
{
	private readonly IBookRepository _bookRepository;
	private readonly IOrderRepository _orderRepository;
	private readonly INotificationService _notificationService;
	private readonly IEnumerable<IDeliveryService> _deliveryServices;
	private readonly IEnumerable<IPaymentService> _paymentServices;
	private readonly IEnumerable<IWebContractor> _webContractors;

	public OrderController(
		IBookRepository bookRepository,
		IOrderRepository orderRepository,
		INotificationService notificationService,
		IEnumerable<IDeliveryService> deliveryServices,
		IEnumerable<IPaymentService> paymentServices,
		IEnumerable<IWebContractor> webContractors)
	{
		_bookRepository = bookRepository;
		_orderRepository = orderRepository;
		_notificationService = notificationService;
		_deliveryServices = deliveryServices;
		_paymentServices = paymentServices;
		_webContractors = webContractors;
	}

	public IActionResult Index()
	{
		if (HttpContext.Session.TryGetCart(out var cart))
		{
			var order = _orderRepository.GetById(cart.OrderId);
			var orderModel = MapOrder(order);
			return View(orderModel);
		}

		return View("Empty");
	}

	public IActionResult Add(int bookId)
	{
		Order order;

		if (HttpContext.Session.TryGetCart(out var cart))
		{
			order = _orderRepository.GetById(cart.OrderId);
		}
		else
		{
			order = _orderRepository.Create();
			cart = new Cart(order.Id);
		}

		var book = _bookRepository.GetBookById(bookId);
		order.AddOrUpdateItem(book, 1);
		_orderRepository.Update(order);

		cart.TotalPrice = order.TotalPrice;
		cart.TotalCount = order.TotalCount;

		HttpContext.Session.Set(cart);
		return RedirectToAction("Index", "Book", new { id = bookId });
	}

	[HttpPost]
	public IActionResult RemoveItem(int bookId)
	{
		if (HttpContext.Session.TryGetCart(out var cart))
		{
			var order = _orderRepository.GetById(cart.OrderId);
			order.RemoveItem(bookId);
			cart.TotalCount = order.TotalCount;
			cart.TotalPrice = order.TotalPrice;
			HttpContext.Session.Set(cart);
		}

		return RedirectToAction("Index");
	}

	[HttpPost]
	public IActionResult DeleteAll()
	{
		if (HttpContext.Session.TryGetCart(out var cart))
		{
			var order = _orderRepository.GetById(cart.OrderId);
			order.DeleteAll();
			HttpContext.Session.RemoveCart();
		}

		return RedirectToAction("Index");
	}

	[HttpPost]
	public IActionResult SendConfirmationCode(int orderId, string phoneNumber)
	{
		var phoneIsValid = IsValidPhone(phoneNumber);
		if (!phoneIsValid)
		{
			var order = _orderRepository.GetById(orderId);
			var model = MapOrder(order);
			model.Errors["phoneNumber"] = "Номер телефона не соответствует формату +71234567891";
			return View("Index", model);
		}
		// var code = new Random().Next(1000, 10000);
		var code = 1111;
		_notificationService.SendNotificationCode(code, phoneNumber);
		HttpContext.Session.SetInt32(phoneNumber, code);

		return View("Confirmation", new ConfirmationModel()
		{
			OrderId = orderId,
			PhoneNumber = phoneNumber
		});
	}

	[HttpPost]
	public IActionResult Confirm(int orderId, string phoneNumber, int code)
	{
		var confirmationModel = new ConfirmationModel()
		{
			OrderId = orderId,
			PhoneNumber = phoneNumber
		};
		var codeFromSession = HttpContext.Session.GetInt32(phoneNumber);

		if (codeFromSession == null)
		{
			confirmationModel.Errors["code"] = "Пустой код, повторите отправку";
		}

		if (codeFromSession != code)
		{
			confirmationModel.Errors["code"] = "Введенный код не соответствует отправленному";
		}
		
		HttpContext.Session.Remove(phoneNumber);
		var deliveryModel = new DeliveryModel()
		{
			OrderId = orderId,
			Methods = _deliveryServices.ToDictionary(
				service => service.UniqueCode,
				service => service.Title)

		};

		return View("DeliveryMethod", deliveryModel);
	}
	
	[HttpPost]
	public IActionResult StartDelivery(int orderId, string uniqueCode)
	{
		var deliveryService = _deliveryServices.Single(s => s.UniqueCode == uniqueCode);
		var order = _orderRepository.GetById(orderId);
		var form = deliveryService.CreateForm(order);
		
		return View("DeliveryStep", form);
	}

	[HttpPost]
	public IActionResult NextDelivery(int orderId, string uniqueCode, int step, Dictionary<string, string> values)
	{
		var deliveryService = _deliveryServices.Single(s => s.UniqueCode == uniqueCode);
		var form = deliveryService.MoveNextForm(orderId, step, values);

		if (form.IsFinal)
		{
			var order = _orderRepository.GetById(orderId);
			var orderDelivery = deliveryService.GetDelivery(form);
			order.OrderDelivery = orderDelivery;
			_orderRepository.Update(order);
			
			var model = new PaymentModel()
			{
				OrderId = orderId,
				Methods = _paymentServices.ToDictionary(p => p.UniqueCode, p => p.Title)
			};
			return View("PaymentMethod", model);

		}
		return View("DeliveryStep", form);
	}

	[HttpPost]
	public IActionResult StartPayment(int orderId, string uniqueCode)
	{
		var paymentService = _paymentServices.Single(p => p.UniqueCode == uniqueCode);
		var order = _orderRepository.GetById(orderId);
		var form = paymentService.CreateForm(order);

		var webContractors = _webContractors.SingleOrDefault(w => w.UniqueCode == uniqueCode);
		if (webContractors != null)
		{
			return Redirect(webContractors.GetUri);
		}
		
		return View("PaymentStep", form);
	}

	[HttpPost]
	public IActionResult NextPayment(int orderId, string uniqueCode, int step, Dictionary<string, string> values)
	{
		var paymentService = _paymentServices.Single(p => p.UniqueCode == uniqueCode);
		var form = paymentService.MoveNextForm(orderId, step, values);
		if (form.IsFinal)
		{
			var order = _orderRepository.GetById(orderId);
			var orderPayment = paymentService.GetPayment(form);
			order.OrderPayment = orderPayment;
			_orderRepository.Update(order);
			return View("Finish");
		}

		return View("PaymentStep", form);
	}

	public IActionResult Finish()
	{
		HttpContext.Session.RemoveCart();
		return RedirectToAction("Index", "Search");
	}

	private OrderModel MapOrder(Order order)
	{
		var books = _bookRepository.GetBooksByIds(order.Items.Select(s => s.BookId));


		var orderItemModels = (from book in books
							   join orderItem in order.Items on book.Id equals orderItem.BookId
							   select new OrderItemModel()
							   {
								   Id = book.Id,
								   Title = book.Title,
								   Price = book.Price,
								   Count = orderItem.Count,
								   Author = book.Author,
								   Description = book.Description,
								   Isbn = book.Isbn
							   }).ToList();

		var orderModel = new OrderModel()
		{
			Id = order.Id,
			TotalCount = order.TotalCount,
			TotalPrice = order.TotalPrice,
			OrderItems = orderItemModels
		};
		return orderModel;
	}

	private static bool IsValidPhone(string phone)
	{
		if (string.IsNullOrEmpty(phone))
		{
			return false;
		}

		phone = phone.Replace(" ", "").Replace("-", "");

		return Regex.IsMatch(phone, @"\+?\d{11}$");
	}
}