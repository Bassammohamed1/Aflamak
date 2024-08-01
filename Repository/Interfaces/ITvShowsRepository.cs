using Aflamak.Models;
using Aflamak.Models.ViewModels;

namespace Aflamak.Repository.Interfaces
{
    public interface ITvShowsRepository : IRepository<TvShow>
    {
        public TvShow GetTvShowById(int id);
        public IEnumerable<TvShow> GetTvShows();
        public IQueryable<TvShow> GetFilteredTvShowsWithId(int id, string Key);
        public IQueryable<TvShow> GetFilteredTvShows(string genre, string country, int? language, int? year, bool isArabic = false, bool isRamadan = false);
        public IQueryable<TvShow> GetRecentTvShows(int page);
        public IQueryable<TvShow> GetTopRatedTvShows(int page);
        public IEnumerable<TvShow> GetAllTvShows(int pageNumber, int pageSize);
        public IEnumerable<TvShow> GetAllTvShowsOrderedByDate(int pageNumber, int pageSize);
        public IEnumerable<TvShow> GetAllTvShowsOrderedByLikes(int pageNumber, int pageSize);
        public IEnumerable<TvShow> GetAllTvShowsForSelectList();
        public void AddTvShow(TvShowViewModel data);
        public void UpdateTvShow(TvShowViewModel data);
        public IQueryable<TvShow> MostWatchedTvShows();
        public IQueryable<TvShow> RecentTvShows();
        public IQueryable<TvShow> ArabicTvShows();
        public IQueryable<TvShow> RamadanTvShows();
        public IQueryable<TvShow> GetTvShowsForSearch(string key);
    }
}
