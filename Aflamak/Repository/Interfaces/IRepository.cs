using Aflamak.Models;
using System.Linq.Expressions;

namespace Aflamak.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll(int pageNumber, int pageSize);
        IEnumerable<T> GetAllWithoutPagination();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}