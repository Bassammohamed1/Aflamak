using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IFilmsRepository : IRepository<Film>
    {
        Task<Film> GetFilmById(int id);
        IEnumerable<Film> GetFilms();
        Task<IEnumerable<Film>> GetAllFilms(int pageNumber, int pageSize);
        Task<IEnumerable<Film>> GetAllFilmsOrderedByKey(int pageNumber, int pageSize, Keys key);
        IQueryable<Film> GetFilteredFilmsWithID(int id);
        IQueryable<Film> GetFilteredFilmsWithProducerID(int id);
        Task<Film?> AddFilm(Film data, List<int> actorsIDs);
        Task<Film?> UpdateFilm(Film data, List<int> actorsIDs);
    }
}
