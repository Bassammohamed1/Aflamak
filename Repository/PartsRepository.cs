using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Aflamak.Repository
{
    public class PartsRepository : Repository<Part>, IPartsRepository
    {
        private readonly AppDbContext _context;

        public PartsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Part> GetAllParts(int pageNumber, int pageSize)
        {
            return _context.Parts.OrderBy(x => x.Name).Include(m => m.TvShow).ThenInclude(m => m.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);

        }

        public IEnumerable<Part> GetAllParts()
        {
            return _context.Parts.Include(p => p.TvShow).ThenInclude(p => p.Producer).Include(p => p.TvShow).ThenInclude(p => p.Category).Include(p => p.TvShow).ThenInclude(p => p.Country).Include(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor);
        }

        public IEnumerable<Part> GetAllPartsForSelectList()
        {
            return _context.Parts.OrderBy(x => x.Name);
        }

        public IQueryable<Part> GetFilteredPartsWithTvShowId(int id)
        {
            return _context.Parts.Where(p => p.TvShowId == id).Include(p => p.TvShow).ThenInclude(p => p.Producer).Include(p => p.TvShow).ThenInclude(p => p.Category).Include(p => p.TvShow).ThenInclude(p => p.Country).Include(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor);
        }
    }
}
