using Microsoft.AspNetCore.Mvc;
using Store.UI.Models;

namespace Store.Web.Controllers;

public class CartController : Controller
{
    private readonly IBookRepository _bookRepository;

    public CartController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Add(int id)
    {
        var book = _bookRepository.GetBookById(id);
        Cart cart;
        if (!HttpContext.Session.TryGetCart(out  cart))
        {
            cart = new Cart();
        }

        if (cart.Items.ContainsKey(id))
        {
            cart.Items[id] += 1;
        }
        else
        {
            cart.Items[id] = 1;
        }

        cart.Amount += book.Price;

        HttpContext.Session.Set(cart);
        return RedirectToAction("Index", "Book", new { id });

    }
}