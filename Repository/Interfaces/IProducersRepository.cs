using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IProducersRepository : IRepository<Producer>
    {
        Producer GetProducer(string key);
    }
}
