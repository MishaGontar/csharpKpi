using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DLL2;

public class BookRepository(LibraryContext context) : IRepository<Book>
{
    private readonly LibraryContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IQueryable<Book> GetAll()
    {
        return _context.Books?.AsQueryable() ?? throw new InvalidOperationException();
    }

    public Book GetById(int id)
    {
        return _context.Books?.Find(id) ?? throw new InvalidOperationException();
    }

    public void Add(Book entity)
    {
        _context.Books?.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Book entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(Book entity)
    {
        _context.Books?.Remove(entity);
        _context.SaveChanges();
    }

    public List<Book> Search(Expression<Func<Book, bool>> predicate)
    {
        return (_context.Books ?? throw new InvalidOperationException()).Where(predicate).ToList();
    }
}