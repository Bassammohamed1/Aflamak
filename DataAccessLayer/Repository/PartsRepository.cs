using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DataAccessLayer.Repository
{
    public class PartsRepository : Repository<Part>, IPartsRepository
    {
        private readonly AppDbContext _context;

        public PartsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Part>> GetAllParts(int pageNumber, int pageSize)
        {
            return await _context.Parts.AsNoTracking().OrderBy(x => x.Name).Include(m => m.TvShow)
                .ThenInclude(m => m.ActorTvShows).ThenInclude(m => m.Actor)
                .AsSplitQuery()
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public IEnumerable<Part> GetAllParts()
        {
            return _context.Parts.AsNoTracking().Include(p => p.TvShow).ThenInclude(p => p.Producer)
                .Include(p => p.TvShow).ThenInclude(p => p.Category)
                .Include(p => p.TvShow).ThenInclude(p => p.Country)
                .Include(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor)
                .AsSplitQuery();
        }

        public IEnumerable<Part> GetAllPartsForSelectList()
        {
            return _context.Parts.AsNoTracking().OrderBy(x => x.Name);
        }

        public IQueryable<Part> GetFilteredPartsWithTvShowId(int id)
        {
            return _context.Parts.Where(p => p.TvShowId == id)
                .Include(p => p.TvShow).ThenInclude(p => p.Producer)
                .Include(p => p.TvShow).ThenInclude(p => p.Category)
                .Include(p => p.TvShow).ThenInclude(p => p.Country)
                .Include(p => p.TvShow).ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor)
                .AsSplitQuery();
        }
    }
}