using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IActorsRepository : IRepository<Actor>
    {
        List<Actor> GetActor(string key);
    }
}
