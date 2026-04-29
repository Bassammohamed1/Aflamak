
namespace DataAccessLayer.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(int pageNumber, int pageSize);
        Task<IEnumerable<T>> GetAllWithoutPagination();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}