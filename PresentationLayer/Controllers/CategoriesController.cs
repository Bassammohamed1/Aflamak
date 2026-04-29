using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Category> result = await _categoriesService.GetAllCategories(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Category data)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoriesService.AddCategory(data);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(data);
            }
            else
            {
                return View(data);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var data = await _categoriesService.GetCategoryByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Category data)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoriesService.UpdateCategory(data);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(data);
            }
            else
            {
                return View(data);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var data = await _categoriesService.GetCategoryByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(Category data)
        {
            var result = await _categoriesService.DeleteCategory(data);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(data);
        }
    }
}