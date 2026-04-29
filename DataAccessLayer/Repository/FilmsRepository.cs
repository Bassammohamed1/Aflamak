using DataAccessLayer.Data;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace DataAccessLayer.Repository
{
    public class FilmsRepository : Repository<Film>, IFilmsRepository
    {
        private readonly AppDbContext _context;

        public FilmsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Film> GetFilmById(int id)
        {
            return await _context.Films.AsNoTracking().Include(x => x.Producer).Include(x => x.Category).Include(x => x.Country)
                .Include(y => y.ActorFilms).ThenInclude(z => z.Actor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IEnumerable<Film> GetFilms()
        {
            return _context.Films.AsNoTracking().OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms)
                .ThenInclude(m => m.Actor);
        }

        public async Task<IEnumerable<Film>> GetAllFilms(int pageNumber, int pageSize)
        {
            return await _context.Films.AsNoTracking().OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category)
                .Include(m => m.Country).Include(x => x.ActorFilms)
                .ThenInclude(m => m.Actor).ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Film>> GetAllFilmsOrderedByKey(int pageNumber, int pageSize, Keys key)
        {
            return await _context.Films.AsNoTracking().OrderByDescending(f => EF.Property<object>(f, key.ToString()))
                .Include(m => m.Producer).Include(m => m.Category)
                .Include(m => m.Country).Include(x => x.ActorFilms)
                .ThenInclude(m => m.Actor).ToPagedListAsync(pageNumber, pageSize);
        }

        public IQueryable<Film> GetFilteredFilmsWithID(int id)
        {
            return _context.Films.AsNoTracking().Where(f => f.Id == id)
                .OrderBy(f => f.Name).Include(m => m.Producer)
                .Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms)
                .ThenInclude(m => m.Actor);
        }

        public IQueryable<Film> GetFilteredFilmsWithProducerID(int id)
        {
            return _context.Films.AsNoTracking().Where(f => f.ProducerId == id)
                .OrderBy(f => f.Name)
                .Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country)
                .Include(x => x.ActorFilms).ThenInclude(m => m.Actor);
        }

        public async Task<Film?> AddFilm(Film data, List<int> actorsIDs)
        {
            await _context.Films.AddAsync(data);
            await _context.SaveChangesAsync();

            foreach (var i in actorsIDs)
            {
                var result = new ActorFilms()
                {
                    FilmId = data.Id,
                    ActorId = i
                };
                await _context.ActorFilms.AddAsync(result);
            }

            await _context.SaveChangesAsync();

            return data;
        }

        public async Task<Film?> UpdateFilm(Film data, List<int> actorsIDs)
        {
            var movie = await _context.Films.FindAsync(data.Id);

            movie.Name = data.Name;
            movie.Description = data.Description;
            movie.dbImage = data.dbImage;
            movie.IsSeries = data.IsSeries;
            movie.PartsNo = data.PartsNo;
            movie.Root = data.Root;
            movie.NoOfLikes = data.NoOfLikes;
            movie.NoOfDisLikes = data.NoOfDisLikes;
            movie.Year = data.Year;
            movie.Month = data.Month;
            movie.Type = data.Type;
            movie.Language = data.Language;
            movie.CountryId = data.CountryId;
            movie.ProducerId = data.ProducerId;
            movie.CategoryId = data.CategoryId;

            await _context.SaveChangesAsync();

            var existingActorFilms = await _context.ActorFilms.Where(af => af.FilmId == movie.Id).ToListAsync();

            _context.ActorFilms.RemoveRange(existingActorFilms);
            await _context.SaveChangesAsync();

            foreach (var i in actorsIDs)
            {
                var result = new ActorFilms()
                {
                    FilmId = movie.Id,
                    ActorId = i
                };
                await _context.ActorFilms.AddAsync(result);
            }

            await _context.SaveChangesAsync();
            return data;
        }
    }
}