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
        public IQueryable<Producer> GetProducersForSearch(string key)
        {
            var results = _context.Producers.Where(a => a.Name.Contains(key) || a.AnotherLangName.Contains(key));

            return results.Any() ? results : Enumerable.Empty<Producer>().AsQueryable();
        }
    }
}
