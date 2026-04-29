using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IFilmsService
    {
        Task<Film> GetFilmByID(int id);
        IEnumerable<Film> GetFilms(Keys? key, Languages? language, ItemType? itemType);
        Task<IEnumerable<Film>> GetAllFilms(int pageNumber, int pageSize);
        IQueryable<Film> GetFilteredFilmsWithKey(int id, Keys key);
        Task<IEnumerable<Film>> GetAllFilmsOrderedByKey(int pageNumber, int pageSize, Keys key);
        IQueryable<Film> GetFilteredFilms(string genre, string country, int? language, int? year, bool isArabic = false, bool isCartoon = false);
        IQueryable<Film> GetRelatedFilmsByKey(Keys key);
        Task<Result> AddFilm(FilmDTO film);
        Task<Result> UpdateFilm(FilmDTO film);
        Result DeleteFilm(Film film);
        Task<FilmDetailsDTO> FilmDetails(int filmID, bool isAuthenticated, string? userID);
        Task<FilmDetailsDTO> LikeFilm(int filmID, string userID);
        Task<FilmDetailsDTO> DisLikeFilm(int filmID, string userID);
        IEnumerable<Film> GetFilmsForSearch(string key);
        Task<FilmDataForSelectListsDTO> GetFilmDataForSelectLists();
    }
}
