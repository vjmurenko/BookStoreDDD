namespace Store.Domain
{
    public interface IBookRepository
    {
        Book GetBookByTitle(string title);
        Book[] GetAllBooksByTitle(string title);
    }
}