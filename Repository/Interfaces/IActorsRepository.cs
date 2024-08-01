using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IActorsRepository : IRepository<Actor>
    {
        IQueryable<Actor> GetActorsForSearch(string key);
    }
}
