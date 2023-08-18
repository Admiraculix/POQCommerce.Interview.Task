using System.Linq.Expressions;

namespace Persistance.Abstractions.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}