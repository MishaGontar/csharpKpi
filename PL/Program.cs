using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;

namespace PL;

class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        LibraryContext context = new LibraryContext(options);

        IRepository<Book> bookRepository = new BookRepository(context);
        IRepository<Reader> readerRepository = new ReaderRepository(context);

        IBookService bookService = new BookService(bookRepository);
        IReaderService readerService = new ReaderService(readerRepository);

        SetUpData(bookService, readerService);
        ConsoleUI consoleUI = new ConsoleUI(bookService, readerService);

        consoleUI.Run();
    }

    private static void SetUpData(IBookService bookService, IReaderService readerService)
    {
        bookService.AddBook(new Book { Title = "Назва 1", Author = "Автор 1", Topic = "Тематика 1", Quantity = 5 });
        bookService.AddBook(new Book { Title = "Назва 2", Author = "Автор 2", Topic = "Тематика 1", Quantity = 1 });
        bookService.AddBook(new Book { Title = "Назва 3", Author = "Автор 2", Topic = "Тематика 2", Quantity = 0 });
        bookService.AddBook(new Book { Title = "Назва 4", Author = "Автор 3", Topic = "Тематика 3", Quantity = 10});

        readerService.AddReader(new Reader { Name = "Читач 1", Email = "email 1" });
        readerService.AddReader(new Reader { Name = "Читач 2", Email = "email 2" });
        readerService.AddReader(new Reader { Name = "Читач 3", Email = "email 3" });
        readerService.AddReader(new Reader { Name = "Читач 4", Email = "email 4" });
    }
}