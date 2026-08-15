using System;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Book : LibraryItem, ISearchable
    {
        public string Author { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }

        public Book(int id, string title, string author, int year, string genre)
            : base(id, title)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author cannot be empty.", nameof(author));

            Author = author;
            Year = year;
            Genre = genre;
            IsAvailable = true;
        }

        public override string GetInfo()
        {
            string status = IsAvailable ? "Available" : "Borrowed";
            return $"[{Id}] \"{Title}\" by {Author} ({Year}) - {Genre} - {status}";
        }

        public bool MatchesQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return false;

            return Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Author.Contains(query, StringComparison.OrdinalIgnoreCase)
                || (Genre != null && Genre.Contains(query, StringComparison.OrdinalIgnoreCase));
        }
    }
}