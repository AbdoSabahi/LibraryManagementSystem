using System;
using LibraryManagementSystem.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Member : ISearchable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public Book[] BorrowedBooks { get; set; }

        public Member(int id, string name, string email)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.", nameof(email));

            Id = id;
            Name = name;
            Email = email;
            JoinDate = DateTime.Now;
            BorrowedBooks = Array.Empty<Book>();
        }

        public virtual string GetInfo()
        {
            return $"[{Id}] {Name} - {Email} - Joined: {JoinDate:yyyy-MM-dd}";
        }

        public bool MatchesQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return false;

            return Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                || Email.Contains(query, StringComparison.OrdinalIgnoreCase);
        }
    }
}