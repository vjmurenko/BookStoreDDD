using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Store.UI.Models;

namespace Store.Web.Controllers;

public class OrderController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IOrderRepository _orderRepository;

    public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository)
    {
        _bookRepository = bookRepository;
        _orderRepository = orderRepository;
    }

    // GET
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

    public IActionResult Add(int id)
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

        var book = _bookRepository.GetBookById(id);
        order.AddItem(book, 1);
        _orderRepository.Update(order);

        cart.TotalPrice = order.TotalPrice;
        cart.TotalCount = order.TotalCount;

        HttpContext.Session.Set(cart);
        return RedirectToAction("Index", "Book", new { id });
    }

    private OrderModel MapOrder(Order order)
    {
        var books = _bookRepository.GetBooksByIds(order.Items.Select(s => s.Id));


        var orderItemModels = (from book in books
            join orderItem in order.Items on book.Id equals orderItem.Id
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

        var orderModel = new OrderModel() { Id = order.Id, TotalCount = order.TotalCount, TotalPrice = order.TotalPrice, OrderItems = orderItemModels };
        return orderModel;
    }
}