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

        public List<Actor> GetActor(string key)
        {
            var data = new List<Actor>();
            foreach (var actor in _context.Actors)
            {
                if (actor.Name.Contains(key) || actor.AnotherLangName.Contains(key))
                    data.Add(actor);
            }
            return data;
        }
    }
}
