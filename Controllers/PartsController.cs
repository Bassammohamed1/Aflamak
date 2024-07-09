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
    public class PartsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public PartsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateTvShowsSelectList()
        {
            var tvshows = _unitOfWork.TvShows.GetAllTvShowsForSelectList();
            SelectList List = new SelectList(tvshows, "Id", "Name");
            ViewBag.MyBag1 = List;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IEnumerable<Part> result = _unitOfWork.Parts.GetAllParts(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            CreateTvShowsSelectList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Part data)
        {
            if (ModelState.IsValid)
            {
                var stream = new MemoryStream();
                data.clientFile.CopyTo(stream);
                data.dbImage = stream.ToArray();
                _unitOfWork.Parts.Add(data);
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
            var data = _unitOfWork.Parts.GetById(Id);
            if (data == null)
                return NotFound();
            CreateTvShowsSelectList();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Part data)
        {
            if (ModelState.IsValid)
            {
                var stream = new MemoryStream();
                data.clientFile.CopyTo(stream);
                data.dbImage = stream.ToArray();
                _unitOfWork.Parts.Update(data);
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
            var data = _unitOfWork.Parts.GetById(Id);
            if (data == null)
                return NotFound();
            return View(data);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Part data)
        {
            _unitOfWork.Parts.Delete(data);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
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

                var part = _unitOfWork.Parts.GetById(id);
                if (part == null)
                    return NotFound();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

                if (interaction is not null)
                {
                    var viewModel = new PartDetailsVM()
                    {
                        Part = part,
                        Parts = parts,
                        Episodes = episodes,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return View(viewModel);
                }
                else
                {
                    var viewModel = new PartDetailsVM()
                    {
                        Part = part,
                        Parts = parts,
                        Episodes = episodes,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return View(viewModel);
                }
            }
            else
            {
                var part = _unitOfWork.Parts.GetById(id);
                if (part == null)
                    return NotFound();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

                var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

                var viewModel = new PartDetailsVM()
                {
                    Part = part,
                    Parts = parts,
                    Episodes = episodes,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return View(viewModel);
            }
        }
        [AllowAnonymous]
        public IActionResult LikePart(int partId)
        {
            Part part = new Part();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == partId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = partId,
                    IsLiked = true,
                    IsDisLiked = false
                };
                _unitOfWork.Interactions.Add(interaction);

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfLikes += 1;
                part.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            _unitOfWork.SaveChanges();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

            var viewModel = new PartDetailsVM()
            {
                Part = part,
                Parts = parts,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
        [AllowAnonymous]
        public IActionResult DislikePart(int partId)
        {
            Part part = new Part();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == partId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = partId,
                    IsLiked = false,
                    IsDisLiked = true
                };
                _unitOfWork.Interactions.Add(interaction);

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfDisLikes += 1;
                part.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                part = _unitOfWork.Parts.GetById(partId);
                part.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            _unitOfWork.SaveChanges();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(part.TvShowId).ToList();

            var episodes = _unitOfWork.Episodes.GetFilteredEpisodesWithPartId(part.Id).ToList();

            var viewModel = new PartDetailsVM()
            {
                Part = part,
                Parts = parts,
                Episodes = episodes,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
    }
}
