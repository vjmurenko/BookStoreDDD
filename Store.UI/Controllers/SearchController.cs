using Microsoft.AspNetCore.Mvc;
using Store.Domain;

namespace Store.UI.Controllers
{
    public class SearchController : Controller
    {
        private IBookRepository _bookRepository { get; set; }

        public SearchController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        public IActionResult Index(string query)
        {
            var books = _bookRepository.GetAllBooksByTitle(query);
            return View(books);
        }

    }
}