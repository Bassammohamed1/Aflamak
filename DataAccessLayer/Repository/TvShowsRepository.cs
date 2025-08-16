using DataAccessLayer.Data;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
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
        public IEnumerable<TvShow> GetTvShows()
        {
            var result = _context.TvShows.OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }
        public IQueryable<TvShow> GetFilteredTvShowsWithId(int id, string Key)
        {
            if (Key == "ID")
                return _context.TvShows.Where(t => t.Id == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);

            else if (Key == "Producer")
                return _context.TvShows.Where(t => t.ProducerId == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);

            else
                return Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> GetFilteredTvShows(string genre, string country, int? language, int? year, bool isArabic = false, bool isRamadan = false)
        {
            if (isArabic && !isRamadan)
            {
                var query = ArabicTvShows().AsQueryable();

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

                var tvshows = query;

                return tvshows;
            }
            else if (isRamadan && !isArabic)
            {
                var query = RamadanTvShows().AsQueryable();

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

                var tvshows = query;

                return tvshows;
            }
            else
            {
                var query = GetTvShows().AsQueryable();

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

                var tvshows = query;

                return tvshows;
            }
        }
        public IEnumerable<TvShow> GetAllTvShows(int pageNumber, int pageSize)
        {
            var result = _context.TvShows.OrderBy(t => t.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }
        public IEnumerable<TvShow> GetAllTvShowsOrderedByDate(int pageNumber, int pageSize)
        {
            var result = _context.TvShows.OrderByDescending(t => t.Year).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }
        public IEnumerable<TvShow> GetAllTvShowsOrderedByLikes(int pageNumber, int pageSize)
        {
            var result = _context.TvShows.OrderByDescending(t => t.NoOfLikes).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }
        public TvShow GetTvShowById(int id)
        {
            var result = _context.TvShows.Include(x => x.Producer).Include(x => x.Category).Include(x => x.Country).Include(y => y.ActorTvShows).ThenInclude(z => z.Actor).FirstOrDefault(s => s.Id == id);

            return result is not null ? result : null;
        }
        public IEnumerable<TvShow> GetAllTvShowsForSelectList()
        {
            var result = _context.TvShows.OrderBy(x => x.Name);

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }
        public void AddTvShow(TvShowViewModel data)
        {
            int languageId = (int)data.Language;
            int typeId = (int)data.Type;

            var tvshow = new TvShow()
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                dbImage = data.dbImage,
                IsSeries = data.IsSeries,
                IsRamadan = data.IsRamadan,
                PartsNo = data.PartsNo,
                NoOfLikes = data.NoOfLikes,
                Year = data.Year,
                Month = data.Month,
                Type = typeId,
                Language = languageId,
                CountryId = data.CountryId,
                CategoryId = data.CategoryId,
                ProducerId = data.ProducerId,
            };
            _context.TvShows.Add(tvshow);
            _context.SaveChanges();
            foreach (var i in data.ActorsId)
            {
                var result = new ActorTvShows()
                {
                    TvShowId = tvshow.Id,
                    ActorId = i
                };
                _context.ActorTvShows.Add(result);
            }
            _context.SaveChanges();
        }
        public void UpdateTvShow(TvShowViewModel data)
        {
            int languageId = (int)data.Language;
            int typeId = (int)data.Type;

            var tvshow = _context.TvShows.Find(data.Id);

            tvshow.Name = data.Name;
            tvshow.Description = data.Description;
            tvshow.dbImage = data.dbImage;
            tvshow.IsSeries = data.IsSeries;
            tvshow.IsRamadan = data.IsRamadan;
            tvshow.PartsNo = data.PartsNo;
            tvshow.NoOfLikes = data.NoOfLikes;
            tvshow.Year = data.Year;
            tvshow.Month = data.Month;
            tvshow.Type = typeId;
            tvshow.Language = languageId;
            tvshow.CountryId = data.CountryId;
            tvshow.ProducerId = data.ProducerId;
            tvshow.CategoryId = data.CategoryId;
            _context.SaveChanges();

            var existingActorTvShows = _context.ActorTvShows.Where(af => af.TvShowId == tvshow.Id).ToList();
            _context.ActorTvShows.RemoveRange(existingActorTvShows);
            _context.SaveChanges();

            foreach (var i in data.ActorsId)
            {
                var result = new ActorTvShows()
                {
                    TvShowId = tvshow.Id,
                    ActorId = i
                };
                _context.ActorTvShows.Add(result);
            }
            _context.SaveChanges();
        }
        public IQueryable<TvShow> MostWatchedTvShows()
        {
            var result = _context.TvShows.OrderByDescending(x => x.NoOfLikes).Where(m => m.Type == 2).AsNoTracking();

            return result.Any() ? result : Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> RecentTvShows()
        {
            var result = _context.TvShows.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).Where(m => m.Type == 2 && !m.IsRamadan).AsNoTracking();

            return result.Any() ? result : Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> ArabicTvShows()
        {
            var result = _context.TvShows.Where(m => m.Type == 2 && m.Language == 1 && !m.IsRamadan).AsNoTracking();

            return result.Any() ? result : Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> RamadanTvShows()
        {
            var data = new List<TvShow>();
            var tvshows1 = _context.TvShows.Where(m => m.Type == 2 && m.IsRamadan && m.Year == DateTime.Today.Year && m.Month < 8).OrderBy(x => x.Year).AsNoTracking();
            data.AddRange(tvshows1);
            var tvshows2 = _context.TvShows.Where(m => m.Type == 2 && m.IsRamadan && (m.Year == DateTime.Today.Year || m.Year == DateTime.Today.Year - 1) && m.Month >= 8).OrderBy(x => x.Year).AsNoTracking();
            data.AddRange(tvshows2);

            if (data.Any())
            {
                data = data.OrderByDescending(x => x.Year).ToList();

                int year = data.First().Year;

                data = data.Where(t => t.Year == year).ToList();

                return data.AsQueryable();

            }
            else
            {
                return Enumerable.Empty<TvShow>().AsQueryable();
            }
        }
        public IQueryable<TvShow> GetRecentTvShows(int page)
        {
            List<TvShow> data = new List<TvShow>();

            var result1 = _context.TvShows.OrderBy(f => f.Name).Where(f => f.Month < 8 && f.Year == DateTime.Now.Year).ToList();
            if (result1 is not null)
                data.AddRange(result1);

            var result2 = _context.TvShows.OrderBy(f => f.Name).Where(f => f.Month >= 8 && (f.Year == DateTime.Now.Year || f.Year == DateTime.Now.Year - 1)).ToList();
            if (result2 is not null)
                data.AddRange(result2);

            if (data.Any())
                return data.AsQueryable();

            return Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> GetTopRatedTvShows(int page)
        {
            List<TvShow> data = new List<TvShow>();

            var result = _context.TvShows.OrderBy(f => f.Name).Where(f => f.NoOfLikes > 5000).ToList();

            if (result.Any())
            {
                data.AddRange(result);
                return data.AsQueryable();
            }

            return Enumerable.Empty<TvShow>().AsQueryable();
        }
        public IQueryable<TvShow> GetTvShowsForSearch(string key)
        {
            var result = _context.TvShows.Where(a => a.Name.Contains(key));

            return result.Any() ? result : Enumerable.Empty<TvShow>().AsQueryable();
        }
    }
}