namespace Store.Web.App;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IReadOnlyCollection<BookModel>> GetBooksByQuery(string query)
    {
        query = query?.Trim();
        Book[] books;
        if (string.IsNullOrEmpty(query))
        {
            books = await _bookRepository.GetAllBooksAsync();
        }
        else if(Book.IsIsbn(query))
        {
            books = await _bookRepository.GetBookByIsbnAsync(query);
        }
        else
        {
            books = await _bookRepository.GetAllBooksByAuthorOrTitleAsync(query);
        }

        return books.Select(Map).ToArray();
    }

    public async Task<BookModel> GetByid(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
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