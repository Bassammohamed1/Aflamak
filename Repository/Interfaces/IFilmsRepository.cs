using Aflamak.Models;
using Aflamak.Models.ViewModels;

namespace Aflamak.Repository.Interfaces
{
    public interface IFilmsRepository : IRepository<Film>
    {
        public Film GetFilmById(int id);
        public IEnumerable<Film> GetFilms();
        public IQueryable<Film> GetFilteredFilmsWithId(int id, string Key);
        public IQueryable<Film> GetFilteredFilms(string genre, string country, int? language, int? year, bool isArabic = false, bool isCartoon = false);
        public IQueryable<Film> GetRecentFilms(int page);
        public IQueryable<Film> GetTopRatedFilms(int page);
        public IEnumerable<Film> GetAllFilms(int pageNumber, int pageSize);
        public IEnumerable<Film> GetAllFilmsOrderedByDate(int pageNumber, int pageSize);
        public IEnumerable<Film> GetAllFilmsOrderedByLikes(int pageNumber, int pageSize);
        public void AddFilm(FilmViewModel data);
        public void UpdateFilm(FilmViewModel data);
        public IQueryable<Film> MostWatchedFilms();
        public IQueryable<Film> RecentFilms();
        public IQueryable<Film> ArabicFilms();
        public IQueryable<Film> CartoonFilms();
        public List<Film> GetFilm(string key);
    }
}
