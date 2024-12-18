using Aflamak.Data;
using Aflamak.Repository.Interfaces;
using X.PagedList;

namespace Aflamak.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll(int pageNumber, int pageSize)
        {
            var result = _context.Set<T>().ToPagedList(pageNumber, pageSize);

            return result.Any() ? result : Enumerable.Empty<T>();
        }
        public IEnumerable<T> GetAllWithoutPagination()
        {
            var result = _context.Set<T>().ToList();

            return result.Any() ? result : Enumerable.Empty<T>();
        }
        public T GetById(int id)
        {
            var result = _context.Set<T>().Find(id);

            return result is not null ? result : null;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}