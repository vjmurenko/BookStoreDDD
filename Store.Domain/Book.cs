using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public Book(int id, string title, string author, string isbn, string description, decimal price)
        {
            Author = author;
            Isbn = isbn;
            Description = description;
            Price = price;
            Id = id;
            Title = title;
        }

        public static bool IsIsbn(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return false;
            }
            query = query?.Replace("-", "").Replace(" ", "");
            return Regex.IsMatch(query, @"^ISBN\d{10}(\d{3})?$");
        }
    }
}