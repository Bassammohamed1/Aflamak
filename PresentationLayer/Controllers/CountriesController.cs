using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _countriesService;

        public CountriesController(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Country> result = await _countriesService.GetAllCountries(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Country data)
        {
            if (ModelState.IsValid)
            {
                var result = await _countriesService.AddCountry(data);

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

            var data = await _countriesService.GetCountryByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Country data)
        {
            if (ModelState.IsValid)
            {
                var result = await _countriesService.UpdateCountry(data);

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

            var data = await _countriesService.GetCountryByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(Country data)
        {
            var result =await _countriesService.DeleteCountry(data);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(data);
        }
    }
}
