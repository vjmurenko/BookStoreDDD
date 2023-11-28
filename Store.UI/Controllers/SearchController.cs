using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Store.Web.App;

namespace Store.UI.Controllers
{
    public class SearchController : Controller
    {
        private readonly BookService _bookService;

        public SearchController(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<IActionResult> Index(string query)
        {
            var books = await _bookService.GetBooksByQuery(query);
            return View(books);
        }
    }
}