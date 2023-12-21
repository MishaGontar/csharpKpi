using System.Linq.Expressions;

namespace DLL2;

public interface IRepository<T>
{
    IQueryable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    List<T> Search(Expression<Func<T, bool>> predicate);
}