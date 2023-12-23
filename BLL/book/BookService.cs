using DLL2;

namespace BLL;

public class BookService(IRepository<Book> bookRepository) : IBookService
{
    private readonly IRepository<Book> _bookRepository =
        bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));

    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAll().ToList();
    }

    public Book GetBookById(int id)
    {
        return _bookRepository.GetById(id);
    }

    public void AddBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        _bookRepository.Add(book);
    }

    public void UpdateBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        _bookRepository.Update(book);
    }

    public void DeleteBook(int id)
    {
        var bookToDelete = _bookRepository.GetById(id);
        _bookRepository.Delete(bookToDelete);
    }

    public List<Book> SearchBooksByTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Title cannot be null or empty.", nameof(title));
        }

        return _bookRepository.Search(b => b.Title.Contains(title)).ToList();
    }

    public List<Book> SearchBooksByAuthor(string author)
    {
        if (string.IsNullOrWhiteSpace(author))
        {
            throw new ArgumentException("Author cannot be null or empty.", nameof(author));
        }

        return _bookRepository.Search(b => b.Author.Contains(author)).ToList();
    }

    public List<Book> SearchBooksByTopic(string topic)
    {
        if (string.IsNullOrWhiteSpace(topic))
        {
            throw new ArgumentException("Topic cannot be null or empty.", nameof(topic));
        }

        return _bookRepository.Search(b => b.Topic.Contains(topic)).ToList();
    }
}