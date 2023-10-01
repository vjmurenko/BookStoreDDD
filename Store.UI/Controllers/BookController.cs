using Microsoft.AspNetCore.Mvc;

namespace Store.Web.Controllers;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    // GET
    public IActionResult Index(int id)
    {
        var book = _bookRepository.GetBookById(id);
        return View(book);
    }
}