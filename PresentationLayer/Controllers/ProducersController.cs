using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProducersController : Controller
    {
        private readonly IProducersService _producersService;

        public ProducersController(IProducersService producersService)
        {
            _producersService = producersService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Producer> result =await _producersService.GetAllProducers(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Producer producer)
        {
            if (ModelState.IsValid)
            {
                var result = await _producersService.AddProducer(producer);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(producer);
            }
            else
            {
                return View(producer);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var producer = await _producersService.GetProducerByID(Id);

            if (producer == null)
                return NotFound();

            return View(producer);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Producer producer)
        {
            if (ModelState.IsValid)
            {
                var result = await _producersService.UpdateProducer(producer);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(producer);
            }
            else
            {
                return View(producer);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var producer = await _producersService.GetProducerByID(Id);

            if (producer == null)
                return NotFound();

            return View(producer);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Producer producer)
        {
            var result = _producersService.DeleteProducer(producer);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(producer);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
                return NotFound();

            var producer = await _producersService.ProducerDetails(id);

            var viewModel = new PersonDetailsVM<Producer>()
            {
                Person = producer.Person,
                Works = producer.Works
            };

            return View(viewModel);
        }
    }
}
