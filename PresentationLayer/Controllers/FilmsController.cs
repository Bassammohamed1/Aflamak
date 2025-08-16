using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Repository.Interfaces;
using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using DataAccessLayer.Enums;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FilmsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public FilmsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void CreateProducersSelectList()
        {
            var producers = _unitOfWork.Producers.GetAllWithoutPagination();
            var data = producers.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");
            ViewBag.MyBag1 = List;
        }
        public void CreateActorsSelectList()
        {
            var actors = _unitOfWork.Actors.GetAllWithoutPagination();
            var data = actors.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");
            ViewBag.MyBag2 = List;
        }
        public void CreateCategoriesSelectList()
        {
            var categories = _unitOfWork.Categories.GetAllWithoutPagination();
            var data = categories.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");
            ViewBag.MyBag3 = List;
        }
        public void CreateCountriesSelectList()
        {
            var countries = _unitOfWork.Countries.GetAllWithoutPagination();
            var data = countries.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");
            ViewBag.MyBag4 = List;
        }
        public IActionResult Index(int? page, bool sortLikes = false, bool sortDate = false)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            IEnumerable<Film> result;

            if (!sortDate && !sortLikes)
            {
                result = _unitOfWork.Films.GetAllFilms(pageNumber, pageSize);
            }
            else if (sortLikes && !sortDate)
            {
                result = _unitOfWork.Films.GetAllFilmsOrderedByLikes(pageNumber, pageSize);
            }
            else
            {
                result = _unitOfWork.Films.GetAllFilmsOrderedByDate(pageNumber, pageSize);
            }

            ViewBag.SortLikes = sortLikes;
            ViewBag.SortDate = sortDate;
            ViewBag.SortLanguage = sortDate;

            return View(result);
        }
        public IActionResult Add()
        {
            CreateProducersSelectList();
            CreateActorsSelectList();
            CreateCategoriesSelectList();
            CreateCountriesSelectList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(FilmViewModel movie)
        {
            if (ModelState.IsValid)
            {
                if (movie.clientFile != null)
                {
                    var stream = new MemoryStream();
                    movie.clientFile.CopyTo(stream);
                    movie.dbImage = stream.ToArray();
                }
                _unitOfWork.Films.AddFilm(movie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(movie);
            }
        }
        public IActionResult Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var movie = _unitOfWork.Films.GetFilmById(Id);

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
            CreateProducersSelectList();
            CreateActorsSelectList();
            CreateCategoriesSelectList();
            CreateCountriesSelectList();
            return View(result);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(FilmViewModel movie)
        {
            if (ModelState.IsValid)
            {
                var data = _unitOfWork.Films.GetById(movie.Id);
                if (data.clientFile != movie.clientFile)
                {
                    var stream = new MemoryStream();
                    movie.clientFile.CopyTo(stream);
                    movie.dbImage = stream.ToArray();
                }
                _unitOfWork.Films.UpdateFilm(movie);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(movie);
            }
        }
        public IActionResult Delete(int Id)
        {

            if (Id == null || Id == 0)
                return NotFound();
            var movie = _unitOfWork.Films.GetById(Id);
            if (movie == null)
                return NotFound();
            return View(movie);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Film movie)
        {
            _unitOfWork.Films.Delete(movie);
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

                var film = _unitOfWork.Films.GetFilmById(id);
                if (film == null)
                    return NotFound();

                var films = _unitOfWork.Films.GetFilms().ToList();
                films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

                if (interaction is not null)
                {
                    var viewModel = new FilmDetailsVM()
                    {
                        Film = film,
                        RelatedFilms = films,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return View(viewModel);
                }
                else
                {
                    var viewModel = new FilmDetailsVM()
                    {
                        Film = film,
                        RelatedFilms = films,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return View(viewModel);
                }
            }
            else
            {
                var film = _unitOfWork.Films.GetFilmById(id);
                if (film == null)
                    return NotFound();

                var films = _unitOfWork.Films.GetFilms().ToList();
                films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

                var viewModel = new FilmDetailsVM()
                {
                    Film = film,
                    RelatedFilms = films,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return View(viewModel);
            }
        }
        [AllowAnonymous]
        public IActionResult FilmsList(int page = 1, bool isArabic = false, bool isCartoon = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<Film> films = new List<Film>();

            if (isArabic && !isCartoon)
            {
                films = _unitOfWork.Films.ArabicFilms().ToList();
                flag = true;
            }
            else if (isCartoon && !isArabic)
            {
                films = _unitOfWork.Films.CartoonFilms().ToList();
                flag = true;
            }
            else
            {
                films = _unitOfWork.Films.GetFilms().ToList();
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
                        .Cast<Languages>()
                        .Select(e => new { Id = (int)e, Name = e.ToString() })
                        .ToList();
            SelectList enumSelectList = new SelectList(enumValues, "Id", "Name");
            ViewBag.MyBag5 = enumSelectList;

            CreateCountriesSelectList();
            CreateCategoriesSelectList();

            List<int> dates = new List<int>();
            for (int i = 1950; i <= DateTime.Now.Year; i++)
            {
                dates.Add(i);
            }
            var Dates = dates.OrderByDescending(x => x)
                .Select(n => new { Value = n, Text = n.ToString() }).ToList();
            ViewBag.MyBag6 = Dates;

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
                combinedItems = _unitOfWork.Films.GetFilteredFilms(genre, country, language, year, true, false).ToList();
                flag = true;
            }
            else if (isCartoon && !isArabic)
            {
                combinedItems = _unitOfWork.Films.GetFilteredFilms(genre, country, language, year, false, true).ToList();
                flag = true;
            }
            else
            {
                combinedItems = _unitOfWork.Films.GetFilteredFilms(genre, country, language, year).ToList();
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
        public IActionResult LikeFilm(int filmId)
        {
            Film film = new Film();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == filmId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = filmId,
                    IsLiked = true,
                    IsDisLiked = false
                };
                _unitOfWork.Interactions.Add(interaction);

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfLikes += 1;
                film.NoOfDisLikes -= 1;
            }
            else if(interaction.IsLiked)
            {
                interaction.IsLiked = false;

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            _unitOfWork.SaveChanges();

            var films = _unitOfWork.Films.GetFilms().ToList();
            films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

            var viewModel = new FilmDetailsVM()
            {
                Film = film,
                RelatedFilms = films,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
        [AllowAnonymous]
        public IActionResult DislikeFilm(int filmId)
        {
            Film film = new Film();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == filmId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = filmId,
                    IsLiked = false,
                    IsDisLiked = true
                };
                _unitOfWork.Interactions.Add(interaction);

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfDisLikes += 1;
                film.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                film = _unitOfWork.Films.GetById(filmId);
                film.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            _unitOfWork.SaveChanges();

            var films = _unitOfWork.Films.GetFilms().ToList();
            films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

            var viewModel = new FilmDetailsVM()
            {
                Film = film,
                RelatedFilms = films,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
    }
}