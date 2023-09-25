namespace Store;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Book[] GetBooksByQuery(string query)
    {
        if (string.IsNullOrEmpty(query))
        {
            return _bookRepository.GetAllBooks();
        }
        if (Book.IsIsbn(query))
        {
            return _bookRepository.GetBookByIsbn(query);
        }

        return _bookRepository.GetAllBooksByAuthorOrTitle(query);

    }
}