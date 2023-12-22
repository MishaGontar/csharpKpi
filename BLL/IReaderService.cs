using DLL2;

namespace BLL;

public interface IReaderService
{
    List<Reader> GetAllReaders();
    Reader GetReaderById(int id);
    void AddReader(Reader reader);
    void UpdateReader(Reader reader);
    void DeleteReader(int id);
    List<Reader> SearchReadersByName(string name);
    List<Reader> SearchReadersByEmail(string email);
    void getBook(Book book, Reader reader);
}