using BLL;
using DLL2;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

public class ReaderController(IReaderService readerService, IBookService bookService) : Controller
{
    private readonly IReaderService _readerService =
        readerService ?? throw new ArgumentNullException(nameof(readerService));

    private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

    public ActionResult Index()
    {
        List<Reader> allReaders = _readerService.GetAllReaders();
        return View(allReaders);
    }

    public ActionResult Details(int id)
    {
        Reader reader = _readerService.GetReaderById(id);
        return View(reader);
    }

    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public ActionResult Create(Reader reader)
    {
        try
        {
            _readerService.AddReader(reader);
            return RedirectToAction("Index");
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Edit(int id)
    {
        Reader reader = _readerService.GetReaderById(id);
        return View(reader);
    }

    [HttpPost]
    public ActionResult Edit(Reader reader)
    {
        _readerService.UpdateReader(reader);
        return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
        Reader reader = _readerService.GetReaderById(id);
        return View(reader);
    }

    public ActionResult SearchByName(string name)
    {
        List<Reader> foundReaders = string.IsNullOrWhiteSpace(name) ? [] : _readerService.SearchReadersByName(name);
        return View(foundReaders);
    }

    public ActionResult SearchByEmail(string email)
    {
        List<Reader> foundReaders = string.IsNullOrWhiteSpace(email) ? [] : _readerService.SearchReadersByEmail(email);
        return View(foundReaders);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _readerService.DeleteReader(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult GetBook(int readerId, int bookId)
    {
        Reader reader = _readerService.GetReaderById(readerId);
        Book book = _bookService.GetBookById(bookId);

        _readerService.GetBook(book, reader);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult GiveBackBook(int readerId, int bookId)
    {
        Reader reader = _readerService.GetReaderById(readerId);
        Book book = _bookService.GetBookById(bookId);

        _readerService.GiveBackBook(book, reader);

        return RedirectToAction("Index");
    }
}