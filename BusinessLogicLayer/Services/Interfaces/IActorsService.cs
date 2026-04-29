using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IActorsService
    {
        Task<Actor> GetActorByID(int id);
        Task<IEnumerable<Actor>> GetAllActors(int pageNumber, int pageSize);
        Task<Result> AddActor(Actor actor);
        Task<Result> UpdateActor(Actor actor);
        Result DeleteActor(Actor actor);
        IEnumerable<Actor> GetActorsForSearch(string key);
        Task<PersonDetailsDTO<Actor>> GetActorDetails(int id);
    }
}
