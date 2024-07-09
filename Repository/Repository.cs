using Aflamak.Data;
using Aflamak.Models;
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
            return _context.Set<T>().ToPagedList(pageNumber, pageSize);
        }
        public IEnumerable<T> GetAllWithoutPagination()
        {
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
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
