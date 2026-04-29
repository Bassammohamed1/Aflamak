using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IPartsService
    {
        Task<Part> GetPartByID(int id);
        Task<IEnumerable<Part>> GetAllParts(int pageNumber, int pageSize);
        Task<Result> AddPart(Part part);
        Task<Result> UpdatePart(Part part);
        Task<Result> DeletePart(Part part);
        Task<PartDetailsDTO> PartDetails(int partID, bool isAuthenticated, string? userID);
        Task<PartDetailsDTO> LikePart(int partID, string userID);
        Task<PartDetailsDTO> DisLikePart(int partID, string userID);
        IEnumerable<TvShow> GetAllTvShowsForSelectList();
    }
}
