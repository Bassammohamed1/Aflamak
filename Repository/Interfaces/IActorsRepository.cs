using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IActorsRepository : IRepository<Actor>
    { 
        Actor GetActor(string key);
    }
}
