using DataAccessLayer.Enums;
using DataAccessLayer.Models.ViewModels;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
        public IActionResult Index()
        {
            var MostWatchedFilms = _unitOfWork.Films.MostWatchedFilms().Take(10);
            var MostWatchedTvShows = _unitOfWork.TvShows.MostWatchedTvShows().Take(10);
            var RecentEpisodes = _unitOfWork.Episodes.GetRecentEpisodes().Take(30);
            var RecentFilms = _unitOfWork.Films.RecentFilms().Take(10);
            var RecentTvShows = _unitOfWork.TvShows.RecentTvShows().Take(10);
            var ArabicFilms = _unitOfWork.Films.ArabicFilms().Take(10);
            var ArabicTvShows = _unitOfWork.TvShows.ArabicTvShows().Take(10);
            var CartoonFilms = _unitOfWork.Films.CartoonFilms().Take(10);
            var RamadanTvShows = _unitOfWork.TvShows.RamadanTvShows().Take(10);
            var data = new HomePageVM()
            {
                MostWatchedFilms = MostWatchedFilms,
                MostWatchedTvShows = MostWatchedTvShows,
                RecentEpisodes = RecentEpisodes,
                RecentFilms = RecentFilms,
                RecentTvShows = RecentTvShows,
                ArabicFilms = ArabicFilms,
                ArabicTvShows = ArabicTvShows,
                CartoonFilms = CartoonFilms,
                RamadanTvShows = RamadanTvShows
            };
            return View(data);
        }
        public IActionResult Recent(int page = 1, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();
            if (isFilms && !isTvShows)
            {
                combinedItems = _unitOfWork.Films.GetRecentFilms(page)
                               .Select(f => new ItemViewModel { Type = "Film", Item = f })
                               .ToList();
                flag = true;
            }
            else if (isTvShows && !isFilms)
            {
                combinedItems = _unitOfWork.TvShows.GetRecentTvShows(page)
                           .Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                           .ToList();
                flag = true;
            }
            else
            {
                combinedItems = _unitOfWork.Films.GetRecentFilms(page)
                          .Select(f => new ItemViewModel { Type = "Film", Item = f })
                          .Concat(_unitOfWork.TvShows.GetRecentTvShows(page)
                          .Select(t => new ItemViewModel { Type = "TvShow", Item = t }))
                          .ToList();
            }

            int totalItems = combinedItems.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new WorksVM()
            {
                Items = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages,
                FromHome = flag,
                Film = isFilms,
                TvShow = isTvShows
            };

            var enumValues1 = Enum.GetValues(typeof(Languages))
                   .Cast<Languages>()
                   .Select(e => new { Id = (int)e, Name = e.ToString() })
                   .ToList();
            SelectList enumSelectList1 = new SelectList(enumValues1, "Id", "Name");
            ViewBag.MyBag5 = enumSelectList1;

            var enumValues2 = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Select(e => new { Id = (int)e, Name = e.ToString() })
            .ToList();
            SelectList enumSelectList2 = new SelectList(enumValues2, "Id", "Name");
            ViewBag.MyBag6 = enumSelectList2;

            CreateCountriesSelectList();
            CreateCategoriesSelectList();

            return View(viewModel);
        }
        public IActionResult LoadMoreRecent(int page, string genre, string country, int? language, int? type, bool fromHome = false, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();

            if (isTvShows && !isFilms)
            {
                var tvshows = _unitOfWork.TvShows.GetRecentTvShows(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    tvshows = tvshows.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    tvshows = tvshows.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    tvshows = tvshows.Where(f => f.Language == language);
                }
                combinedItems = tvshows.Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                                .ToList();
                flag = true;
            }
            else if (isFilms && !isTvShows)
            {
                var films = _unitOfWork.Films.GetRecentFilms(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    films = films.Where(t => t.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    films = films.Where(t => t.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    films = films.Where(t => t.Language == language);
                }
                combinedItems = films.Select(t => new ItemViewModel { Type = "Film", Item = t })
                                .ToList();
                flag = true;
            }
            else
            {
                var films = _unitOfWork.Films.GetRecentFilms(page).AsQueryable();
                var tvShows = _unitOfWork.TvShows.GetRecentTvShows(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    films = films.Where(f => f.CategoryId == genreId);
                    tvShows = tvShows.Where(t => t.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    films = films.Where(f => f.CountryId == countryId);
                    tvShows = tvShows.Where(t => t.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    films = films.Where(f => f.Language == language);
                    tvShows = tvShows.Where(t => t.Language == language);
                }

                if (type.HasValue && type.Value > 0)
                {
                    films = films.Where(f => f.Type == type);
                    tvShows = tvShows.Where(t => t.Type == type);
                }

                combinedItems = films.Select(f => new ItemViewModel { Type = "Film", Item = f })
                                .Concat(tvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t }))
                                .ToList();
            }

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new WorksVM()
            {
                Items = pagedItems,
                FromHome = flag,
                Film = isFilms,
                TvShow = isTvShows
            };

            return PartialView("_Recent", viewModel);
        }
        public IActionResult TopRated(int page = 1, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();
            if (isFilms && !isTvShows)
            {
                combinedItems = _unitOfWork.Films.GetTopRatedFilms(page)
                               .Select(f => new ItemViewModel { Type = "Film", Item = f })
                               .ToList();
                flag = true;
            }
            else if (isTvShows && !isFilms)
            {
                combinedItems = _unitOfWork.TvShows.GetTopRatedTvShows(page)
                           .Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                           .ToList();
                flag = true;
            }
            else
            {
                combinedItems = _unitOfWork.Films.GetTopRatedFilms(page)
                          .Select(f => new ItemViewModel { Type = "Film", Item = f })
                          .Concat(_unitOfWork.TvShows.GetTopRatedTvShows(page)
                          .Select(t => new ItemViewModel { Type = "TvShow", Item = t }))
                          .ToList();
            }

            int totalItems = combinedItems.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new WorksVM()
            {
                Items = pagedItems,
                CurrentPage = page,
                TotalPages = totalPages,
                FromHome = flag,
                Film = isFilms,
                TvShow = isTvShows
            };

            var enumValues1 = Enum.GetValues(typeof(Languages))
                   .Cast<Languages>()
                   .Select(e => new { Id = (int)e, Name = e.ToString() })
                   .ToList();
            SelectList enumSelectList1 = new SelectList(enumValues1, "Id", "Name");
            ViewBag.MyBag5 = enumSelectList1;

            var enumValues2 = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Select(e => new { Id = (int)e, Name = e.ToString() })
            .ToList();
            SelectList enumSelectList2 = new SelectList(enumValues2, "Id", "Name");
            ViewBag.MyBag6 = enumSelectList2;

            CreateCountriesSelectList();
            CreateCategoriesSelectList();

            return View(viewModel);
        }
        public IActionResult LoadMoreTopRated(int page, string genre, string country, int? language, int? type, bool fromHome = false, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();

            if (isTvShows && !isFilms)
            {
                var tvshows = _unitOfWork.TvShows.GetTopRatedTvShows(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    tvshows = tvshows.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    tvshows = tvshows.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    tvshows = tvshows.Where(f => f.Language == language);
                }
                combinedItems = tvshows.Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                                .ToList();
                flag = true;
            }
            else if (isFilms && !isTvShows)
            {
                var films = _unitOfWork.Films.GetTopRatedFilms(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    films = films.Where(t => t.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    films = films.Where(t => t.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    films = films.Where(t => t.Language == language);
                }
                combinedItems = films.Select(t => new ItemViewModel { Type = "Film", Item = t })
                                .ToList();
                flag = true;
            }
            else
            {
                var films = _unitOfWork.Films.GetTopRatedFilms(page).AsQueryable();
                var tvShows = _unitOfWork.TvShows.GetTopRatedTvShows(page).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    films = films.Where(f => f.CategoryId == genreId);
                    tvShows = tvShows.Where(t => t.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    films = films.Where(f => f.CountryId == countryId);
                    tvShows = tvShows.Where(t => t.CountryId == countryId);
                }

                if (language.HasValue && language.Value > 0)
                {
                    films = films.Where(f => f.Language == language);
                    tvShows = tvShows.Where(t => t.Language == language);
                }

                if (type.HasValue && type.Value > 0)
                {
                    films = films.Where(f => f.Type == type);
                    tvShows = tvShows.Where(t => t.Type == type);
                }

                combinedItems = films.Select(f => new ItemViewModel { Type = "Film", Item = f })
                                .Concat(tvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t }))
                                .ToList();
            }

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new WorksVM()
            {
                Items = pagedItems,
                FromHome = flag,
                Film = isFilms,
                TvShow = isTvShows
            };

            return PartialView("_TopRated", viewModel);
        }
        public IActionResult Filter(string searchString)
        {
            var producers = _unitOfWork.Producers.GetProducersForSearch(searchString);

            var actors = _unitOfWork.Actors.GetActorsForSearch(searchString);

            var films = _unitOfWork.Films.GetFilmsForSearch(searchString);

            var tvshows = _unitOfWork.TvShows.GetTvShowsForSearch(searchString);

            var viewModel = new SearchVM()
            {
                Producers = producers,
                Actors = actors,
                Films = films,
                TvShows = tvshows
            };

            return View(viewModel);
        }
    }
}