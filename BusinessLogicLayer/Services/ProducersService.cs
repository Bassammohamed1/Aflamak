using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class ProducersService : IProducersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProducersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Producer> GetProducerByID(int id)
        {
            return await _unitOfWork.Producers.GetById(id);
        }

        public async Task<IEnumerable<Producer>> GetAllProducers(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Producers.GetAll(pageNumber, pageSize);
        }

        public async Task<Result> AddProducer(Producer producer)
        {
            if (producer.clientFile != null)
            {
                var stream = new MemoryStream();
                await producer.clientFile.CopyToAsync(stream);
                producer.dbImage = stream.ToArray();

                var result = await _unitOfWork.Producers.Add(producer);
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

        public async Task<Result> UpdateProducer(Producer producer)
        {
            if (producer.clientFile != null)
            {
                var stream = new MemoryStream();
                await producer.clientFile.CopyToAsync(stream);
                producer.dbImage = stream.ToArray();

                var result = _unitOfWork.Producers.Update(producer);
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

        public Result DeleteProducer(Producer producer)
        {
            var result = _unitOfWork.Producers.Delete(producer);

            return result is not null ? new Result() { Success = true } :
                     new Result()
                     {
                         Success = false,
                         Error = "An error ouccered while deleting."
                     };
        }

        public IEnumerable<Producer> GetProducersForSearch(string key)
        {
            var results = _unitOfWork.Producers.GetAllWithoutPagination().Result
                 .Where(a => a.Name.ToLower().Contains(key.ToLower()) || a.AnotherLangName.ToLower().Contains(key.ToLower()));

            return results.Any() ? results : Enumerable.Empty<Producer>();
        }

        public async Task<PersonDetailsDTO<Producer>> ProducerDetails(int id)
        {
            var producer = await _unitOfWork.Producers.GetById(id);

            if (producer == null)
                throw new NullReferenceException();

            var films = _unitOfWork.Films.GetFilteredFilmsWithProducerID(producer.Id).ToList();
            var tvShows = _unitOfWork.TvShows.GetFilteredTvShowsWithProducerID(producer.Id).ToList();

            var works = films.Select(f => new ItemDTO { Type = "Film", Item = f }).
                Concat(tvShows.Select(t => new ItemDTO { Type = "TvShow", Item = t })).ToList();

            var data = new PersonDetailsDTO<Producer>()
            {
                Person = producer,
                Works = works
            };

            return data;
        }
    }
}
