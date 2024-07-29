using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IProducersRepository : IRepository<Producer>
    {
        List<Producer> GetProducer(string key);
    }
}
