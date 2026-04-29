using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TvShowsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITvShowsService _tvShowsService;

        public TvShowsController(IUnitOfWork unitOfWork, ITvShowsService tvShowsService)
        {
            _unitOfWork = unitOfWork;
            _tvShowsService = tvShowsService;
        }

        private async Task CreateNeededSelectLists()
        {
            var allNeededData = await _tvShowsService.GetTvShowDataForSelectLists();

            var actors = allNeededData.Actors;
            var producers = allNeededData.Producers;
            var categories = allNeededData.Categories;
            var countries = allNeededData.Countries;

            actors = actors.OrderBy(x => x.Name).ToList();
            producers = producers.OrderBy(x => x.Name).ToList();
            categories = categories.OrderBy(x => x.Name).ToList();
            countries = countries.OrderBy(x => x.Name).ToList();

            SelectList actorsList = new SelectList(actors, "Id", "Name");
            SelectList producersList = new SelectList(producers, "Id", "Name");
            SelectList categoriesList = new SelectList(categories, "Id", "Name");
            SelectList countriesList = new SelectList(countries, "Id", "Name");

            ViewBag.MyBag1 = actorsList;
            ViewBag.MyBag2 = producersList;
            ViewBag.MyBag3 = categoriesList;
            ViewBag.MyBag4 = countriesList;
        }

        public async Task<IActionResult> Index(int? page, bool sortLikes = false, bool sortDate = false)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            IEnumerable<TvShow> result;

            if (!sortDate && !sortLikes)
            {
                result = await _tvShowsService.GetAllTvShows(pageNumber, pageSize);
            }
            else if (sortLikes && !sortDate)
            {
                result = await _tvShowsService.GetAllTvShowsOrderedByKey(pageNumber, pageSize, Keys.NoOfLikes);
            }
            else
            {
                result = await _tvShowsService.GetAllTvShowsOrderedByKey(pageNumber, pageSize, Keys.Year);
            }

            ViewBag.SortLikes = sortLikes;
            ViewBag.SortDate = sortDate;
            ViewBag.SortLanguage = sortDate;

            return View(result);
        }

        public async Task<IActionResult> Add()
        {
            await this.CreateNeededSelectLists();

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Add(TvShowDTO TvShow)
        {
            if (ModelState.IsValid)
            {
                var result = await _tvShowsService.AddTvShow(TvShow);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(TvShow);
            }
            else
            {
                return View(TvShow);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var tvShow = await _tvShowsService.GetTvShowByID(Id);

            int languageId = tvShow.Language.Value;
            Languages languageEnum = (Languages)languageId;

            int typeId = tvShow.Type.Value;
            ItemType typeEnum = (ItemType)typeId;

            if (tvShow == null)
                return NotFound();

            var result = new TvShowViewModel()
            {
                Id = tvShow.Id,
                Name = tvShow.Name,
                Description = tvShow.Description,
                dbImage = tvShow.dbImage,
                IsSeries = tvShow.IsSeries,
                PartsNo = tvShow.PartsNo,
                NoOfLikes = tvShow.NoOfLikes,
                NoOfDisLikes = tvShow.NoOfDisLikes,
                Year = tvShow.Year,
                Month = tvShow.Month,
                Type = typeEnum,
                Language = languageEnum,
                CountryId = tvShow.CountryId,
                ProducerId = tvShow.ProducerId,
                CategoryId = tvShow.CategoryId,
                ActorsId = tvShow.ActorTvShows.Select(a => a.ActorId).ToList()
            };

            await this.CreateNeededSelectLists();

            return View(result);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(TvShowDTO TvShow)
        {
            if (ModelState.IsValid)
            {
                var result = await _tvShowsService.UpdateTvShow(TvShow);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(TvShow);
            }
            else
            {
                return View(TvShow);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {

            if (Id == null || Id == 0)
                return NotFound();

            var tvShow = await _tvShowsService.GetTvShowByID(Id);

            if (tvShow == null)
                return NotFound();

            return View(tvShow);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(TvShow TvShow)
        {
            var result = _tvShowsService.DeleteTvShow(TvShow);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(TvShow);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var TvShow = await _tvShowsService.TvShowDetails(id, true, userID);

                var data = new TvShowDetailsVM()
                {
                    TvShow = TvShow.TvShow,
                    HasUserDisliked = TvShow.HasUserDisliked,
                    HasUserLiked = TvShow.HasUserLiked,
                    RelatedTvShows = TvShow.RelatedTvShows
                };

                return View(data);
            }
            else
            {
                var TvShow = await _tvShowsService.TvShowDetails(id, false, null);

                var data = new TvShowDetailsVM()
                {
                    TvShow = TvShow.TvShow,
                    TvShowParts = TvShow.TvShowParts,
                    HasUserDisliked = TvShow.HasUserDisliked,
                    HasUserLiked = TvShow.HasUserLiked,
                    RelatedTvShows = TvShow.RelatedTvShows
                };

                return View(data);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> TvShowsList(int page = 1, bool isArabic = false, bool isRamadan = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<TvShow> TvShows = new List<TvShow>();

            if (isArabic && !isRamadan)
            {
                TvShows = _tvShowsService.GetTvShows(null, Languages.عربي, ItemType.مسلسل, false).ToList();
                flag = true;
            }
            else if (isRamadan && !isArabic)
            {
                TvShows = _tvShowsService.GetTvShows(null, Languages.عربي, ItemType.مسلسل, true).ToList();
                flag = true;
            }
            else
            {
                TvShows = _tvShowsService.GetTvShows(null, null, ItemType.مسلسل, false).ToList();
            }

            int totalItems = TvShows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedItems = TvShows.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new TvShowVM
            {
                TvShows = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages,
                FromHome = flag,
                Arabic = isArabic,
                Ramadan = isRamadan
            };

            var enumValues = Enum.GetValues(typeof(Languages))
                .Cast<Languages>().Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();

            SelectList enumSelectList = new SelectList(enumValues, "Id", "Name");
            ViewBag.MyBag5 = enumSelectList;

            await this.CreateNeededSelectLists();

            List<int> datesList = new List<int>();

            for (int i = 1950; i <= DateTime.Now.Year; i++)
            {
                datesList.Add(i);
            }

            var dates = datesList.OrderByDescending(x => x)
                .Select(n => new { Value = n, Text = n.ToString() }).ToList();

            ViewBag.MyBag6 = dates;

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult LoadMoreTvShows(int page, string genre, string country, int? language, int? year, bool fromHome = false, bool isArabic = false, bool isRamadan = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<TvShow> combinedItems = new List<TvShow>();

            if (isArabic && !isRamadan)
            {
                combinedItems = _tvShowsService.GetFilteredTvShows(genre, country, language, year, true, false).ToList();
                flag = true;
            }
            else if (isRamadan && !isArabic)
            {
                combinedItems = _tvShowsService.GetFilteredTvShows(genre, country, language, year, false, true).ToList();
                flag = true;
            }
            else
            {
                combinedItems = _tvShowsService.GetFilteredTvShows(genre, country, language, year).ToList();
            }

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new TvShowVM()
            {
                TvShows = pagedItems,
                FromHome = flag,
                Arabic = isArabic,
                Ramadan = isRamadan
            };

            return PartialView("_TvShowsItems", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LikeTvShow(int TvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _tvShowsService.LikeTvShow(TvShowId, userId);

            var viewModel = new TvShowDetailsVM()
            {
                TvShow = result.TvShow,
                RelatedTvShows = result.RelatedTvShows,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View("Details", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DislikeTvShow(int TvShowId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _tvShowsService.DisLikeTvShow(TvShowId, userId);

            var viewModel = new TvShowDetailsVM()
            {
                TvShow = result.TvShow,
                RelatedTvShows = result.RelatedTvShows,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View("Details", viewModel);
        }
    }
}