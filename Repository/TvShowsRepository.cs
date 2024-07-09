using Aflamak.Data;
using Aflamak.Models;
using Aflamak.Models.ViewModels;
using Aflamak.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Aflamak.Repository
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
            return _context.TvShows.OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);
        }
        public IQueryable<TvShow> GetFilteredTvShowsWithId(int id, string Key)
        {
            if (Key == "ID")
                return _context.TvShows.Where(t => t.Id == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);

            else if (Key == "Producer")
                return _context.TvShows.Where(t => t.ProducerId == id).OrderBy(f => f.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor);

            else
                return null;
        }
        public IQueryable<TvShow> GetFilteredTvShows(string genre, string country, int? language, int? year, bool isArabic = false, bool isRamadan = false)
        {
            if (isArabic && !isRamadan)
            {
                var query = this.ArabicTvShows().AsQueryable();

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
                var query = this.RamadanTvShows().AsQueryable();

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
                var query = this.GetTvShows().AsQueryable();

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
            return _context.TvShows.OrderBy(t => t.Name).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public IEnumerable<TvShow> GetAllTvShowsOrderedByDate(int pageNumber, int pageSize)
        {
            return _context.TvShows.OrderByDescending(t => t.Year).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public IEnumerable<TvShow> GetAllTvShowsOrderedByLikes(int pageNumber, int pageSize)
        {
            return _context.TvShows.OrderByDescending(t => t.NoOfLikes).Include(m => m.Producer).Include(m => m.Category).Include(m => m.Country).Include(x => x.ActorTvShows).ThenInclude(m => m.Actor).ToPagedList(pageNumber, pageSize);
        }
        public TvShow GetTvShowById(int id)
        {
            return _context.TvShows.Include(x => x.Producer).Include(x => x.Category).Include(x => x.Country).Include(y => y.ActorTvShows).ThenInclude(z => z.Actor).FirstOrDefault(s => s.Id == id);
        }
        public IEnumerable<TvShow> GetAllTvShowsForSelectList()
        {
            return _context.TvShows.OrderBy(x => x.Name);
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
            foreach (var i in data.ActorsId)
            {
                var result = new ActorTvShows()
                {
                    TvShowId = tvshow.Id,
                    ActorId = i
                };
                _context.ActorTvShows.Update(result);
            }
            _context.SaveChanges();
        }
        public IQueryable<TvShow> MostWatchedTvShows()
        {
            return _context.TvShows.OrderByDescending(x => x.NoOfLikes).Where(m => m.Type == 2);
        }
        public IQueryable<TvShow> RecentTvShows()
        {
            return _context.TvShows.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).Where(m => m.Type == 2 && !m.IsRamadan);
        }
        public IQueryable<TvShow> ArabicTvShows()
        {
            return _context.TvShows.Where(m => m.Type == 2 && m.Language == 1 && !m.IsRamadan);
        }
        public IQueryable<TvShow> RamadanTvShows()
        {
            var data = new List<TvShow>();
            var tvshows1 = _context.TvShows.Where(m => m.Type == 2 && m.IsRamadan && m.Year == DateTime.Today.Year && m.Month < 8).OrderBy(x => x.Year);
            data.AddRange(tvshows1);
            var tvshows2 = _context.TvShows.Where(m => m.Type == 2 && m.IsRamadan && (m.Year == DateTime.Today.Year || m.Year == DateTime.Today.Year - 1) && m.Month >= 8).OrderBy(x => x.Year);
            data.AddRange(tvshows2);

            bool flag = false;
            int year = data.Last().Year;
            foreach (var item in data)
            {
                if (item.Year != year)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                foreach (var item in data)
                {
                    if (item.Year != year)
                        data.Remove(item);
                }
            }
            return data.OrderBy(x => x.Name).AsQueryable();
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

            return data.AsQueryable();
        }
        public IQueryable<TvShow> GetTopRatedTvShows(int page)
        {
            List<TvShow> data = new List<TvShow>();

            var result1 = _context.TvShows.OrderBy(f => f.Name).Where(f => f.NoOfLikes > 5000).ToList();
            if (result1 is not null)
                data.AddRange(result1);

            return data.AsQueryable();
        }
        public TvShow GetTvShow(string key)
        {
            var tvshow = _context.TvShows.SingleOrDefault(x => x.Name.ToLower() == key.ToLower());
  
            return tvshow;
        }
    }
}