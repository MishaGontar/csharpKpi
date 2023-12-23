using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DLL2;

public class ReaderRepository(LibraryContext context) : IRepository<Reader>
{
    private readonly LibraryContext _context = context ?? throw new ArgumentNullException(nameof(context));

    public IQueryable<Reader> GetAll()
    {
        return _context.Readers?.AsQueryable() ?? throw new InvalidOperationException();
    }

    public Reader GetById(int id)
    {
        return _context.Readers?.Find(id) ?? throw new InvalidOperationException();
    }

    public void Add(Reader entity)
    {
        _context.Readers?.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Reader entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(Reader entity)
    {
        _context.Readers?.Remove(entity);
        _context.SaveChanges();
    }


    public List<Reader> Search(Expression<Func<Reader, bool>> predicate)
    {
        return (_context.Readers ?? throw new InvalidOperationException()).Where(predicate).ToList();
    }
}