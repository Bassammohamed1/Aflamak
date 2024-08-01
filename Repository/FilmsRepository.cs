using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Models.ViewModels;
using Aflamak.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Aflamak.Repository
{
    public class FilmsRepository : Repository<Film>, IFilmsRepository
    {
        private readonly AppDbContext _context;
        public FilmsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Film> GetFilms()
        {
            return _context.Films.OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor);
        }
        public IQueryable<Film> GetFilteredFilmsWithId(int id, string Key)
        {
            if (Key == "ID")
                return _context.Films.Where(f => f.Id == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor);

            else if (Key == "Producer")
                return _context.Films.Where(f => f.ProducerId == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor);

            else
                return null;
        }
        public IQueryable<Film> GetFilteredFilms(string genre, string country, int? language, int? year, bool isArabic = false, bool isCartoon = false)
        {
            if (isArabic && !isCartoon)
            {
                var query = this.ArabicFilms().AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var films = query;

                return films;
            }
            else if (isCartoon && !isArabic)
            {
                var query = this.CartoonFilms().AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var films = query;

                return films;
            }
            else
            {
                var query = this.GetFilms().AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var films = query;

                return films;
            }
        }
        public IEnumerable<Film> GetAllFilms(int pageNumber, int pageSize)
        {
            return _context.Films.OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public IEnumerable<Film> GetAllFilmsOrderedByDate(int pageNumber, int pageSize)
        {
            return _context.Films.OrderByDescending(f => f.Year).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public IEnumerable<Film> GetAllFilmsOrderedByLikes(int pageNumber, int pageSize)
        {
            return _context.Films.OrderByDescending(f => f.NoOfLikes).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorFilms).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public Film GetFilmById(int id)
        {
            return _context.Films.Include(x => x.Producer).Include(x => x.Category).Include(x => x.Country).Include(y => y.ActorFilms).ThenInclude(z => z.Actor).FirstOrDefault(s => s.Id == id);
        }
        public void AddFilm(FilmViewModel data)
        {
            int languageId = (int)data.Language;
            int typeId = (int)data.Type;

            var film = new Film()
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                dbImage = data.dbImage,
                IsSeries = data.IsSeries,
                PartsNo = data.PartsNo,
                Root = data.Root,
                NoOfLikes = data.NoOfLikes,
                NoOfDisLikes = data.NoOfDisLikes,
                Year = data.Year,
                Month = data.Month,
                Type = typeId,
                Language = languageId,
                CountryId = data.CountryId,
                CategoryId = data.CategoryId,
                ProducerId = data.ProducerId,
            };
            _context.Films.Add(film);
            _context.SaveChanges();
            foreach (var i in data.ActorsId)
            {
                var result = new ActorFilms()
                {
                    FilmId = film.Id,
                    ActorId = i
                };
                _context.ActorFilms.Add(result);
            }
            _context.SaveChanges();
        }
        public void UpdateFilm(FilmViewModel data)
        {
            int languageId = (int)data.Language;
            int typeId = (int)data.Type;

            var movie = _context.Films.Find(data.Id);
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
            movie.Type = typeId;
            movie.Language = languageId;
            movie.CountryId = data.CountryId;
            movie.ProducerId = data.ProducerId;
            movie.CategoryId = data.CategoryId;
            _context.SaveChanges();
            foreach (var i in data.ActorsId)
            {
                var result = new ActorFilms()
                {
                    FilmId = movie.Id,
                    ActorId = i
                };
                _context.ActorFilms.Update(result);
            }
            _context.SaveChanges();
        }
        public IQueryable<Film> MostWatchedFilms()
        {
            return _context.Films.OrderByDescending(x => x.NoOfLikes).Where(m => m.Type == 1).AsNoTracking();
        }
        public IQueryable<Film> RecentFilms()
        {
            return _context.Films.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).Where(m => m.Type == 1).AsNoTracking();
        }
        public IQueryable<Film> ArabicFilms()
        {
            return _context.Films.Where(m => m.Type == 1 && m.Language == 1).AsNoTracking();
        }
        public IQueryable<Film> CartoonFilms()
        {
            return _context.Films.OrderByDescending(x => x.NoOfLikes).Where(m => m.Type == 3).AsNoTracking();
        }
        public IQueryable<Film> GetRecentFilms(int page)
        {
            List<Film> data = new List<Film>();

            var result1 = _context.Films.OrderBy(f => f.Name).Where(f => f.Month < 8 && f.Year == DateTime.Now.Year).ToList();
            if (result1 is not null)
                data.AddRange(result1);

            var result2 = _context.Films.OrderBy(f => f.Name).Where(f => f.Month >= 8 && (f.Year == DateTime.Now.Year || f.Year == DateTime.Now.Year - 1)).ToList();
            if (result2 is not null)
                data.AddRange(result2);

            return data.AsQueryable();
        }
        public IQueryable<Film> GetTopRatedFilms(int page)
        {
            List<Film> data = new List<Film>();

            var result1 = _context.Films.OrderBy(f => f.Name).Where(f => f.NoOfLikes > 5000).ToList();
            if (result1 is not null)
                data.AddRange(result1);

            return data.AsQueryable();
        }
        public IQueryable<Film> GetFilmsForSearch(string key)
        {
            return _context.Films.Where(a => a.Name.Contains(key));
        }
    }
}