using System.Threading.Tasks;
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
    public async Task<IActionResult> Index(int id)
    {
        var bookModel = await _bookService.GetByid(id);
        return View(bookModel);
    }
}