using BLL;
using DLL2;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

public class BookController : Controller
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService)
    {
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }

    public ActionResult Index()
    {
        List<Book> allBooks = _bookService.GetAllBooks();
        return View(allBooks);
    }

    public ActionResult Details(int id)
    {
        Book book = _bookService.GetBookById(id);
        return View(book);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Book book)
    {
        try
        {
            _bookService.AddBook(book);
            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Edit(int id)
    {
        Book book = _bookService.GetBookById(id);
        return View(book);
    }

    [HttpPost]
    public ActionResult Edit(Book book)
    {
        _bookService.UpdateBook(book);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        try
        {
            Book book = _bookService.GetBookById(id);
            return View(book);
        }
        catch (Exception e)
        {
            Console.Write(e);
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _bookService.DeleteBook(id);
        return RedirectToAction("Index");
    }

    public ActionResult SearchByTitle(string title)
    {
        List<Book> foundBooks = string.IsNullOrWhiteSpace(title) ? [] : _bookService.SearchBooksByTitle(title);
        return View(foundBooks);
    }

    public ActionResult SearchByAuthor(string author)
    {
        List<Book> foundBooks = string.IsNullOrWhiteSpace(author) ? [] : _bookService.SearchBooksByAuthor(author);
        return View(foundBooks);
    }

    public ActionResult SearchByTopic(string topic)
    {
        List<Book> foundBooks = string.IsNullOrWhiteSpace(topic) ? [] : _bookService.SearchBooksByTopic(topic);
        return View(foundBooks);
    }

    public IActionResult BorrowBook(int id)
    {
        Book book = _bookService.GetBookById(id);
        return View(book);
    }
}