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
        Console.WriteLine("Ласкаво просимо до Системи управління бібліотекою!");

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
                case "8":
                    AddReader();
                    break;
                case "9":
                    getBook();
                    break;
                case "10":
                    removeBook();
                    break;
                case "Q":
                case "q":
                    Console.WriteLine("Вихід з програми. До побачення!");
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Будь ласка спробуйте ще раз.");
                    break;
            }
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine("\nМеню:");
        Console.WriteLine("1. Вивести всіх книжок");
        Console.WriteLine("2. Додати книжку");
        Console.WriteLine("3. Пошук по назві");
        Console.WriteLine("4. Пошук по автору");
        Console.WriteLine("5. Пошук по тематиці");
        Console.WriteLine("6. Видалити книжку");
        Console.WriteLine("7. Показати всіх читачів");
        Console.WriteLine("8. Додати читача");
        Console.WriteLine("9. Додати книжку читачу");
        Console.WriteLine("10. Видалити книжку читачу");
        Console.WriteLine("Q. Вийти");
        Console.Write("Введіть число : ");
    }

    private void DisplayAllBooks()
    {
        Console.WriteLine("\nВсі книжки:");

        List<Book> allBooks = _bookService.GetAllBooks();

        foreach (var book in allBooks)
        {
            Console.WriteLine($"Id: {book.Id} Назва: {book.Title}, Автор: {book.Author}, Тематика: {book.Topic}");
        }
    }

    private void AddBook()
    {
        Console.Write("\nВведіть назву книги: ");
        string title = Console.ReadLine();

        Console.Write("Введіть автора книги: ");
        string author = Console.ReadLine();

        Console.Write("Введіть тему книги: ");
        string topic = Console.ReadLine();

        Book newBook = new Book { Title = title, Author = author, Topic = topic };
        _bookService.AddBook(newBook);

        Console.WriteLine("Книгу успішно додано!");
    }

    private void SearchBooksByTitle()
    {
        Console.Write("\nВведіть назву для пошуку: ");
        string title = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByTitle(title);

        DisplaySearchResults(foundBooks);
    }

    private void SearchBooksByAuthor()
    {
        Console.Write("\nВведіть автора для пошуку: ");
        string author = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByAuthor(author);

        DisplaySearchResults(foundBooks);
    }

    private void SearchBooksByTopic()
    {
        Console.Write("\nВведіть тему для пошуку: ");
        string topic = Console.ReadLine();

        List<Book> foundBooks = _bookService.SearchBooksByTopic(topic);

        DisplaySearchResults(foundBooks);
    }

    private void RemoveBook()
    {
        Console.Write("\nВведіть id книги, яку потрібно видалити: ");
        int id = Convert.ToInt32(Console.ReadLine());
        _bookService.DeleteBook(id);
        Console.WriteLine("Книгу успішно видалено!");
    }

    private void DisplaySearchResults(List<Book> foundBooks)
    {
        Console.WriteLine("\nРезультати пошуку:");

        if (foundBooks.Count > 0)
        {
            foreach (var book in foundBooks)
            {
                Console.WriteLine($"Id: {book.Id} Назва: {book.Title}, Автор: {book.Author}, Тематика: {book.Topic}");
            }
        }
        else
        {
            Console.WriteLine("Відповідних книг не знайдено.");
        }
    }

    private void DisplayAllReaders()
    {
        Console.WriteLine("\nУсі читачі:");

        List<Reader> allReaders = _readerService.GetAllReaders();

        foreach (var reader in allReaders)
        {
            Console.WriteLine(
                $"id: {reader.Id} Ім'я: {reader.Name}, Пошта: {reader.Email} , Кількість книжок взято : {reader.GetsBooks.Count}");
        }
    }

    private void AddReader()
    {
        Console.Write("\nВведіть ім'я читача: ");
        string name = Console.ReadLine();

        Console.Write("Введіть адресу електронної пошти читача: ");
        string email = Console.ReadLine();

        Reader newReader = new Reader { Name = name, Email = email };
        _readerService.AddReader(newReader);

        Console.WriteLine("Читач успішно додано!");
    }

    private void getBook()
    {
        Console.Write("\nВведіть id читача: ");
        int idReader = Convert.ToInt32(Console.ReadLine());
        Reader reader = _readerService.GetReaderById(idReader);

        Console.Write("\nВведіть id книжки: ");
        int idBook = Convert.ToInt32(Console.ReadLine());
        Book book = _bookService.GetBookById(idBook);

        _readerService.GetBook(book, reader);
    }

    private void removeBook()
    {
        Console.Write("\nВведіть id читача: ");
        int idReader = Convert.ToInt32(Console.ReadLine());
        Reader reader = _readerService.GetReaderById(idReader);

        Console.Write("\nВведіть id книжки: ");
        int idBook = Convert.ToInt32(Console.ReadLine());
        Book book = _bookService.GetBookById(idBook);

        _readerService.GiveBackBook(book, reader);
    }
}