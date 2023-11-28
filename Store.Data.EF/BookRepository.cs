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

        public async Task<Book[]> GetAllBooksAsync()
        {
            var books = await DbContext.Books
                .ToArrayAsync();

            return books.Select(Book.Mapper.Map).ToArray();
        }
        
        public async Task<Book[]> GetAllBooksByAuthorOrTitleAsync(string query)
        {
            var books = await  DbContext.Books
                .Where(b => Microsoft.EntityFrameworkCore.EF.Functions.Contains(b.Author, query) || Microsoft.EntityFrameworkCore.EF.Functions.Contains(b.Title, query))
                .ToArrayAsync();
            
            return books.Select(Book.Mapper.Map).ToArray();
        }
        
        public async  Task<Book> GetBookByIdAsync(int id)
        {
            var book =  await DbContext.Books
                .SingleAsync(b => b.Id == id);

            return Book.Mapper.Map(book);
        }
        
        public async  Task<Book[]> GetBookByIsbnAsync(string isbn)
        {
            var books = await DbContext.Books
                .Where(b => b.Isbn == isbn)
                .ToArrayAsync();

            return books.Select(Book.Mapper.Map).ToArray();
        }
        
        public async Task<Book[]> GetBooksByIdsAsync(IEnumerable<int> bookIds)
        {
            var books = await DbContext.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToArrayAsync();

            return books.Select(Book.Mapper.Map).ToArray();
        }
    }
}
