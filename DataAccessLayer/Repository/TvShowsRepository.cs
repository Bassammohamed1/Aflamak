using DataAccessLayer.Data;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DataAccessLayer.Repository
{
    public class TvShowsRepository : Repository<TvShow>, ITvShowsRepository
    {
        private readonly AppDbContext _context;

        public TvShowsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TvShow?> GetTvShowById(int id)
        {
            return await _context.TvShows.AsNoTracking().Include(x => x.Producer).Include(x => x.Category)
                .Include(x => x.Country).Include(y => y.ActorTvShows).ThenInclude(z => z.Actor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IEnumerable<TvShow> GetTvShows()
        {
            return _context.TvShows.AsNoTracking().OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows)
                .ThenInclude(m => m.Actor)
                .AsSplitQuery();
        }

        public async Task<IEnumerable<TvShow>> GetAllTvShows(int pageNumber, int pageSize)
        {
            return await _context.TvShows.AsNoTracking().OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category)
                .Include(m => m.Country).Include(x => x.ActorTvShows)
                .ThenInclude(m => m.Actor)
                .AsSplitQuery()
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<TvShow>> GetAllTvShowsOrderedByKey(int pageNumber, int pageSize, Keys key)
        {
            return await _context.TvShows.AsNoTracking().OrderByDescending(f => EF.Property<object>(f, key.ToString()))
                .Include(m => m.Producer).Include(m => m.Category)
                .Include(m => m.Country).Include(x => x.ActorTvShows)
                .ThenInclude(m => m.Actor)
                .AsSplitQuery()
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public IQueryable<TvShow> GetFilteredTvShowsWithID(int id)
        {
            return _context.TvShows.AsNoTracking().Where(f => f.Id == id)
                .OrderBy(f => f.Name).Include(m => m.Producer)
                .Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows)
                .ThenInclude(m => m.Actor)
                .AsSplitQuery();
        }

        public IQueryable<TvShow> GetFilteredTvShowsWithProducerID(int id)
        {
            return _context.TvShows.AsNoTracking().Where(f => f.ProducerId == id).OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country)
                .Include(x => x.ActorTvShows).ThenInclude(m => m.Actor)
                .AsSplitQuery();
        }

        public async Task<TvShow?> AddTvShow(TvShow data, List<int> actorsIDs)
        {
            await _context.TvShows.AddAsync(data);
            await _context.SaveChangesAsync();

            foreach (var i in actorsIDs)
            {
                var result = new ActorTvShows()
                {
                    TvShowId = data.Id,
                    ActorId = i
                };
                await _context.ActorTvShows.AddAsync(result);
            }

            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<TvShow?> UpdateTvShow(TvShow data, List<int> actorsIDs)
        {
            var tvShow = await _context.TvShows.FindAsync(data.Id);

            tvShow.Name = data.Name;
            tvShow.Description = data.Description;
            tvShow.dbImage = data.dbImage;
            tvShow.IsSeries = data.IsSeries;
            tvShow.IsRamadan = data.IsRamadan;
            tvShow.PartsNo = data.PartsNo;
            tvShow.NoOfLikes = data.NoOfLikes;
            tvShow.Year = data.Year;
            tvShow.Month = data.Month;
            tvShow.Type = data.Type;
            tvShow.Language = data.Language;
            tvShow.CountryId = data.CountryId;
            tvShow.ProducerId = data.ProducerId;
            tvShow.CategoryId = data.CategoryId;

            await _context.SaveChangesAsync();

            var existingActorTvShows = await _context.ActorTvShows.Where(af => af.TvShowId == tvShow.Id).ToListAsync();

            _context.ActorTvShows.RemoveRange(existingActorTvShows);
            await _context.SaveChangesAsync();

            foreach (var i in actorsIDs)
            {
                var result = new ActorTvShows()
                {
                    TvShowId = tvShow.Id,
                    ActorId = i
                };
                await _context.ActorTvShows.AddAsync(result);
            }

            await _context.SaveChangesAsync();

            return data;
        }

        public IEnumerable<TvShow> GetAllTvShowsForSelectList()
        {
            return _context.TvShows.AsNoTracking().OrderBy(x => x.Name);
        }
    }
}