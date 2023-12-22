using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TestLab3;

[TestClass]
public class TestLab3
{
    private IBookService bookService;
    private IReaderService readerService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        LibraryContext context = new LibraryContext(options);

        IRepository<Book> bookRepository = new BookRepository(context);
        IRepository<Reader> readerRepository = new ReaderRepository(context);

        bookService = new BookService(bookRepository);
        readerService = new ReaderService(readerRepository);

        context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void GetAllBooks_ReturnsAllBooks()
    {
        bookService.AddBook(new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        var result = bookService.GetAllBooks();

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetBookById_ValidId_ReturnsBook()
    {
        var book = new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" };
        bookService.AddBook(book);

        var result = bookService.GetBookById(book.Id);

        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(book.Id));
    }

    [TestMethod]
    public void GetAllReaders_ReturnsAllReaders()
    {
        readerService.AddReader(new Reader { Name = "Reader1", Email = "reader1@example.com" });
        readerService.AddReader(new Reader { Name = "Reader2", Email = "reader2@example.com" });

        var result = readerService.GetAllReaders();

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetReaderById_ValidId_ReturnsReader()
    {
        var reader = new Reader { Name = "Reader1", Email = "reader1@example.com" };
        readerService.AddReader(reader);

        var result = readerService.GetReaderById(reader.Id);

        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(reader.Id));
    }

    [TestMethod]
    public void GetBooksByTitle()
    {
        string title = "simple title";
        bookService.AddBook(new Book { Title = title, Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        List<Book> foundBooks = bookService.SearchBooksByTitle(title);

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(1));

        bookService.AddBook(new Book { Title = title, Author = "Author2", Topic = "Topic1" });

        foundBooks = bookService.SearchBooksByTitle(title);

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetBooksByAuthor()
    {
        string author = "Author2";
        Console.Write(bookService.GetAllBooks().Count);
        bookService.AddBook(new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        List<Book> foundBooks = bookService.SearchBooksByAuthor(author);
        Console.Write(foundBooks.Count);
        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(1));

        bookService.AddBook(new Book { Title = "Book1", Author = "Author2", Topic = "Topic1" });

        foundBooks = bookService.SearchBooksByAuthor(author);

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetBooksByTopic()
    {
        string topic = "Topic1";
        bookService.AddBook(new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        List<Book> foundBooks = bookService.SearchBooksByTopic(topic);

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(1));

        bookService.AddBook(new Book { Title = "Book1", Author = "Author2", Topic = "Topic1" });

        foundBooks = bookService.SearchBooksByTopic(topic);

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void RemoveBookById()
    {
        bookService.AddBook(new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        var foundBooks = bookService.GetAllBooks();
        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(2));

        bookService.DeleteBook(1);
        foundBooks = bookService.GetAllBooks();

        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(1));
    }

    [TestMethod]
    public void ReaderGetBooks()
    {
        for (int i = 0; i < 12; i++)
        {
            bookService.AddBook(new Book { Title = "Book" + i, Author = "Author" + i, Topic = "Topic" + i });
        }

        List<Book> foundBooks = bookService.GetAllBooks();
        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(12));

        Reader reader = new Reader { Name = "Name", Email = "test@gmail.com" };
        readerService.AddReader(reader);

        var readers = readerService.GetAllReaders();
        Assert.IsNotNull(readers);
        Assert.That(readers.Count, Is.EqualTo(1));
        Assert.That(reader.GetsBooks.Count, Is.EqualTo(0));

        for (int i = 0; i < 10; i++)
        {
            readerService.getBook(foundBooks[i], reader);
        }

        Assert.That(reader.GetsBooks.Count, Is.EqualTo(10));

        Assert.Throws<AlreadyGetBook>(() => readerService.getBook(foundBooks[0], reader));
        Assert.Throws<MoreThanCan>(() => readerService.getBook(foundBooks[11], reader));
    }
}