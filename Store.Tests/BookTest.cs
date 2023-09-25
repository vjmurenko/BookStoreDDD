using Moq;

namespace Store.Tests;

public class BookTest
{

    [Fact]
    public void IsIsbn_WithISBN10_ReturnTrue()
    {
        bool actual = Book.IsIsbn("ISBN-10234-12345");

        Assert.True(actual);
    }

    [Fact]
    public void IsIsbn_With1111_ReturnFalse()
    {
        bool actual = Book.IsIsbn("111");
        Assert.False(actual);
    }

    [Fact]
    public void IsIsbn_WithNull_ReturnFalse()
    {
        bool actual = Book.IsIsbn(null);
        Assert.False(actual);
    }

    [Fact]
    public void IsIsbn_WithIsbn13_ReturnTrue()
    {
        bool actual = Book.IsIsbn("ISBN-12345-12345-113");
        Assert.True(actual);
    }

    [Fact]
    public void IsIsbn_TrashStart_ReturnFalse()
    {
        bool actual = Book.IsIsbn("abcISBN-12345-12434");
        Assert.False(actual);
    }

    [Fact]
    public void IsIsbn_EmptyString_ReturnFalse()
    {
        bool actual = Book.IsIsbn("   ");
        Assert.False(actual);
    }
}