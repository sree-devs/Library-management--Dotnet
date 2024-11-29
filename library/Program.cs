using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Library library = new Library();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("====== Library Management System ======");
            Console.WriteLine("1. Add a Book");
            Console.WriteLine("2. Borrow a Book");
            Console.WriteLine("3. Return a Book");
            Console.WriteLine("4. View All Books");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice (1-5): ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddBook(library);
                    break;
                case "2":
                    BorrowBook(library);
                    break;
                case "3":
                    ReturnBook(library);
                    break;
                case "4":
                    ViewBooks(library);
                    break;
                case "5":
                    Console.WriteLine("Exiting... Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press Enter to try again.");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void AddBook(Library library)
    {
        Console.Clear();
        Console.WriteLine("====== Add a Book ======");
        Console.Write("Enter Book Title: ");
        string? title = Console.ReadLine();
        Console.Write("Enter Author: ");
        string? author = Console.ReadLine();
        Console.Write("Enter ISBN: ");
        string? isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(author) || string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("All fields are required.");
            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        if (library.AddBook(new Book(title, author, isbn)))
        {
            Console.WriteLine("Book added successfully!");
        }
        else
        {
            Console.WriteLine("A book with this ISBN already exists.");
        }
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }

    static void BorrowBook(Library library)
    {
        Console.Clear();
        Console.WriteLine("====== Borrow a Book ======");
        Console.Write("Enter ISBN of the book to borrow: ");
        string? isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("ISBN cannot be empty.");
            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        if (library.BorrowBook(isbn))
        {
            Console.WriteLine("Book borrowed successfully!");
        }
        else
        {
            Console.WriteLine("Book is unavailable or does not exist.");
        }
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }

    static void ReturnBook(Library library)
    {
        Console.Clear();
        Console.WriteLine("====== Return a Book ======");
        Console.Write("Enter ISBN of the book to return: ");
        string? isbn = Console.ReadLine();

        if (string.IsNullOrEmpty(isbn))
        {
            Console.WriteLine("ISBN cannot be empty.");
            Console.WriteLine("Press Enter to return to the menu.");
            Console.ReadLine();
            return;
        }

        if (library.ReturnBook(isbn))
        {
            Console.WriteLine("Book returned successfully!");
        }
        else
        {
            Console.WriteLine("Book was not borrowed or does not exist.");
        }
        Console.WriteLine("Press Enter to return to the menu.");
        Console.ReadLine();
    }

    static void ViewBooks(Library library)
    {
        Console.Clear();
        Console.WriteLine("====== View All Books ======");
        library.DisplayBooks();
        Console.WriteLine("\nPress Enter to return to the menu.");
        Console.ReadLine();
    }
}

class Book
{
    public string? Title { get; private set; }
    public string? Author { get; private set; }
    public string? ISBN { get; private set; }
    public bool IsBorrowed { get; private set; }
    public DateTime? DueDate { get; private set; }

    public Book(string? title, string? author, string? isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
        IsBorrowed = false;
    }

    public void Borrow()
    {
        IsBorrowed = true;
        DueDate = DateTime.Now.AddDays(14); // 2-week loan period
    }

    public void Return()
    {
        IsBorrowed = false;
        DueDate = null;
    }

    public override string ToString()
    {
        return $"{Title} by {Author} (ISBN: {ISBN}) - " +
               (IsBorrowed ? $"Borrowed, Due: {DueDate?.ToShortDateString()}" : "Available");
    }
}

class Library
{
    private List<Book> books;

    public Library()
    {
        books = new List<Book>();
    }

    public bool AddBook(Book book)
    {
        if (books.Exists(b => b.ISBN == book.ISBN))
        {
            return false; // Duplicate ISBN not allowed
        }
        books.Add(book);
        return true;
    }

    public bool BorrowBook(string isbn)
    {
        Book? book = books.Find(b => b.ISBN == isbn && !b.IsBorrowed);
        if (book != null)
        {
            book.Borrow();
            return true;
        }
        return false; // Book not found or already borrowed
    }

    public bool ReturnBook(string isbn)
    {
        Book? book = books.Find(b => b.ISBN == isbn && b.IsBorrowed);
        if (book != null)
        {
            book.Return();
            return true;
        }
        return false; // Book not found or not borrowed
    }

    public void DisplayBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("No books in the library.");
            return;
        }

        foreach (var book in books)
        {
            Console.WriteLine(book);
        }
    }
}
