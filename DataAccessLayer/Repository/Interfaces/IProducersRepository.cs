using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IProducersRepository : IRepository<Producer>
    {
        IQueryable<Producer> GetProducersForSearch(string key);
    }
}
