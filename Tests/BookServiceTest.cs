using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace TestLab3;

[TestClass]
public class BookServiceTest
{
    private IBookService bookService;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        LibraryContext context = new LibraryContext(options);
        context.Database.EnsureDeleted();
        bookService = new BookService(new BookRepository(context));
    }

    [TestMethod]
    public void GetAllBooks()
    {
        bookService.AddBook(new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" });
        bookService.AddBook(new Book { Title = "Book2", Author = "Author2", Topic = "Topic2" });

        var result = bookService.GetAllBooks();

        Assert.IsNotNull(result);
        Assert.That(result.Count, Is.EqualTo(2));
    }

    [TestMethod]
    public void GetBookById()
    {
        var book = new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" };
        bookService.AddBook(book);

        var result = bookService.GetBookById(book.Id);

        Assert.IsNotNull(result);
        Assert.That(result.Id, Is.EqualTo(book.Id));
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
    public void UpdateBook()
    {
        var newTitle = "Updated title";
        var newAuthor = "Updated author";
        var newTopic = "Updated topic";

        Book book = new Book { Title = "Book1", Author = "Author1", Topic = "Topic1" };
        bookService.AddBook(book);

        var updatedBook = bookService.GetBookById(book.Id);
        Assert.IsNotNull(updatedBook);

        updatedBook.Title = newTitle;
        updatedBook.Author = newAuthor;
        updatedBook.Topic = newTopic;

        bookService.UpdateBook(updatedBook);
        var result = bookService.GetBookById(updatedBook.Id);

        Assert.That(result.Title, Is.EqualTo(newTitle));
        Assert.That(result.Author, Is.EqualTo(newAuthor));
        Assert.That(result.Topic, Is.EqualTo(newTopic));
    }
}