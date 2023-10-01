using System.Linq;

namespace Store.Data
{
    public class BookRepository : IBookRepository
    {
        private Book[] Books = new Book[]
        {
            new Book(1, "True", "Kevin", "ISBN 12345-12345"),
            new Book(2, "Forewer", "Justin", "ISBN 12345-12346"),
            new Book(3, "False", "Jack", "ISBN 12345-67891")
        };

        public Book[] GetAllBooksByAuthorOrTitle(string query)
        {
            return Books.Where(s => s.Author.Contains(query) || s.Title.Contains(query)).ToArray();
        }

        public Book[] GetBookByIsbn(string isbn)
        {
            return Books.Where(b => b.Isbn == isbn).ToArray();
        }

        public Book[] GetAllBooks()
        {
            return Books;
        }

        public Book GetBookById(int id)
        {
            return Books.Single(b => b.Id == id);
        }
    }
}