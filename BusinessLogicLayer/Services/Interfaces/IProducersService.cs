using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;


namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IProducersService
    {
        Task<Producer> GetProducerByID(int id);
        Task<IEnumerable<Producer>> GetAllProducers(int pageNumber, int pageSize);
        Task<Result> AddProducer(Producer producer);
        Task<Result> UpdateProducer(Producer producer);
        Result DeleteProducer(Producer producer);
        IEnumerable<Producer> GetProducersForSearch(string key);
        Task<PersonDetailsDTO<Producer>> ProducerDetails(int id);
    }
}
