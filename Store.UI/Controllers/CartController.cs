using Microsoft.AspNetCore.Mvc;
using Store.UI.Models;

namespace Store.Web.Controllers;

public class CartController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IOrderRepository _orderRepository;

    public CartController(IBookRepository bookRepository, IOrderRepository orderRepository)
    {
        _bookRepository = bookRepository;
        _orderRepository = orderRepository;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add(int id)
    {
        Order order;
        
        if(HttpContext.Session.TryGetCart(out var cart))
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
}