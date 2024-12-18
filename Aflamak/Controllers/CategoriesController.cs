using Aflamak.Models;
using Aflamak.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aflamak.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<Category> result = _unitOfWork.Categories.GetAll(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Category data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(data);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(data);
            }
        }
        public IActionResult Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var data = _unitOfWork.Categories.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Category data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(data);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(data);
            }
        }
        public IActionResult Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var data = _unitOfWork.Categories.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Category data)
        {
            _unitOfWork.Categories.Delete(data);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}