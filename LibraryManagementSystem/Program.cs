using System;
using LibraryManagementSystem.Services;

namespace LibraryManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            SeedData(library);

            bool running = true;

            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("===== Library Management System =====");
                Console.WriteLine("1. Add a book");
                Console.WriteLine("2. Register a member");
                Console.WriteLine("3. Borrow a book");
                Console.WriteLine("4. Return a book");
                Console.WriteLine("5. Search catalog");
                Console.WriteLine("6. Show available books");
                Console.WriteLine("7. Show member borrow history");
                Console.WriteLine("8. Show late books report");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddBookFlow(library);
                            break;
                        case "2":
                            RegisterMemberFlow(library);
                            break;
                        case "3":
                            BorrowBookFlow(library);
                            break;
                        case "4":
                            ReturnBookFlow(library);
                            break;
                        case "5":
                            SearchFlow(library);
                            break;
                        case "6":
                            library.ShowAvailableBooks();
                            break;
                        case "7":
                            MemberHistoryFlow(library);
                            break;
                        case "8":
                            library.ShowLateReport();
                            break;
                        case "0":
                            running = false;
                            Console.WriteLine("Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        static void AddBookFlow(Library library)
        {
            Console.Write("Title: ");
            string title = Console.ReadLine();

            Console.Write("Author: ");
            string author = Console.ReadLine();

            Console.Write("Year: ");
            int year = int.Parse(Console.ReadLine());

            Console.Write("Genre: ");
            string genre = Console.ReadLine();

            var book = library.AddBook(title, author, year, genre);
            Console.WriteLine($"Book added successfully: {book.GetInfo()}");
        }

        static void RegisterMemberFlow(Library library)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Premium member? (y/n): ");
            string premiumInput = Console.ReadLine();
            bool isPremium = premiumInput != null && premiumInput.Trim().ToLower() == "y";

            var member = library.RegisterMember(name, email, isPremium);
            Console.WriteLine($"Member registered successfully: {member.GetInfo()}");
        }

        static void BorrowBookFlow(Library library)
        {
            Console.Write("Book Id: ");
            int bookId = int.Parse(Console.ReadLine());

            Console.Write("Member Id: ");
            int memberId = int.Parse(Console.ReadLine());

            var record = library.BorrowBook(bookId, memberId);
            Console.WriteLine($"Book borrowed successfully. Record Id: {record.Id}");
        }

        static void ReturnBookFlow(Library library)
        {
            Console.Write("Book Id: ");
            int bookId = int.Parse(Console.ReadLine());

            library.ReturnBook(bookId);
            Console.WriteLine("Book returned successfully.");
        }

        static void SearchFlow(Library library)
        {
            Console.Write("Enter search query: ");
            string query = Console.ReadLine();
            library.Search(query);
        }

        static void MemberHistoryFlow(Library library)
        {
            Console.Write("Member Id: ");
            int memberId = int.Parse(Console.ReadLine());
            library.ShowMemberBorrowHistory(memberId);
        }

        static void SeedData(Library library)
        {
            library.AddBook("Clean Code", "Robert C. Martin", 2008, "Software Engineering");
            library.AddBook("The Pragmatic Programmer", "Andrew Hunt", 1999, "Software Engineering");
            library.AddBook("1984", "George Orwell", 1949, "Fiction");
            library.AddBook("Dune", "Frank Herbert", 1965, "Science Fiction");

            library.RegisterMember("Ahmed Ali", "ahmed@example.com", false);
            library.RegisterMember("Sara Mostafa", "sara@example.com", true);

            library.BorrowBook(1, 1);
            library.BorrowBook(3, 2);
        }
    }
}
