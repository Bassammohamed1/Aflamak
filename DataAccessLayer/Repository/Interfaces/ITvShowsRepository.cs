using DataAccessLayer.Enums;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface ITvShowsRepository : IRepository<TvShow>
    {
        Task<TvShow?> GetTvShowById(int id);
        IEnumerable<TvShow> GetTvShows();
        Task<IEnumerable<TvShow>> GetAllTvShows(int pageNumber, int pageSize);
        Task<IEnumerable<TvShow>> GetAllTvShowsOrderedByKey(int pageNumber, int pageSize, Keys key);
        IQueryable<TvShow> GetFilteredTvShowsWithID(int id);
        IQueryable<TvShow> GetFilteredTvShowsWithProducerID(int id);
        Task<TvShow?> AddTvShow(TvShow data, List<int> actorsIDs);
        Task<TvShow?> UpdateTvShow(TvShow data, List<int> actorsIDs);
        IEnumerable<TvShow> GetAllTvShowsForSelectList();
    }
}
