using System.Collections.Generic;

namespace Store
{
    public interface IBookRepository
    {
        Book[] GetAllBooksByAuthorOrTitle(string query);
        Book[] GetBookByIsbn(string isbn);
        Book[] GetAllBooks();
        Book[] GetBooksByIds(IEnumerable<int> bookIds);
        Book GetBookById(int id);
    }
}