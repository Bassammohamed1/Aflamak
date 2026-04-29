using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ViewModels;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActorsController : Controller
    {
        private readonly IActorsService _actorsService;

        public ActorsController(IActorsService actorsService)
        {
            _actorsService = actorsService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Actor> result = await _actorsService.GetAllActors(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Actor actor)
        {
            if (ModelState.IsValid)
            {
                var result = await _actorsService.AddActor(actor);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(actor);
            }
            else
            {
                return View(actor);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var actor = await _actorsService.GetActorByID(Id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Actor actor)
        {
            if (ModelState.IsValid)
            {
                var result = await _actorsService.UpdateActor(actor);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(actor);
            }
            else
            {
                return View(actor);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var actor = await _actorsService.GetActorByID(Id);

            if (actor == null)
                return NotFound();

            return View(actor);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Actor actor)
        {
            var result = _actorsService.DeleteActor(actor);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(actor);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
                return NotFound();

            var result = await _actorsService.GetActorDetails(id);

            var data = new PersonDetailsVM<Actor>()
            {
                Person = result.Person,
                Works = result.Works
            };

            return data is not null ? View(data) : NotFound();
        }
    }
}
