using BusinessLogicLayer.Helpers;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICategoriesService
    {
        Task<Category> GetCategoryByID(int id);
        Task<IEnumerable<Category>> GetAllCategories(int pageNumber, int pageSize);
        Task<Result> AddCategory(Category category);
        Task<Result> UpdateCategory(Category category);
        Task<Result> DeleteCategory(Category category);
    }
}
