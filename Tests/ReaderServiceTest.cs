using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TestLab3;

[TestClass]
public class ReaderServiceTest
{
    private IBookService _bookService;
    private IReaderService _readerService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        LibraryContext context = new LibraryContext(options);

        IRepository<Book> bookRepository = new BookRepository(context);
        IRepository<Reader> readerRepository = new ReaderRepository(context);

        _bookService = new BookService(bookRepository);
        _readerService = new ReaderService(readerRepository);

        context.Database.EnsureDeleted();
    }

    [TestMethod]
    public void ReaderGetBooks()
    {
        for (int i = 0; i < 12; i++)
        {
            _bookService.AddBook(new Book
                { Title = "Book" + i, Author = "Author" + i, Topic = "Topic" + i, Quantity = 1 });
        }

        List<Book> foundBooks = _bookService.GetAllBooks();
        Assert.IsNotNull(foundBooks);
        Assert.That(foundBooks.Count, Is.EqualTo(12));

        Reader reader = new Reader { Name = "Name", Email = "test@gmail.com" };
        _readerService.AddReader(reader);

        var readers = _readerService.GetAllReaders();
        Assert.IsNotNull(readers);
        Assert.That(readers.Count, Is.EqualTo(1));
        Assert.That(reader.GetsBooks.Count, Is.EqualTo(0));

        for (int i = 0; i < 10; i++)
        {
            _readerService.GetBook(_bookService.GetBookById(i + 1), reader);
        }

        Assert.That(reader.GetsBooks.Count, Is.EqualTo(10));

        Assert.Throws<AlreadyGetBook>(() => _readerService.GetBook(_bookService.GetBookById(1), reader));
        Assert.Throws<MoreThanCan>(() => _readerService.GetBook(_bookService.GetBookById(12), reader));

        Reader reader2 = new Reader { Name = "Name2", Email = "test2@gmail.com" };
        _readerService.AddReader(reader2);

        Assert.Throws<NoMoreInstance>(() => _readerService.GetBook(_bookService.GetBookById(1), reader2));
    }

    [TestMethod]
    public void RemoveReaderById()
    {
        for (int i = 0; i < 100; i++)
        {
            _readerService.AddReader(new Reader { Name = "Name" + i, Email = i + "test@gmail.com" });
        }

        Assert.That(_readerService.GetAllReaders().Count, Is.EqualTo(100));

        for (int i = 0; i < 50; i++)
        {
            _readerService.DeleteReader(i + 1);
        }

        Assert.That(_readerService.GetAllReaders().Count, Is.EqualTo(50));
    }

    [TestMethod]
    public void GetAllReaders()
    {
        _readerService.AddReader(new Reader { Name = "Reader1", Email = "reader1@example.com" });
        _readerService.AddReader(new Reader { Name = "Reader2", Email = "reader2@example.com" });

        var result = _readerService.GetAllReaders();

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetReaderById()
    {
        var reader = new Reader { Name = "Reader1", Email = "reader1@example.com" };
        _readerService.AddReader(reader);

        var result = _readerService.GetReaderById(reader.Id);

        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(reader.Id));
    }

    [TestMethod]
    public void UpdateReader()
    {
        string newName = "Updated Name";
        string newEmail = "updatedemail@example.com";

        var reader = new Reader { Name = "Reader1", Email = "reader1@example.com" };
        _readerService.AddReader(reader);

        var updatedReader = _readerService.GetReaderById(reader.Id);
        updatedReader.Name = newName;
        updatedReader.Email = newEmail;

        _readerService.UpdateReader(updatedReader);

        var result = _readerService.GetReaderById(reader.Id);
        Assert.That(result.Name, Is.EqualTo(newName));
        Assert.That(result.Email, Is.EqualTo(newEmail));
    }

    [TestMethod]
    public void SearchReadersByName()
    {
        var name = "Reader1";
        var reader = new Reader { Name = name, Email = "reader1@example.com" };
        _readerService.AddReader(reader);

        List<Reader> readers = _readerService.SearchReadersByName(name);

        Assert.NotNull(readers);
        Assert.That(readers.Count, Is.EqualTo(1));
        Assert.That(readers[0].Name, Is.EqualTo(name));
    }

    [TestMethod]
    public void SearchReadersByEmail()
    {
        var email = "reader1@example.com";
        var reader = new Reader { Name = "Reader1", Email = email };
        _readerService.AddReader(reader);

        List<Reader> readers = _readerService.SearchReadersByEmail(email);

        Assert.NotNull(readers);
        Assert.That(readers.Count, Is.EqualTo(1));
        Assert.That(readers[0].Email, Is.EqualTo(email));
    }

    [TestMethod]
    public void GiveBackBook()
    {
        Book book = new Book { Title = "Book", Author = "Author", Topic = "Topic", Quantity = 1 };
        Reader reader = new Reader { Name = "Reader1", Email = "reader1@example.com" };

        _bookService.AddBook(book);
        _readerService.AddReader(reader);

        Assert.Throws<NotFoundBook>(() =>
            _readerService.GiveBackBook(_bookService.GetBookById(1), _readerService.GetReaderById(1)));

        _readerService.GetBook(_bookService.GetBookById(1), _readerService.GetReaderById(1));
        Assert.That(_readerService.GetReaderById(1).GetsBooks.Count, Is.EqualTo(1));

        _readerService.GiveBackBook(_bookService.GetBookById(1), _readerService.GetReaderById(1));
        Assert.That(_readerService.GetReaderById(1).GetsBooks.Count, Is.EqualTo(0));
    }
}