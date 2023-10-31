using System;
using Microsoft.AspNetCore.Mvc;
using Store.UI.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Store.Mesages;

namespace Store.Web.Controllers;

public class OrderController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly INotificationService _notificationService;

    public OrderController(
        IBookRepository bookRepository,
        IOrderRepository orderRepository,
        INotificationService notificationService)
    {
        _bookRepository = bookRepository;
        _orderRepository = orderRepository;
        _notificationService = notificationService;
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

    public IActionResult DeleteAll()
    {
        if (HttpContext.Session.TryGetCart(out var cart))
        {
            var order = _orderRepository.GetById(cart.OrderId);
            order.DeleteAll();
            HttpContext.Session.CleanCart();
        }

        return RedirectToAction("Index");
    }

    public IActionResult SendCode(int orderId, string phoneNumber)
    {
        var code = new Random().Next(1000, 9999);
        
        _notificationService.SendNotificationCode(code, phoneNumber);
        HttpContext.Session.SetInt32(phoneNumber, code);

        return View("Confiramtion", new ConfirmationModel()
        {
            OrderId = orderId,
            PhoneNumber = phoneNumber
        });
    }

    public IActionResult StartDelivery(int orderId, string phoneNumber, int code)
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

        return View("Confiramtion", confirmationModel);
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
            Id = order.Id, TotalCount = order.TotalCount, TotalPrice = order.TotalPrice, OrderItems = orderItemModels
        };
        return orderModel;
    }
}