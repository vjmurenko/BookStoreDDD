using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Data.EF
{
    internal class BookRepository : IBookRepository
    {
        public Order Create()
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllBooks()
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllBooksByAuthorOrTitle(string query)
        {
            throw new NotImplementedException();
        }

        public Book GetBookById(int id)
        {
            throw new NotImplementedException();
        }

        public Book[] GetBookByIsbn(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetBooksByIds(IEnumerable<int> bookIds)
        {
            throw new NotImplementedException();
        }

        public Order GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
