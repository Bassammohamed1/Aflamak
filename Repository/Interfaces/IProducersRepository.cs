using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IProducersRepository : IRepository<Producer>
    {
        IQueryable<Producer> GetProducersForSearch(string key);
    }
}
