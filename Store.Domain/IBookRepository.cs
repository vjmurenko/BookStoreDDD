using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store
{
    public interface IBookRepository
    {
        Task<Book[]> GetAllBooksByAuthorOrTitleAsync(string query);
        Task<Book[]> GetBookByIsbnAsync(string isbn);
        Task<Book[]> GetAllBooksAsync();
        Task<Book[]> GetBooksByIdsAsync(IEnumerable<int> bookIds);
        Task<Book> GetBookByIdAsync(int id);
    }
}