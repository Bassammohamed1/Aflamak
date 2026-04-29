using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using System.Security.Claims;

namespace BusinessLogicLayer.Services
{
    public class PartsService : IPartsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Part> GetPartByID(int id)
        {
            return await _unitOfWork.Parts.GetById(id);
        }

        public async Task<IEnumerable<Part>> GetAllParts(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Parts.GetAllParts(pageNumber, pageSize);
        }

        public async Task<Result> AddPart(Part part)
        {
            if (part.clientFile is not null)
            {
                var stream = new MemoryStream();
                await part.clientFile.CopyToAsync(stream);
                part.dbImage = stream.ToArray();

                var result = await _unitOfWork.Parts.Add(part);
                await _unitOfWork.SaveChanges();

                return result is not null ? new Result() { Success = true } :
                new Result()
                {
                    Success = false,
                    Error = "An error ouccered while adding."
                };
            }

            return new Result()
            {
                Success = false,
                Error = "clientFile is missing."
            };
        }

        public async Task<Result> UpdatePart(Part part)
        {
            if (part.clientFile is not null)
            {
                var stream = new MemoryStream();
                await part.clientFile.CopyToAsync(stream);
                part.dbImage = stream.ToArray();

                var result = _unitOfWork.Parts.Update(part);
                await _unitOfWork.SaveChanges();

                return result is not null ? new Result() { Success = true } :
                new Result()
                {
                    Success = false,
                    Error = "An error ouccered while updating."
                };
            }

            return new Result()
            {
                Success = false,
                Error = "clientFile is missing."
            };
        }

        public async Task<Result> DeletePart(Part part)
        {
            var result = _unitOfWork.Parts.Delete(part);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while deleting."
                 };
        }

        public async Task<PartDetailsDTO> PartDetails(int partID, bool isAuthenticated, string? userID)
        {
            if (isAuthenticated)
            {
                var part = await _unitOfWork.Parts.GetById(partID);

                if (part == null)
                    return new PartDetailsDTO();

                var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                    .FirstOrDefault(fi => fi.ItemId == partID && fi.UserId == userID);

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

                if (interaction is not null)
                {
                    var data = new PartDetailsDTO()
                    {
                        Part = part,
                        Parts = parts,
                        Episodes = episodes,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return data;
                }
                else
                {
                    var data = new PartDetailsDTO()
                    {
                        Part = part,
                        Parts = parts,
                        Episodes = episodes,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return data;
                }
            }
            else
            {
                var part = await _unitOfWork.Parts.GetById(partID);

                if (part == null)
                    return new PartDetailsDTO();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

                var data = new PartDetailsDTO()
                {
                    Part = part,
                    Parts = parts,
                    Episodes = episodes,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return data;
            }
        }

        public async Task<PartDetailsDTO> LikePart(int partID, string userID)
        {
            Part part = new Part();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == partID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = partID,
                    IsLiked = true,
                    IsDisLiked = false
                };
                await _unitOfWork.Interactions.Add(interaction);

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfLikes += 1;
                part.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            await _unitOfWork.SaveChanges();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

            var data = new PartDetailsDTO()
            {
                Part = part,
                Parts = parts,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return data;
        }

        public async Task<PartDetailsDTO> DisLikePart(int partID, string userID)
        {
            Part part = new Part();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == partID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = partID,
                    IsLiked = false,
                    IsDisLiked = true
                };
                await _unitOfWork.Interactions.Add(interaction);

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfDisLikes += 1;
                part.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                part = await _unitOfWork.Parts.GetById(partID);
                part.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            await _unitOfWork.SaveChanges();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

            var data = new PartDetailsDTO()
            {
                Part = part,
                Parts = parts,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return data;
        }

        public IEnumerable<TvShow> GetAllTvShowsForSelectList()
        {
            return _unitOfWork.TvShows.GetAllTvShowsForSelectList();
        }
    }
}
