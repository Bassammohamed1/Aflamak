using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace DataAccessLayer.Repository
{
    public class ActorsRepository : Repository<Actor>, IActorsRepository
    {
        private readonly AppDbContext _context;
        public ActorsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable<Actor> GetActorsForSearch(string key)
        {
            var results = _context.Actors.Where(a => a.Name.Contains(key) || a.AnotherLangName.Contains(key));

            return results.Any() ? results : Enumerable.Empty<Actor>().AsQueryable();
        }
    }
}
