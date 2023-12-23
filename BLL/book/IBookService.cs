using DLL2;

namespace BLL;

public interface IBookService
{
    List<Book> GetAllBooks();
    Book GetBookById(int id);
    void AddBook(Book book);
    void UpdateBook(Book book);
    void DeleteBook(int id);
    List<Book> SearchBooksByTitle(string title);
    List<Book> SearchBooksByAuthor(string author);
    List<Book> SearchBooksByTopic(string topic);
}