using DLL2;

namespace BLL;

public class ReaderService(IRepository<Reader> readerRepository) : IReaderService
{
    private readonly IRepository<Reader> _readerRepository =
        readerRepository ?? throw new ArgumentNullException(nameof(readerRepository));

    public List<Reader> GetAllReaders()
    {
        return _readerRepository.GetAll().ToList();
    }

    public Reader GetReaderById(int id)
    {
        return _readerRepository.GetById(id);
    }

    public void AddReader(Reader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        _readerRepository.Add(reader);
    }

    public void UpdateReader(Reader reader)
    {
        ArgumentNullException.ThrowIfNull(reader);

        Reader updatedReader = _readerRepository.GetById(reader.Id);
        ArgumentNullException.ThrowIfNull(updatedReader);

        updatedReader.Name = reader.Name;
        updatedReader.Email = reader.Email;
        _readerRepository.Update(updatedReader);
    }

    public void DeleteReader(int id)
    {
        var readerToDelete = _readerRepository.GetById(id);
        _readerRepository.Delete(readerToDelete);
    }

    public List<Reader> SearchReadersByName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        return _readerRepository.Search(r => r.Name.Contains(name)).ToList();
    }

    public List<Reader> SearchReadersByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        return _readerRepository.Search(r => r.Email.Contains(email)).ToList();
    }


    public void GetBook(Book book, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(book);
        ArgumentNullException.ThrowIfNull(reader);

        bool isAlreadyExist = reader.GetsBooks.Any(b =>
            b.Title.Equals(book.Title) && b.Author.Equals(book.Author) && b.Topic.Equals(book.Topic));

        if (isAlreadyExist)
            throw new AlreadyGetBook();
        if (reader.GetsBooks.Count == 10)
            throw new MoreThanCan();
        if (book.Quantity == 0)
            throw new NoMoreInstance();

        reader.GetsBooks.Add(book);
        book.Quantity -= 1;
        Console.WriteLine("Книжку було додано до формулятора.");
    }

    public void GiveBackBook(Book book, Reader reader)
    {
        ArgumentNullException.ThrowIfNull(book);
        ArgumentNullException.ThrowIfNull(reader);

        var foundBook = reader.GetsBooks.Find(b =>
            b.Title.Equals(book.Title) && b.Author.Equals(book.Author) && b.Topic.Equals(book.Topic));
        if (foundBook == null)
            throw new NotFoundBook();

        reader.GetsBooks.Remove(book);
        book.Quantity += 1;
        Console.WriteLine("Книжку було видалено з формулятора.");
    }
}