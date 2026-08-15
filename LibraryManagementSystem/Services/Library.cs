using System;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services
{
    public class Library
    {
        private Book[] books;
        private int bookCount;

        private Member[] members;
        private int memberCount;

        private BorrowRecord[] borrowRecords;
        private int borrowRecordCount;

        private int nextBookId;
        private int nextMemberId;
        private int nextBorrowRecordId;

        public Library()
        {
            books = new Book[100];
            bookCount = 0;

            members = new Member[100];
            memberCount = 0;

            borrowRecords = new BorrowRecord[500];
            borrowRecordCount = 0;

            nextBookId = 1;
            nextMemberId = 1;
            nextBorrowRecordId = 1;
        }

        public Book AddBook(string title, string author, int year, string genre)
        {
            if (bookCount >= books.Length)
                throw new InvalidOperationException("Library book capacity reached.");

            var book = new Book(nextBookId, title, author, year, genre);
            books[bookCount] = book;
            bookCount++;
            nextBookId++;

            return book;
        }

        public Member RegisterMember(string name, string email, bool isPremium)
        {
            if (memberCount >= members.Length)
                throw new InvalidOperationException("Library member capacity reached.");

            Member member = isPremium
                ? new PremiumMember(nextMemberId, name, email)
                : new Member(nextMemberId, name, email);

            members[memberCount] = member;
            memberCount++;
            nextMemberId++;

            return member;
        }

        public BorrowRecord BorrowBook(int bookId, int memberId)
        {
            Book book = FindBookById(bookId);
            if (book == null)
                throw new ArgumentException($"No book found with Id {bookId}.");

            if (!book.IsAvailable)
                throw new InvalidOperationException($"Book \"{book.Title}\" is not available.");

            Member member = FindMemberById(memberId);
            if (member == null)
                throw new ArgumentException($"No member found with Id {memberId}.");

            if (borrowRecordCount >= borrowRecords.Length)
                throw new InvalidOperationException("Borrow record capacity reached.");

            var record = new BorrowRecord(nextBorrowRecordId, book, member);
            borrowRecords[borrowRecordCount] = record;
            borrowRecordCount++;
            nextBorrowRecordId++;

            book.IsAvailable = false;

            AddBorrowedBookToMember(member, book);

            return record;
        }

        public void ReturnBook(int bookId)
        {
            Book book = FindBookById(bookId);
            if (book == null)
                throw new ArgumentException($"No book found with Id {bookId}.");

            BorrowRecord openRecord = null;
            for (int i = 0; i < borrowRecordCount; i++)
            {
                if (borrowRecords[i].Book.Id == bookId && borrowRecords[i].ReturnDate == null)
                {
                    openRecord = borrowRecords[i];
                    break;
                }
            }

            if (openRecord == null)
                throw new InvalidOperationException($"No open borrow record found for book Id {bookId}.");

            openRecord.ReturnDate = DateTime.Now;
            book.IsAvailable = true;
        }

        public void Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Search query cannot be empty.");
                return;
            }

            Console.WriteLine($"--- Search results for \"{query}\" ---");

            Console.WriteLine("Books:");
            for (int i = 0; i < bookCount; i++)
            {
                if (books[i].MatchesQuery(query))
                    Console.WriteLine("  " + books[i].GetInfo());
            }

            Console.WriteLine("Members:");
            for (int i = 0; i < memberCount; i++)
            {
                if (members[i].MatchesQuery(query))
                    Console.WriteLine("  " + members[i].GetInfo());
            }
        }

        public void ShowAvailableBooks()
        {
            Console.WriteLine("--- Available Books ---");
            bool anyFound = false;

            for (int i = 0; i < bookCount; i++)
            {
                if (books[i].IsAvailable)
                {
                    Console.WriteLine(books[i].GetInfo());
                    anyFound = true;
                }
            }

            if (!anyFound)
                Console.WriteLine("No available books at the moment.");
        }

        public void ShowMemberBorrowHistory(int memberId)
        {
            Member member = FindMemberById(memberId);
            if (member == null)
            {
                Console.WriteLine($"No member found with Id {memberId}.");
                return;
            }

            Console.WriteLine($"--- Borrow history for {member.Name} ---");
            bool anyFound = false;

            for (int i = 0; i < borrowRecordCount; i++)
            {
                if (borrowRecords[i].Member.Id == memberId)
                {
                    string status = borrowRecords[i].ReturnDate.HasValue
                        ? $"Returned on {borrowRecords[i].ReturnDate:yyyy-MM-dd}"
                        : "Still borrowed";

                    Console.WriteLine($"  \"{borrowRecords[i].Book.Title}\" - Borrowed on {borrowRecords[i].BorrowDate:yyyy-MM-dd} - {status}");
                    anyFound = true;
                }
            }

            if (!anyFound)
                Console.WriteLine("No borrow history found for this member.");
        }

        public void ShowLateReport()
        {
            Console.WriteLine("--- Late Books Report ---");
            bool anyFound = false;

            for (int i = 0; i < borrowRecordCount; i++)
            {
                if (borrowRecords[i].ReturnDate == null && borrowRecords[i].IsLate())
                {
                    int daysLate = (int)(DateTime.Now - borrowRecords[i].BorrowDate).TotalDays;
                    Console.WriteLine($"  {borrowRecords[i].Member.Name} - \"{borrowRecords[i].Book.Title}\" - {daysLate} days late");
                    anyFound = true;
                }
            }

            if (!anyFound)
                Console.WriteLine("No late books.");
        }

        private Book FindBookById(int id)
        {
            for (int i = 0; i < bookCount; i++)
            {
                if (books[i].Id == id)
                    return books[i];
            }
            return null;
        }

        private Member FindMemberById(int id)
        {
            for (int i = 0; i < memberCount; i++)
            {
                if (members[i].Id == id)
                    return members[i];
            }
            return null;
        }

        private void AddBorrowedBookToMember(Member member, Book book)
        {
            Book[] updated = new Book[member.BorrowedBooks.Length + 1];
            for (int i = 0; i < member.BorrowedBooks.Length; i++)
            {
                updated[i] = member.BorrowedBooks[i];
            }
            updated[updated.Length - 1] = book;
            member.BorrowedBooks = updated;
        }
    }
}