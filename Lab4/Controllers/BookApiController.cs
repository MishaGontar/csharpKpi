using BLL;
using DLL2;
using Microsoft.AspNetCore.Mvc;

namespace Lab4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookApiController(IBookService bookService) : ControllerBase
{
    private readonly IBookService _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));

    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetAllBooks()
    {
        List<Book> allBooks = _bookService.GetAllBooks();
        return Ok(allBooks);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Book> GetBookById(int id)
    {
        try
        {
            Book book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    public ActionResult CreateBook([FromBody] Book book)
    {
        try
        {
            _bookService.AddBook(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult UpdateBook(int id, [FromBody] Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        try
        {
            _bookService.UpdateBook(book);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteBook(int id)
    {
        try
        {
            Book book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            _bookService.DeleteBook(id);
            return NoContent();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet("SearchByTitle")]
    public ActionResult<IEnumerable<Book>> SearchByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return BadRequest("Title cannot be empty");
        }

        List<Book> foundBooks = _bookService.SearchBooksByTitle(title);
        return Ok(foundBooks);
    }

    [HttpGet("SearchByAuthor")]
    public ActionResult<IEnumerable<Book>> SearchByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            return BadRequest("Author cannot be empty");
        }

        List<Book> foundBooks = _bookService.SearchBooksByAuthor(author);
        return Ok(foundBooks);
    }

    [HttpGet("SearchByTopic")]
    public ActionResult<IEnumerable<Book>> SearchByTopic(string topic)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            return BadRequest("Topic cannot be empty");
        }

        List<Book> foundBooks = _bookService.SearchBooksByTopic(topic);
        return Ok(foundBooks);
    }
}