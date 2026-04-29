using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PartsController : Controller
    {
        private readonly IPartsService _partsService;

        public PartsController(IPartsService partsService)
        {
            _partsService = partsService;
        }

        public void CreateTvShowsSelectList()
        {
            var tvShows = _partsService.GetAllTvShowsForSelectList();

            SelectList List = new SelectList(tvShows, "Id", "Name");

            ViewBag.MyBag1 = List;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Part> result = await _partsService.GetAllParts(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            CreateTvShowsSelectList();
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Part data)
        {
            if (ModelState.IsValid)
            {
                var result = await _partsService.AddPart(data);

                return result.Success ? RedirectToAction(nameof(Index))
                    : View(data);
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

            var data = await _partsService.GetPartByID(Id);

            if (data == null)
                return NotFound();

            CreateTvShowsSelectList();
            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Part data)
        {
            if (ModelState.IsValid)
            {
                var result = await _partsService.UpdatePart(data);

                return result.Success ? RedirectToAction(nameof(Index))
                    : View(data);
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

            var data = await _partsService.GetPartByID(Id);

            if (data == null)
                return NotFound();

            CreateTvShowsSelectList();
            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(Part data)
        {
            if (ModelState.IsValid)
            {
                var result = await _partsService.DeletePart(data);

                return result.Success ? RedirectToAction(nameof(Index))
                    : View(data);
            }
            else
            {
                return View(data);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _partsService.PartDetails(id, true, userID);

                var viewModel = new PartDetailsVM()
                {
                    Part = result.Part,
                    Parts = result.Parts,
                    Episodes = result.Episodes,
                    HasUserLiked = result.HasUserLiked,
                    HasUserDisliked = result.HasUserDisliked
                };

                return View(viewModel);
            }
            else
            {
                var result = await _partsService.PartDetails(id, false, null);

                var viewModel = new PartDetailsVM()
                {
                    Part = result.Part,
                    Parts = result.Parts,
                    Episodes = result.Episodes,
                    HasUserLiked = result.HasUserLiked,
                    HasUserDisliked = result.HasUserDisliked
                };

                return View(viewModel);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> LikePart(int partId)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _partsService.LikePart(partId, userID);

            var viewModel = new PartDetailsVM()
            {
                Part = result.Part,
                Parts = result.Parts,
                Episodes = result.Episodes,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DislikePart(int partId)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _partsService.DisLikePart(partId, userID);

            var viewModel = new PartDetailsVM()
            {
                Part = result.Part,
                Parts = result.Parts,
                Episodes = result.Episodes,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View(viewModel);
        }
    }
}
