using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IEpisodesRepository : IRepository<Episode>
    {
        Task<IEnumerable<Episode>> GetAllEpisodes(int pageNumber, int pageSize);
        IEnumerable<Episode> GetAllEpisodes();
        IQueryable<Episode> GetFilteredEpisodesWithPartId(int id);
    }
}
