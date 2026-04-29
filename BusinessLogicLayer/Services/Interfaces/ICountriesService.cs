using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICountriesService
    {
        Task<Country> GetCountryByID(int id);
        Task<IEnumerable<Country>> GetAllCountries(int pageNumber, int pageSize);
        Task<Result> AddCountry(Country country);
        Task<Result> UpdateCountry(Country country);
        Task<Result> DeleteCountry(Country country);
    }
}
