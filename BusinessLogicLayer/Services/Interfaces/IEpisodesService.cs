using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IEpisodesService
    {
        Task<Episode> GetEpisodeByID(int id);
        Task<IEnumerable<Episode>> GetAllEpisodes(int pageNumber, int pageSize);
        Task<Result> AddEpisode(Episode episode);
        Task<Result> UpdateEpisode(Episode episode);
        Task<Result> DeleteEpisode(Episode episode);
        EpisodeDTO GetRecentEpisodes(int? page);
        EpisodeDTO LoadMoreEpisodes(int? page);
        Task<EpisodeDetailsDTO> EpisodeDetails(int id, bool isAuthenticated, string userID);
        Task<EpisodeDetailsDTO> LikeEpisode(int episodeID, string userID);
        Task<EpisodeDetailsDTO> DisLikeEpisode(int episodeID, string userID);
        IEnumerable<Part> GetAllPartsForSelectList();
    }
}
