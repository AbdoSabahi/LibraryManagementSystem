using System;

namespace LibraryManagementSystem.Models
{
    public abstract class LibraryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime AddedDate { get; set; }

        protected LibraryItem(int id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            Id = id;
            Title = title;
            AddedDate = DateTime.Now;
        }

        public abstract string GetInfo();
    }
}