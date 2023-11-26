using Microsoft.EntityFrameworkCore;

namespace Store.Data.EF
{
    internal class BookRepository : IBookRepository
    {
        private readonly DbContextFactory dbContextFactory;
        private StoreDbContext DbContext => dbContextFactory.Create(typeof(BookRepository));

        public BookRepository(DbContextFactory dbContextFactory)
        {
            this.dbContextFactory = dbContextFactory;
        }

        public Book[] GetAllBooks()
        {
            return DbContext.Books
                .AsEnumerable()
                 .Select(Book.Mapper.Map)
                 .ToArray();
        }

        public Book[] GetAllBooksByAuthorOrTitle(string query)
        {
            return DbContext.Books
                .Where(b => Microsoft.EntityFrameworkCore.EF.Functions.Contains(b.Author, query) || Microsoft.EntityFrameworkCore.EF.Functions.Contains(b.Title, query))
                .AsEnumerable()
                .Select(Book.Mapper.Map)
                .ToArray();
        }

        public Book GetBookById(int id)
        {
            var book = DbContext.Books.Single(b => b.Id == id);
            return Book.Mapper.Map(book);
        }

        public Book[] GetBookByIsbn(string isbn)
        {
            return DbContext.Books
                .Where(b => b.Isbn == isbn)
                .AsEnumerable()
                .Select(Book.Mapper.Map).ToArray();
        }

        public Book[] GetBooksByIds(IEnumerable<int> bookIds)
        {  
            return DbContext.Books
                .Where(b => bookIds.Contains(b.Id))
                .AsEnumerable()
                .Select(Book.Mapper.Map)
                .ToArray();
        }
    }
}
