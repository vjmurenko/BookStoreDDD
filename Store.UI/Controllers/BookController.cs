using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.UI.Controllers;

public class BookController : Controller
{
    private readonly BookService _bookService;

    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }
    // GET
    public IActionResult Index(int id)
    {
        var bookModel = _bookService.GetByid(id);
        return View(bookModel);
    }
}