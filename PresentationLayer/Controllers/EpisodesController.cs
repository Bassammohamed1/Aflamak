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
    public class EpisodesController : Controller
    {
        private void CreatePartsSelectList()
        {
            var parts = _episodesService.GetAllPartsForSelectList();

            SelectList List = new SelectList(parts, "Id", "Name");

            ViewBag.MyBag1 = List;
        }

        private readonly IEpisodesService _episodesService;

        public EpisodesController(IEpisodesService episodesService)
        {
            _episodesService = episodesService;
        }

        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            IEnumerable<Episode> result = await _episodesService.GetAllEpisodes(pageNumber, pageSize);

            return View(result);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(Episode data)
        {
            if (ModelState.IsValid)
            {
                var result = await _episodesService.AddEpisode(data);

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

            var data = await _episodesService.GetEpisodeByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(Episode data)
        {
            if (ModelState.IsValid)
            {
                var result = await _episodesService.UpdateEpisode(data);

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

            var data = await _episodesService.GetEpisodeByID(Id);

            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(Episode data)
        {
            var result = await _episodesService.DeleteEpisode(data);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(data);
        }

        [AllowAnonymous]
        public IActionResult Recent(int page = 1)
        {
            var data = _episodesService.GetRecentEpisodes(page);

            var viewModel = new EpisodeVM
            {
                Episodes = data.Episodes,
                CurrentPage = data.CurrentPage,
                TotalPages = data.TotalPages
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult LoadMoreEpisodes(int page)
        {
            var data = _episodesService.LoadMoreEpisodes(page);

            var viewModel = new EpisodeVM
            {
                Episodes = data.Episodes
            };

            return PartialView("_Recent", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var result = await _episodesService.EpisodeDetails(id, true, userID);

                return result is not null ? View(result) : NotFound();
            }
            else
            {
                var result = await _episodesService.EpisodeDetails(id, false, null);

                return result is not null ? View(result) : NotFound();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> LikeEpisode(int episodeId)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _episodesService.LikeEpisode(episodeId, userID);

            var viewModel = new EpisodeDetailsVM()
            {
                Episode = data.Episode,
                Episodes = data.Episodes,
                HasUserLiked = data.HasUserLiked,
                HasUserDisliked = data.HasUserDisliked
            };


            return View("Details", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DisLikeEpisode(int episodeId)
        {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var data = await _episodesService.DisLikeEpisode(episodeId, userID);

            var viewModel = new EpisodeDetailsVM()
            {
                Episode = data.Episode,
                Episodes = data.Episodes,
                HasUserLiked = data.HasUserLiked,
                HasUserDisliked = data.HasUserDisliked
            };


            return View("Details", viewModel);
        }
    }
}