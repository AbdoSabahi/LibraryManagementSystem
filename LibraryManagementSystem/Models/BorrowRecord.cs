using System;

namespace LibraryManagementSystem.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public Book Book { get; set; }
        public Member Member { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public BorrowRecord(int id, Book book, Member member)
        {
            Id = id;
            Book = book ?? throw new ArgumentNullException(nameof(book));
            Member = member ?? throw new ArgumentNullException(nameof(member));
            BorrowDate = DateTime.Now;
            ReturnDate = null;
        }

        public bool IsLate()
        {
            if (ReturnDate.HasValue)
                return false;

            return (DateTime.Now - BorrowDate).TotalDays > 14;
        }
    }
}