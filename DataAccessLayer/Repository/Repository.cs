using DataAccessLayer.Data;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DataAccessLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
       
        public Repository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<T>> GetAll(int pageNumber, int pageSize)
        {
            return await _context.Set<T>().ToPagedListAsync(pageNumber, pageSize);
        }
        
        public async Task<IEnumerable<T>> GetAllWithoutPagination()
        {
            return await _context.Set<T>().ToListAsync();
        }
        
        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        
        public async Task<T> Add(T entity)
        {
          await  _context.Set<T>().AddAsync(entity);

            return entity;
        }
        
        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);

            return entity;
        }
        
        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);

            return entity;
        }
    }
}