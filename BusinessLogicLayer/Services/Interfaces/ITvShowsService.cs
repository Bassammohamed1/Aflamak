using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ITvShowsService
    {
        Task<TvShow> GetTvShowByID(int id);
        IEnumerable<TvShow> GetTvShows(Keys? key, Languages? language, ItemType? itemType, bool isRamadan);
        Task<IEnumerable<TvShow>> GetAllTvShows(int pageNumber, int pageSize);
        IQueryable<TvShow> GetFilteredTvShowsWithKey(int id, Keys key);
        Task<IEnumerable<TvShow>> GetAllTvShowsOrderedByKey(int pageNumber, int pageSize, Keys key);
        IQueryable<TvShow> GetFilteredTvShows(string genre, string country, int? language, int? year, bool isArabic = false, bool isCartoon = false);
        IQueryable<TvShow> GetRelatedTvShowsByKey(Keys key);
        Task<Result> AddTvShow(TvShowDTO TvShow);
        Task<Result> UpdateTvShow(TvShowDTO TvShow);
        Result DeleteTvShow(TvShow TvShow);
        Task<TvShowDetailsDTO> TvShowDetails(int TvShowID, bool isAuthenticated, string? userID);
        Task<TvShowDetailsDTO> LikeTvShow(int TvShowID, string userID);
        Task<TvShowDetailsDTO> DisLikeTvShow(int TvShowID, string userID);
        IEnumerable<TvShow> GetTvShowsForSearch(string key);
        Task<TvShowDataForSelectListsDTO> GetTvShowDataForSelectLists();
    }
}
