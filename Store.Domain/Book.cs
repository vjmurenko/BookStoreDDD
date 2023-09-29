using System.Text.RegularExpressions;

namespace Store
{
    public class Book
    {
        public int Id { get; }
        public string Title { get; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public Book(int id, string title, string author, string isbn)
        {
            Author = author;
            Isbn = isbn;
            Id = id;
            Title = title;
        }

        internal static bool IsIsbn(string query)
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