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

        public Producer GetProducer(string key)
        {
            var producer = _context.Producers.SingleOrDefault(x => x.Name.ToLower() == key.ToLower());
            if (producer == null)
            {
                producer = _context.Producers.SingleOrDefault(x => x.AnotherLangName.ToLower() == key.ToLower());
            }
            return producer;
        }
    }
}
