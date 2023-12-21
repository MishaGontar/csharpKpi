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

        ConsoleUI consoleUI = new ConsoleUI(bookService, readerService);

        consoleUI.Run();
    }
}