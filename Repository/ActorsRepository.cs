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

        public Actor GetActor(string key)
        {
            var actor = _context.Actors.SingleOrDefault(x => x.Name.ToLower() == key.ToLower());
            if (actor == null)
            {
                actor = _context.Actors.SingleOrDefault(x => x.AnotherLangName.ToLower() == key.ToLower());
            }
            return actor;
        }
    }
}
