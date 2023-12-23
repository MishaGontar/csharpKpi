using BLL;
using DLL2;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReaderApiController(IReaderService readerService, IBookService bookService) : ControllerBase
{
    private readonly IReaderService _readerService =
        readerService ?? throw new ArgumentNullException(nameof(readerService));

    private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

    [HttpGet]
    public ActionResult<IEnumerable<Reader>> GetAllReaders()
    {
        List<Reader> allReaders = _readerService.GetAllReaders();
        return Ok(allReaders);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Reader> GetReaderById(int id)
    {
        Reader reader = _readerService.GetReaderById(id);
        if (reader == null)
        {
            return NotFound();
        }

        return Ok(reader);
    }

    [HttpPost]
    public ActionResult CreateReader(Reader reader)
    {
        try
        {
            _readerService.AddReader(reader);
            return CreatedAtAction(nameof(GetReaderById), new { id = reader.Id }, reader);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateReader(int id, Reader reader)
    {
        if (id != reader.Id)
            return BadRequest();

        try
        {
            _readerService.UpdateReader(reader);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteReader(int id)
    {
        Reader reader = _readerService.GetReaderById(id);
        if (reader == null)
            return NotFound();

        _readerService.DeleteReader(id);
        return NoContent();
    }

    [HttpGet("searchByName")]
    public ActionResult<IEnumerable<Reader>> SearchReadersByName(string name)
    {
        List<Reader> foundReaders = string.IsNullOrWhiteSpace(name) ? [] : _readerService.SearchReadersByName(name);
        return Ok(foundReaders);
    }

    [HttpGet("searchByEmail")]
    public ActionResult<IEnumerable<Reader>> SearchReadersByEmail(string email)
    {
        List<Reader> foundReaders = string.IsNullOrWhiteSpace(email) ? [] : _readerService.SearchReadersByEmail(email);
        return Ok(foundReaders);
    }

    [HttpPost("getBook")]
    public ActionResult GetBook(int readerId, int bookId)
    {
        Reader reader = _readerService.GetReaderById(readerId);
        Book book = _bookService.GetBookById(bookId);

        _readerService.GetBook(book, reader);

        return NoContent();
    }

    [HttpPost("giveBackBook")]
    public ActionResult GiveBackBook(int readerId, int bookId)
    {
        Reader reader = _readerService.GetReaderById(readerId);
        Book book = _bookService.GetBookById(bookId);

        _readerService.GiveBackBook(book, reader);

        return NoContent();
    }
}