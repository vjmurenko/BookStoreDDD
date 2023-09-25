using Moq;

namespace Store.Tests
{
    public class BookServiceTest
    {
        [Fact]
        public void GetBooksByQuery_CallWithISBN_SearchByIsbn()
        {

            var bookRepositoryStub = new Mock<IBookRepository>();
            bookRepositoryStub.Setup(b => b.GetAllBooksByAuthorOrTitle(It.IsAny<string>()))
                .Returns(new[] { new Book(1, "", "", "") });
            bookRepositoryStub.Setup(b => b.GetBookByIsbn(It.IsAny<string>()))
                .Returns(new[] { new Book(2, "", "", "") });

            var bookService = new BookService(bookRepositoryStub.Object);
            var actual = bookService.GetBooksByQuery("ISBN-12345-12345");
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }


        [Fact]
        public void GetBooksByQuery_CallWithWrongISBN_SearchByAuthor()
        {
            var bookRepositoryStub = new Mock<IBookRepository>();

            bookRepositoryStub.Setup(b => b.GetAllBooksByAuthorOrTitle(It.IsAny<string>()))
                .Returns(new[] { new Book(1, "", "", "") });
            bookRepositoryStub.Setup(b => b.GetBookByIsbn(It.IsAny<string>()))
                .Returns(new[] { new Book(2, "", "", "") });
            var bookService = new BookService(bookRepositoryStub.Object);

            var actual = bookService.GetBooksByQuery("wrongISbn");
            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }
    }
}