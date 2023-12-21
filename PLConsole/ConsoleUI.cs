using BLL;
using DLL2;

namespace PL;

public class ConsoleUI(IBookService bookService, IReaderService readerService)
{
    private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

    private readonly IReaderService _readerService =
        readerService ?? throw new ArgumentNullException(nameof(readerService));

    public void Run()
    {
        Console.WriteLine("Welcome to the Library Management System!");

        while (true)
        {
            DisplayMenu();
            string choice = Console.ReadLine() ?? throw new InvalidOperationException();

            switch (choice)
            {
                case "1":
                    DisplayAllBooks();
                    break;
                case "2":
                    AddBook();
                    break;
                case "3":
                    SearchBooksByTitle();
                    break;
                case "4":
                    SearchBooksByAuthor();
                    break;
                case "5":
                    SearchBooksByTopic();
                    break;
                case "6":
                    RemoveBook();
                    break;
                case "7":
                    DisplayAllReaders();
                    break;
                case "Q":
                case "q":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Display All Books");
        Console.WriteLine("2. Add a Book");
        Console.WriteLine("3. Search Books by Title");
        Console.WriteLine("4. Search Books by Author");
        Console.WriteLine("5. Search Books by Topic");
        Console.WriteLine("6. Remove a Book");
        Console.WriteLine("7. Display All Readers");
        Console.WriteLine("Q. Quit");
        Console.Write("Please enter your choice: ");
    }

    private void DisplayAllBooks()
    {
        Console.WriteLine("\nAll Books:");

        List<Book> allBooks = _bookService.GetAllBooks();

        foreach (var book in allBooks)
        {
            Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Topic: {book.Topic}");
        }
    }

    private void AddBook()
    {
        Console.Write("\nEnter the title of the book: ");
        string title = Console.ReadLine();

        Console.Write("Enter the author of the book: ");
        string author = Console.ReadLine();

        Console.Write("Enter the topic of the book: ");
        string topic = Console.ReadLine();

        Book newBook = new Book { Title = title, Author = author, Topic = topic };
        _bookService.AddBook(newBook);

        Console.WriteLine("Book added successfully!");
    }

    private void SearchBooksByTitle()
    {
        Console.Write("\nEnter the title to search for: ");
        string title = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByTitle(title);

        DisplaySearchResults(foundBooks);
    }

    private void SearchBooksByAuthor()
    {
        Console.Write("\nEnter the author to search for: ");
        string author = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByAuthor(author);

        DisplaySearchResults(foundBooks);
    }

    private void SearchBooksByTopic()
    {
        Console.Write("\nEnter the topic to search for: ");
        string topic = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByTopic(topic);

        DisplaySearchResults(foundBooks);
    }

    private void RemoveBook()
    {
        Console.Write("\nEnter the title of the book to remove: ");
        string title = Console.ReadLine();

        Book bookToRemove = _bookService.SearchBooksByTopic(title ?? throw new InvalidOperationException())[0];

        _bookService.DeleteBook(bookToRemove.Id);
        Console.WriteLine("Book removed successfully!");
    }

    private void DisplaySearchResults(List<Book> foundBooks)
    {
        Console.WriteLine("\nSearch Results:");

        if (foundBooks.Count > 0)
        {
            foreach (var book in foundBooks)
            {
                Console.WriteLine($"Title: {book.Title}, Author: {book.Author}, Topic: {book.Topic}");
            }
        }
        else
        {
            Console.WriteLine("No matching books found.");
        }
    }

    private void DisplayAllReaders()
    {
        Console.WriteLine("\nAll Readers:");

        List<Reader> allReaders = _readerService.GetAllReaders();

        foreach (var reader in allReaders)
        {
            Console.WriteLine($"Name: {reader.Name}, Email: {reader.Email}");
        }
    }
}