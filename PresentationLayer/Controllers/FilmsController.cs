using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;
using System.Security.Claims;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FilmsController : Controller
    {
        private readonly IFilmsService _filmsService;

        public FilmsController(IFilmsService filmsService)
        {
            _filmsService = filmsService;
        }

        private async Task CreateNeededSelectLists()
        {
            var allNeededData = await _filmsService.GetFilmDataForSelectLists();

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
            IEnumerable<Film> result;

            if (!sortDate && !sortLikes)
            {
                result = await _filmsService.GetAllFilms(pageNumber, pageSize);
            }
            else if (sortLikes && !sortDate)
            {
                result = await _filmsService.GetAllFilmsOrderedByKey(pageNumber, pageSize, Keys.NoOfLikes);
            }
            else
            {
                result = await _filmsService.GetAllFilmsOrderedByKey(pageNumber, pageSize, Keys.Year);
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
        public async Task<IActionResult> Add(FilmDTO film)
        {
            if (ModelState.IsValid)
            {
                var result = await _filmsService.AddFilm(film);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(film);
            }
            else
            {
                return View(film);
            }
        }

        public async Task<IActionResult> Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var movie = await _filmsService.GetFilmByID(Id);

            int languageId = movie.Language.Value;
            Languages languageEnum = (Languages)languageId;

            int typeId = movie.Type.Value;
            ItemType typeEnum = (ItemType)typeId;

            if (movie == null)
                return NotFound();

            var result = new FilmViewModel()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                dbImage = movie.dbImage,
                IsSeries = movie.IsSeries,
                PartsNo = movie.PartsNo,
                Root = movie.Root,
                NoOfLikes = movie.NoOfLikes,
                Year = movie.Year,
                Month = movie.Month,
                Type = typeEnum,
                Language = languageEnum,
                CountryId = movie.CountryId,
                ProducerId = movie.ProducerId,
                CategoryId = movie.CategoryId,
                ActorsId = movie.ActorFilms.Select(a => a.ActorId).ToList()
            };

            await this.CreateNeededSelectLists();

            return View(result);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(FilmDTO film)
        {
            if (ModelState.IsValid)
            {
                var result = await _filmsService.UpdateFilm(film);

                return result.Success ? RedirectToAction(nameof(Index)) :
                    View(film);
            }
            else
            {
                return View(film);
            }
        }

        public async Task<IActionResult> Delete(int Id)
        {

            if (Id == null || Id == 0)
                return NotFound();

            var movie = await _filmsService.GetFilmByID(Id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Film film)
        {
            var result = _filmsService.DeleteFilm(film);

            return result.Success ? RedirectToAction(nameof(Index)) :
                View(film);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null || id == 0)
                return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var film = await _filmsService.FilmDetails(id, true, userID);

                var data = new FilmDetailsVM()
                {
                    Film = film.Film,
                    HasUserDisliked = film.HasUserDisliked,
                    HasUserLiked = film.HasUserLiked,
                    RelatedFilms = film.RelatedFilms
                };

                return View(data);
            }
            else
            {
                var film = await _filmsService.FilmDetails(id, false, null);

                var data = new FilmDetailsVM()
                {
                    Film = film.Film,
                    HasUserDisliked = film.HasUserDisliked,
                    HasUserLiked = film.HasUserLiked,
                    RelatedFilms = film.RelatedFilms
                };

                return View(data);
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> FilmsList(int page = 1, bool isArabic = false, bool isCartoon = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<Film> films = new List<Film>();

            if (isArabic && !isCartoon)
            {
                films = _filmsService.GetFilms(null, Languages.عربي, null).ToList();
                flag = true;
            }
            else if (isCartoon && !isArabic)
            {
                films = _filmsService.GetFilms(null, null, ItemType.كرتون).ToList();
                flag = true;
            }
            else
            {
                films = _filmsService.GetFilms(null, null, null).ToList();
            }

            int totalItems = films.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedItems = films.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new FilmVM
            {
                Films = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages,
                FromHome = flag,
                Arabic = isArabic,
                Cartoon = isCartoon
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
        public IActionResult LoadMoreFilms(int page, string genre, string country, int? language, int? year, bool fromHome = false, bool isArabic = false, bool isCartoon = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<Film> combinedItems = new List<Film>();

            if (isArabic && !isCartoon)
            {
                combinedItems = _filmsService.GetFilteredFilms(genre, country, language, year, true, false).ToList();
                flag = true;
            }
            else if (isCartoon && !isArabic)
            {
                combinedItems = _filmsService.GetFilteredFilms(genre, country, language, year, false, true).ToList();
                flag = true;
            }
            else
            {
                combinedItems = _filmsService.GetFilteredFilms(genre, country, language, year).ToList();
            }

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new FilmVM()
            {
                Films = pagedItems,
                FromHome = flag,
                Arabic = isArabic,
                Cartoon = isCartoon
            };

            return PartialView("_FilmItems", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LikeFilm(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _filmsService.LikeFilm(filmId, userId);

            var viewModel = new FilmDetailsVM()
            {
                Film = result.Film,
                RelatedFilms = result.RelatedFilms,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View("Details", viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> DislikeFilm(int filmId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _filmsService.DisLikeFilm(filmId, userId);

            var viewModel = new FilmDetailsVM()
            {
                Film = result.Film,
                RelatedFilms = result.RelatedFilms,
                HasUserLiked = result.HasUserLiked,
                HasUserDisliked = result.HasUserDisliked
            };

            return View("Details", viewModel);
        }
    }
}