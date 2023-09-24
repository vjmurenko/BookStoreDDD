using System.Linq;
using Store.Domain;

namespace Store.Data
{
    public class BookRepository : IBookRepository
    {
        private Book[] Books = new Book[]
        {
            new Book(1, "True"),
            new Book(2, "Forewer"),
            new Book(3, "False")
        };

        public Book GetBookByTitle(string title)
        {
            return Books.FirstOrDefault(b => b.Title == title);
        }

        public Book[] GetAllBooksByTitle(string title)
        {
            return Books.Where(b => string.IsNullOrEmpty(title) || b.Title.ToLowerInvariant().Contains(title.ToLowerInvariant())).ToArray();
        }
    }
}