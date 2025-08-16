using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IActorsRepository : IRepository<Actor>
    {
        IQueryable<Actor> GetActorsForSearch(string key);
    }
}
