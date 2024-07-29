using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Repository.Interfaces;

namespace Aflamak.Repository
{
    public class ProducersRepository : Repository<Producer>, IProducersRepository
    {
        private readonly AppDbContext _context;

        public ProducersRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Producer> GetProducer(string key)
        {
            var data = new List<Producer>();
            foreach (var producer in _context.Producers)
            {
                if (producer.Name.Contains(key) || producer.AnotherLangName.Contains(key))
                    data.Add(producer);
            }
            return data;
        }
    }
}
