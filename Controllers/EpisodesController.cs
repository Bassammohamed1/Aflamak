using Aflamak.Models;
using Aflamak.Models.ViewModels;
using Aflamak.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Aflamak.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EpisodesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public EpisodesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreatePartsSelectList()
        {
            var parts = _unitOfWork.Parts.GetAllPartsForSelectList();
            SelectList List = new SelectList(parts, "Id", "Name");
            ViewBag.MyBag1 = List;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<Episode> result = _unitOfWork.Episodes.GetAllEpisodes(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            CreatePartsSelectList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Episode data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Episodes.Add(data);
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
            var data = _unitOfWork.Episodes.GetById(Id);
            if (data == null)
                return NotFound();
            CreatePartsSelectList();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Episode data)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Episodes.Update(data);
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
            var data = _unitOfWork.Episodes.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Episode data)
        {
            _unitOfWork.Episodes.Delete(data);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Recent(int page = 1)
        {
            var data = _unitOfWork.Episodes.GetRecentEpisodes().ToList();
            int pageSize = 9;

            int totalItems = data.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedItems = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new EpisodeVM
            {
                Episodes = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages,
            };

            return View(viewModel);
        }
        [AllowAnonymous]
        public IActionResult LoadMoreEpisodes(int page)
        {
            var data = _unitOfWork.Episodes.GetRecentEpisodes().ToList();
            int pageSize = 9;

            var pagedItems = data.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new EpisodeVM
            {
                Episodes = pagedItems,
            };

            return PartialView("_Recent", viewModel);
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().FirstOrDefault(fi => fi.ItemId == id && fi.UserId == userId);

                var episode = _unitOfWork.Episodes.GetById(id);
                if (episode == null)
                    return NotFound();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

                if (interaction is not null)
                {
                    var viewModel = new EpisodeDetailsVM()
                    {
                        Episode = episode,
                        Episodes = episodes,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return View(viewModel);
                }
                else
                {
                    var viewModel = new EpisodeDetailsVM()
                    {
                        Episode = episode,
                        Episodes = episodes,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return View(viewModel);
                }
            }
            else
            {
                var episode = _unitOfWork.Episodes.GetById(id);
                if (episode == null)
                    return NotFound();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

                var viewModel = new EpisodeDetailsVM()
                {
                    Episode = episode,
                    Episodes = episodes,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return View(viewModel);
            }
        }
        [AllowAnonymous]
        public IActionResult LikeEpisode(int episodeId)
        {
            Episode episode = new Episode();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == episodeId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = episodeId,
                    IsLiked = true,
                    IsDisLiked = false
                };
                _unitOfWork.Interactions.Add(interaction);

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfLikes += 1;
                episode.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            _unitOfWork.SaveChanges();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

            var viewModel = new EpisodeDetailsVM()
            {
                Episode = episode,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
        [AllowAnonymous]
        public IActionResult DisLikeEpisode(int episodeId)
        {
            Episode episode = new Episode();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == episodeId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = episodeId,
                    IsLiked = false,
                    IsDisLiked = true
                };
                _unitOfWork.Interactions.Add(interaction);

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfDisLikes += 1;
                episode.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                episode = _unitOfWork.Episodes.GetById(episodeId);
                episode.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            _unitOfWork.SaveChanges();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(episode.PartId).ToList();

            var viewModel = new EpisodeDetailsVM()
            {
                Episode = episode,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
    }
}
