using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Repository.Interfaces;

namespace Aflamak.Repository
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
            return _context.Actors.Where(a => a.Name.Contains(key) || a.AnotherLangName.Contains(key));
        }
    }
}
