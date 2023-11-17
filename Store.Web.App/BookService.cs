namespace Store.Web.App;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public IReadOnlyCollection<BookModel> GetBooksByQuery(string query)
    {
        query = query?.Trim();
        Book[] books;
        if (string.IsNullOrEmpty(query))
        {
            books = _bookRepository.GetAllBooks();
        }
        else if(Book.IsIsbn(query))
        {
            books = _bookRepository.GetBookByIsbn(query);
        }
        else
        {
            books = _bookRepository.GetAllBooksByAuthorOrTitle(query);
        }

        return books.Select(Map).ToArray();
    }

    public BookModel GetByid(int id)
    {
        var book = _bookRepository.GetBookById(id);
        return Map(book);
    }

    private BookModel Map(Book book)
    {
        return new BookModel()
        {
            Id = book.Id,
            Author = book.Author,
            Description = book.Description,
            Isbn = book.Isbn,
            Price = book.Price,
            Title = book.Title
        };
    }
}