using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PresentationLayer.ViewModels;

namespace PresentationLayer.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IFilmsService _filmsService;
        private readonly ITvShowsService _tvShowsService;
        private readonly IEpisodesService _episodesService;
        private readonly ICategoriesService _categoriesService;
        private readonly ICountriesService _countriesService;
        private readonly IActorsService _actorsService;
        private readonly IProducersService _producersService;

        public HomeController(ITvShowsService tvShowsService, IFilmsService filmsService, ICategoriesService categoriesService, ICountriesService countriesService, IEpisodesService episodesService, IActorsService actorsService, IProducersService producersService)
        {
            _tvShowsService = tvShowsService;
            _filmsService = filmsService;
            _categoriesService = categoriesService;
            _countriesService = countriesService;
            _episodesService = episodesService;
            _actorsService = actorsService;
            _producersService = producersService;
        }

        public async Task CreateCategoriesSelectList()
        {
            var categories = await _categoriesService.GetAllCategories(1, int.MaxValue);

            var data = categories.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");

            ViewBag.MyBag3 = List;
        }

        public async Task CreateCountriesSelectList()
        {
            var countries = await _countriesService.GetAllCountries(1, int.MaxValue);

            var data = countries.OrderBy(x => x.Name).ToList();
            SelectList List = new SelectList(data, "Id", "Name");

            ViewBag.MyBag4 = List;
        }

        public IActionResult Index()
        {
            var MostWatchedFilms = _filmsService.GetFilms(Keys.NoOfLikes, null, ItemType.فيلم).Take(10);
            var MostWatchedTvShows = _tvShowsService.GetTvShows(Keys.NoOfLikes, null, ItemType.مسلسل, false).Take(10);
            var RecentEpisodes = _episodesService.GetRecentEpisodes(1).Episodes.Take(30);
            var RecentFilms = _filmsService.GetFilms(Keys.Year, null, ItemType.فيلم).Take(10);
            var RecentTvShows = _tvShowsService.GetTvShows(Keys.Year, null, ItemType.مسلسل, false).Take(10);
            var ArabicFilms = _filmsService.GetFilms(Keys.NoOfLikes, Languages.عربي, null).Take(10);
            var ArabicTvShows = _tvShowsService.GetTvShows(null, Languages.عربي, ItemType.مسلسل, false).Take(10);
            var CartoonFilms = _filmsService.GetFilms(Keys.NoOfLikes, null, ItemType.كرتون).Take(10);
            var RamadanTvShows = _tvShowsService.GetTvShows(null, Languages.عربي, ItemType.مسلسل, true).Take(10);

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

        public async Task<IActionResult> Recent(int page = 1, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;

            List<ItemViewModel> combinedItems = new List<ItemViewModel>();

            if (isFilms && !isTvShows)
            {
                combinedItems = _filmsService.GetRelatedFilmsByKey(Keys.Year)
                               .Select(f => new ItemViewModel { Type = "Film", Item = f })
                               .ToList();
                flag = true;
            }
            else if (isTvShows && !isFilms)
            {
                combinedItems = _tvShowsService.GetRelatedTvShowsByKey(Keys.Year)
                           .Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                           .ToList();
                flag = true;
            }
            else
            {
                combinedItems = _filmsService.GetRelatedFilmsByKey(Keys.Year)
                          .Select(f => new ItemViewModel { Type = "Film", Item = f })
                          .Concat(_tvShowsService.GetRelatedTvShowsByKey(Keys.Year)
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

            await CreateCountriesSelectList();
            await CreateCategoriesSelectList();

            return View(viewModel);
        }

        public IActionResult LoadMoreRecent(int page, string genre, string country, int? language, int? type, bool fromHome = false, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();

            if (isTvShows && !isFilms)
            {
                var tvshows = _tvShowsService.GetRelatedTvShowsByKey(Keys.Year).AsQueryable();

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

                combinedItems = tvshows.Select(f => new ItemViewModel { Type = "TvShow", Item = f }).ToList();
                flag = true;
            }
            else if (isFilms && !isTvShows)
            {
                var films = _filmsService.GetRelatedFilmsByKey(Keys.Year).AsQueryable();

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

                combinedItems = films.Select(t => new ItemViewModel { Type = "Film", Item = t }).ToList();
                flag = true;
            }
            else
            {
                var films = _filmsService.GetRelatedFilmsByKey(Keys.Year).AsQueryable();
                var tvShows = _tvShowsService.GetRelatedTvShowsByKey(Keys.Year).AsQueryable();

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
                    .Concat(tvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t })).ToList();
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

        public async Task<IActionResult> TopRated(int page = 1, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();
            if (isFilms && !isTvShows)
            {
                combinedItems = _filmsService.GetRelatedFilmsByKey(Keys.NoOfLikes)
                               .Select(f => new ItemViewModel { Type = "Film", Item = f })
                               .ToList();
                flag = true;
            }
            else if (isTvShows && !isFilms)
            {
                combinedItems = _tvShowsService.GetRelatedTvShowsByKey(Keys.NoOfLikes)
                           .Select(f => new ItemViewModel { Type = "TvShow", Item = f })
                           .ToList();
                flag = true;
            }
            else
            {
                combinedItems = _filmsService.GetRelatedFilmsByKey(Keys.NoOfLikes)
                          .Select(f => new ItemViewModel { Type = "Film", Item = f })
                          .Concat(_tvShowsService.GetRelatedTvShowsByKey(Keys.NoOfLikes)
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

            await CreateCountriesSelectList();
            await CreateCategoriesSelectList();

            return View(viewModel);
        }

        public IActionResult LoadMoreTopRated(int page, string genre, string country, int? language, int? type, bool fromHome = false, bool isFilms = false, bool isTvShows = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<ItemViewModel> combinedItems = new List<ItemViewModel>();

            if (isTvShows && !isFilms)
            {
                var tvshows = _tvShowsService.GetRelatedTvShowsByKey(Keys.NoOfLikes).AsQueryable();

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

                combinedItems = tvshows.Select(f => new ItemViewModel { Type = "TvShow", Item = f }).ToList();
                flag = true;
            }
            else if (isFilms && !isTvShows)
            {
                var films = _filmsService.GetRelatedFilmsByKey(Keys.NoOfLikes).AsQueryable();

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

                combinedItems = films.Select(t => new ItemViewModel { Type = "Film", Item = t }).ToList();
                flag = true;
            }
            else
            {
                var films = _filmsService.GetRelatedFilmsByKey(Keys.NoOfLikes).AsQueryable();
                var tvShows = _tvShowsService.GetRelatedTvShowsByKey(Keys.NoOfLikes).AsQueryable();

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
                    .Concat(tvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t })).ToList();
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
            var producers = _producersService.GetProducersForSearch(searchString);

            var actors = _actorsService.GetActorsForSearch(searchString);

            var films = _filmsService.GetFilmsForSearch(searchString);

            var tvshows = _tvShowsService.GetTvShowsForSearch(searchString);

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