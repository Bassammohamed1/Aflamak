using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Country> GetCountryByID(int id)
        {
            return await _unitOfWork.Countries.GetById(id);
        }

        public async Task<IEnumerable<Country>> GetAllCountries(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Countries.GetAll(pageNumber, pageSize);
        }

        public async Task<Result> AddCountry(Country country)
        {
            var result = await _unitOfWork.Countries.Add(country);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while adding."
                 };
        }

        public async Task<Result> UpdateCountry(Country country)
        {
            var result = _unitOfWork.Countries.Update(country);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while updating."
                 };
        }

        public async Task<Result> DeleteCountry(Country country)
        {
            var result = _unitOfWork.Countries.Delete(country);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while deleting."
                 };
        }
    }
}
