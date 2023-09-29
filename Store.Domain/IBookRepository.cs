namespace Store
{
    public interface IBookRepository
    {
        Book[] GetAllBooksByAuthorOrTitle(string query);
        Book[] GetBookByIsbn(string isbn);
        Book[] GetAllBooks();
    }
}