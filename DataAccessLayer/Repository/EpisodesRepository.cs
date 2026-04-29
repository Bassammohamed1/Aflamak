using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DataAccessLayer.Repository
{
    public class EpisodesRepository : Repository<Episode>, IEpisodesRepository
    {
        private readonly AppDbContext _context;

        public EpisodesRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Episode>> GetAllEpisodes(int pageNumber, int pageSize)
        {
            return await _context.Episodes.AsNoTracking().OrderBy(x => x.Part.Name)
                .Include(m => m.Part).ThenInclude(m => m.TvShow)
                .ThenInclude(m => m.ActorTvShows).ThenInclude(m => m.Actor)
                .AsSplitQuery()
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public IEnumerable<Episode> GetAllEpisodes()
        {
            return _context.Episodes.AsNoTracking().Include(e => e.Part)
                .ThenInclude(p => p.TvShow).ThenInclude(t => t.Producer)
                .Include(e => e.Part).ThenInclude(p => p.TvShow)
                .ThenInclude(p => p.Category).Include(e => e.Part)
                .ThenInclude(p => p.TvShow).ThenInclude(p => p.Country)
                .Include(e => e.Part).ThenInclude(p => p.TvShow)
                .ThenInclude(p => p.ActorTvShows).ThenInclude(p => p.Actor)
                .AsSplitQuery();
        }

        public IQueryable<Episode> GetFilteredEpisodesWithPartId(int id)
        {
            return _context.Episodes.AsNoTracking().Where(e => e.PartId == id)
                .Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(t => t.Producer)
                .Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Category)
                .Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.Country)
                .Include(e => e.Part).ThenInclude(p => p.TvShow).ThenInclude(p => p.ActorTvShows)
                .ThenInclude(p => p.Actor)
                .AsSplitQuery();
        }
    }
}