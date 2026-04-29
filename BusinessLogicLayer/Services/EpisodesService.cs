using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class EpisodesService : IEpisodesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EpisodesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Episode> GetEpisodeByID(int id)
        {
            return await _unitOfWork.Episodes.GetById(id);
        }

        public async Task<IEnumerable<Episode>> GetAllEpisodes(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Episodes.GetAll(pageNumber, pageSize);
        }

        public async Task<Result> AddEpisode(Episode episode)
        {
            var result = await _unitOfWork.Episodes.Add(episode);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while adding."
                 };
        }

        public async Task<Result> UpdateEpisode(Episode episode)
        {
            var result = _unitOfWork.Episodes.Update(episode);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while updating."
                 };
        }

        public async Task<Result> DeleteEpisode(Episode episode)
        {
            var result = _unitOfWork.Episodes.Delete(episode);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while deleting."
                 };
        }

        public EpisodeDTO GetRecentEpisodes(int? page)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;

            var allEpisodes = _unitOfWork.Episodes.GetAllEpisodes();

            var episodes = allEpisodes
                .Where(e => (e.Part.Month == DateTime.Now.Month || e.Part.Month == DateTime.Now.Month - 1) && e.Part.Date == DateTime.Now.Year)
                .OrderByDescending(x => x.Part.Month);

            int totalItems = episodes.Count();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedItems = episodes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new EpisodeDTO
            {
                Episodes = pagedItems,
                CurrentPage = pageNumber,
                TotalPages = totalPages
            };

            return viewModel;
        }

        public EpisodeDTO LoadMoreEpisodes(int? page)
        {
            int pageSize = 9;
            int pageNumber = page ?? 1;

            var data = this.GetRecentEpisodes(1).Episodes.ToList();

            var pagedItems = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new EpisodeDTO
            {
                Episodes = pagedItems,
            };

            return viewModel;
        }

        public async Task<EpisodeDetailsDTO> EpisodeDetails(int id, bool isAuthenticated, string? userID)
        {
            if (isAuthenticated)
            {
                var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                    .FirstOrDefault(fi => fi.ItemId == id && fi.UserId == userID);

                var episode = await _unitOfWork.Episodes.GetById(id);

                if (episode is null)
                    return new EpisodeDetailsDTO();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

                if (interaction is not null)
                {
                    var viewModel = new EpisodeDetailsDTO()
                    {
                        Episode = episode,
                        Episodes = episodes,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return viewModel;
                }
                else
                {
                    var viewModel = new EpisodeDetailsDTO()
                    {
                        Episode = episode,
                        Episodes = episodes,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return viewModel;
                }
            }
            else
            {
                var episode = await _unitOfWork.Episodes.GetById(id);

                if (episode is null)
                    return new EpisodeDetailsDTO();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

                var viewModel = new EpisodeDetailsDTO()
                {
                    Episode = episode,
                    Episodes = episodes,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return viewModel;
            }
        }

        public async Task<EpisodeDetailsDTO> LikeEpisode(int episodeID, string userID)
        {
            Episode episode = new Episode();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == episodeID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = episodeID,
                    IsLiked = true,
                    IsDisLiked = false
                };

                await _unitOfWork.Interactions.Add(interaction);

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfLikes += 1;
                episode.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            await _unitOfWork.SaveChanges();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

            var data = new EpisodeDetailsDTO()
            {
                Episode = episode,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return data;
        }

        public async Task<EpisodeDetailsDTO> DisLikeEpisode(int episodeID, string userID)
        {
            Episode episode = new Episode();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == episodeID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = episodeID,
                    IsLiked = false,
                    IsDisLiked = true
                };
                await _unitOfWork.Interactions.Add(interaction);

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfDisLikes += 1;
                episode.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                episode = await _unitOfWork.Episodes.GetById(episodeID);
                episode.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            await _unitOfWork.SaveChanges();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

            var data = new EpisodeDetailsDTO()
            {
                Episode = episode,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return data;
        }

        public IEnumerable<Part> GetAllPartsForSelectList()
        {
            return _unitOfWork.Parts.GetAllPartsForSelectList();
        }
    }
}
