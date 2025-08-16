using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public CountriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1;
            IEnumerable<Country> result = _unitOfWork.Countries.GetAll(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Country data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Countries.Add(data);
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
            var data = _unitOfWork.Countries.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Country data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Countries.Update(data);
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
            var data = _unitOfWork.Countries.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Country data)
        {
            _unitOfWork.Countries.Delete(data);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
