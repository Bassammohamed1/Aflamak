using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Category> GetCategoryByID(int id)
        {
            return await _unitOfWork.Categories.GetById(id);
        }

        public async Task<IEnumerable<Category>> GetAllCategories(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Categories.GetAll(pageNumber, pageSize);
        }

        public async Task<Result> AddCategory(Category category)
        {
            var result = await _unitOfWork.Categories.Add(category);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while adding."
                 };
        }

        public async Task<Result> UpdateCategory(Category category)
        {
            var result = _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChanges();

            return result is not null ? new Result() { Success = true } :
                 new Result()
                 {
                     Success = false,
                     Error = "An error ouccered while updating."
                 };
        }

        public async Task<Result> DeleteCategory(Category category)
        {
            var result = _unitOfWork.Categories.Delete(category);
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