using Microsoft.EntityFrameworkCore;

namespace isgasoir
{
    public class BaseRepository<T, K> : IBaseRepository<T, K> where T : class
    {
        public readonly ApplicationContext _applicationContext;
        public readonly DbSet<T> _entities;
        public BaseRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _entities= _applicationContext.Set<T>();
           
        }
        public T add(T entity)
        {
           _entities.Add(entity);
            return entity;
        }

        public List<T> findAll()
        {
           return  _entities.ToList();
        }

        public IEnumerable<T> findByCretiria(System.Linq.Expressions.Expression<Func<T, bool>> critiria)
        {
           // Let EF translate the expression instead of compiling to avoid client evaluation
           return _entities.Where(critiria).AsEnumerable();

        }

        public T? findById(K key)
        {
            return _entities.Find(key);
        }

        public void remove(T entity)
        {
            _entities.Remove(entity);
        }

        public T update(T entity)
        {
            _entities.Update(entity);
            return entity;
        }

       public  IQueryable<T> Query => _entities;

    }
}
