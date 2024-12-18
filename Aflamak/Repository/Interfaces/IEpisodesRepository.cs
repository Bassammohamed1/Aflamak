using Aflamak.Models;

namespace Aflamak.Repository.Interfaces
{
    public interface IEpisodesRepository : IRepository<Episode>
    {
        IEnumerable<Episode> GetAllEpisodes(int pageNumber, int pageSize);
        IEnumerable<Episode> GetAllEpisodes();
        IQueryable<Episode> GetRecentEpisodes();
        IQueryable<Episode> GetFilteredEpisodesWithPartId(int id);
    }
}
