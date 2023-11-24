using Store.Data;
using System;
using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        public int Id => _dto.Id;
        public string Title
        {
            get => _dto.Title;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(Title));
                }
                _dto.Title = value.Trim();
            }
        }
        public string Author { get => _dto.Author; set => _dto.Author = value?.Trim(); }
        public string Isbn
        {
            get
            {
                return _dto.Isbn;
            }
            set
            {
                if (TryFormatIsbn(value, out var formattedIsbn))
                {
                    _dto.Isbn = formattedIsbn;
                }
                else
                {
                    throw new FormatException(nameof(Isbn));
                }
            }
        }
        public decimal Price
        {
            get => _dto.Price;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException(nameof(Price));
                }
                _dto.Price = value;
            }
        }
        public string Description { get => _dto.Description; set => _dto.Description = value; }
        private readonly BookDto _dto;

        internal Book(BookDto bookDto)
        {
            _dto = bookDto;
        }

        public static class DtoFactory
        {
            public static BookDto Create(string title, string author, string isbn, decimal price, string description)
            {
                if (string.IsNullOrEmpty(title))
                {
                    throw new ArgumentNullException(nameof(title));
                }

                if (TryFormatIsbn(isbn, out string formattedIsbn))
                {
                    isbn = formattedIsbn;
                }
                else
                {
                    throw new ArgumentException(nameof(isbn));
                }
                if (price < 0)
                {
                    throw new InvalidOperationException(nameof(price));
                }

                return new BookDto()
                {
                    Author = author,
                    Description = description,
                    Isbn = isbn,
                    Price = price,
                    Title = title
                };
            }
        }

        public static class Mapper
        {
            public static Book Map(BookDto bookDto) => new Book(bookDto);
            public static BookDto Map(Book book) => book._dto;
        }

        public static bool IsIsbn(string isbn) => TryFormatIsbn(isbn, out _);
        
        private static bool TryFormatIsbn(string isbn, out string formattedIsbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                formattedIsbn = null;
                return false;
            }
            formattedIsbn = isbn?
                .Replace("-", "")
                .Replace(" ", "")
                .ToUpper();
            return Regex.IsMatch(formattedIsbn, @"^ISBN\d{10}(\d{3})?$");
        }
    }
}