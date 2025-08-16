using DataAccessLayer.Models;

namespace DataAccessLayer.Repository.Interfaces
{
    public interface IPartsRepository : IRepository<Part>
    {
        IEnumerable<Part> GetAllParts(int pageNumber, int pageSize);
        IEnumerable<Part> GetAllPartsForSelectList();
        IEnumerable<Part> GetAllParts();
        IQueryable<Part> GetFilteredPartsWithTvShowId(int id);
    }
}
