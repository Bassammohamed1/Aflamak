using Aflamak.Models.ViewModels;
using Aflamak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aflamak.Repository.Interfaces;
using Aflamak.Data.Enums;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Aflamak.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TvShowsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TvShowsController(IUnitOfWork unitOfWork)
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
            int pageNumber = (page ?? 1);
            IEnumerable<TvShow> result;

            if (!sortDate && !sortLikes)
            {
                result = _unitOfWork.TvShows.GetAllTvShows(pageNumber, pageSize);
            }
            else if (sortLikes && !sortDate)
            {
                result = _unitOfWork.TvShows.GetAllTvShowsOrderedByLikes(pageNumber, pageSize);
            }
            else
            {
                result = _unitOfWork.TvShows.GetAllTvShowsOrderedByDate(pageNumber, pageSize);
            }

            ViewBag.SortLikes = sortLikes;
            ViewBag.SortDate = sortDate;

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
        public IActionResult Add(TvShowViewModel TvShow)
        {
            if (ModelState.IsValid)
            {
                if (TvShow.clientFile != null)
                {
                    var stream = new MemoryStream();
                    TvShow.clientFile.CopyTo(stream);
                    TvShow.dbImage = stream.ToArray();
                }
                _unitOfWork.TvShows.AddTvShow(TvShow);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(TvShow);
            }
        }
        public IActionResult Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();

            var TvShow = _unitOfWork.TvShows.GetTvShowById(Id);

            int languageId = (int)TvShow.Language.Value;
            Languages languageEnum = (Languages)languageId;

            int typeId = TvShow.Type.Value;
            ItemType typeEnum = (ItemType)typeId;

            if (TvShow == null)
                return NotFound();

            var result = new TvShowViewModel()
            {
                Id = TvShow.Id,
                Name = TvShow.Name,
                Description = TvShow.Description,
                dbImage = TvShow.dbImage,
                IsSeries = TvShow.IsSeries,
                PartsNo = TvShow.PartsNo,
                NoOfLikes = TvShow.NoOfLikes,
                NoOfDisLikes = TvShow.NoOfDisLikes,
                Year = TvShow.Year,
                Month = TvShow.Month,
                Type = typeEnum,
                Language = languageEnum,
                CountryId = TvShow.CountryId,
                ProducerId = TvShow.ProducerId,
                CategoryId = TvShow.CategoryId,
                ActorsId = TvShow.ActorTvShows.Select(a => a.ActorId).ToList()
            };
            CreateProducersSelectList();
            CreateActorsSelectList();
            CreateCategoriesSelectList();
            CreateCountriesSelectList();
            return View(result);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(TvShowViewModel TvShow)
        {
            if (ModelState.IsValid)
            {
                var data = _unitOfWork.TvShows.GetById(TvShow.Id);
                if (data.clientFile != TvShow.clientFile)
                {
                    var stream = new MemoryStream();
                    TvShow.clientFile.CopyTo(stream);
                    TvShow.dbImage = stream.ToArray();
                }
                _unitOfWork.TvShows.UpdateTvShow(TvShow);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(TvShow);
            }
        }
        public IActionResult Delete(int Id)
        {

            if (Id == null || Id == 0)
                return NotFound();
            var TvShow = _unitOfWork.TvShows.GetById(Id);
            if (TvShow == null)
                return NotFound();
            return View(TvShow);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(TvShow TvShow)
        {
            _unitOfWork.TvShows.Delete(TvShow);
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

                var tvshow = _unitOfWork.TvShows.GetById(id);
                if (tvshow == null)
                    return NotFound();

                var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
                tvshows = tvshows.Where(t => (t.Producer.Id == tvshow.ProducerId && t.Type == tvshow.Type) || (t.CategoryId == tvshow.CategoryId && t.Type == tvshow.Type && t.Language == tvshow.Language)).Take(10).ToList();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvshow.Id).ToList();

                if (interaction is not null)
                {
                    var viewModel = new TvShowDetailsVM()
                    {
                        TvShow = tvshow,
                        RelatedTvShows = tvshows,
                        TvShowParts = parts,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return View(viewModel);
                }
                else
                {
                    var viewModel = new TvShowDetailsVM()
                    {
                        TvShow = tvshow,
                        RelatedTvShows = tvshows,
                        TvShowParts = parts,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return View(viewModel);
                }
            }
            else
            {
                var tvshow = _unitOfWork.TvShows.GetById(id);
                if (tvshow == null)
                    return NotFound();

                var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
                tvshows = tvshows.Where(t => (t.Producer.Id == tvshow.ProducerId && t.Type == tvshow.Type) || (t.CategoryId == tvshow.CategoryId && t.Type == tvshow.Type && t.Language == tvshow.Language)).Take(10).ToList();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvshow.Id).ToList();

                var viewModel = new TvShowDetailsVM()
                {
                    TvShow = tvshow,
                    RelatedTvShows = tvshows,
                    TvShowParts = parts,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return View(viewModel);
            }
        }
        [AllowAnonymous]
        public IActionResult TvShowsList(int page = 1, bool isArabic = false, bool isRamadan = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<TvShow> tvshows = new List<TvShow>();

            if (isArabic && !isRamadan)
            {
                tvshows = _unitOfWork.TvShows.ArabicTvShows().ToList();
                flag = true;
            }
            else if (isRamadan && !isArabic)
            {
                tvshows = _unitOfWork.TvShows.RamadanTvShows().ToList();
                flag = true;
            }
            else
            {
                tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
            }

            int totalItems = tvshows.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var pagedItems = tvshows.Skip((page - 1) * pageSize).Take(pageSize).ToList();

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
        public IActionResult LoadMoreTvShows(int page, string genre, string country, int? language, int? year, bool fromHome = false, bool isArabic = false, bool isCartoon = false)
        {
            int pageSize = 9;
            bool flag = false;
            List<TvShow> combinedItems = new List<TvShow>();

            if (isArabic && !isCartoon)
            {
                combinedItems = _unitOfWork.TvShows.GetFilteredTvShows(genre, country, language, year, true, false).ToList();
                flag = true;
            }
            else if (isCartoon && !isArabic)
            {
                combinedItems = _unitOfWork.TvShows.GetFilteredTvShows(genre, country, language, year, false, true).ToList();
                flag = true;
            }
            else
            {
                combinedItems = _unitOfWork.TvShows.GetFilteredTvShows(genre, country, language, year).ToList();
            }

            var pagedItems = combinedItems.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var viewModel = new TvShowVM()
            {
                TvShows = pagedItems,
                FromHome = flag,
                Arabic = isArabic,
                Ramadan = isCartoon
            };

            return PartialView("_TvShowsItems", viewModel);
        }
        [AllowAnonymous]
        public IActionResult LikeTvShow(int tvshowId)
        {
            TvShow tvshow = new TvShow();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == tvshowId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = tvshowId,
                    IsLiked = true,
                    IsDisLiked = false
                };
                _unitOfWork.Interactions.Add(interaction);

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfLikes += 1;
                tvshow.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            _unitOfWork.SaveChanges();

            tvshow = _unitOfWork.TvShows.GetById(tvshowId);

            var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
            tvshows = tvshows.Where(t => (t.Producer.Id == tvshow.ProducerId && t.Type == tvshow.Type) || (t.CategoryId == tvshow.CategoryId && t.Type == tvshow.Type && t.Language == tvshow.Language)).Take(10).ToList();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvshow.Id).ToList();

            var viewModel = new TvShowDetailsVM()
            {
                TvShow = tvshow,
                RelatedTvShows = tvshows,
                TvShowParts = parts,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details",viewModel);
        }
        [AllowAnonymous]
        public IActionResult DisLikeTvShow(int tvshowId)
        {
            TvShow tvshow = new TvShow();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination()
                .FirstOrDefault(fi => fi.ItemId == tvshowId && fi.UserId == userId);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userId,
                    ItemId = tvshowId,
                    IsLiked = false,
                    IsDisLiked = true
                };
                _unitOfWork.Interactions.Add(interaction);

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfDisLikes += 1;
                tvshow.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                tvshow = _unitOfWork.TvShows.GetById(tvshowId);
                tvshow.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            _unitOfWork.SaveChanges();

            tvshow = _unitOfWork.TvShows.GetById(tvshowId);

            var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
            tvshows = tvshows.Where(t => (t.Producer.Id == tvshow.ProducerId && t.Type == tvshow.Type) || (t.CategoryId == tvshow.CategoryId && t.Type == tvshow.Type && t.Language == tvshow.Language)).Take(10).ToList();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvshow.Id).ToList();

            var viewModel = new TvShowDetailsVM()
            {
                TvShow = tvshow,
                RelatedTvShows = tvshows,
                TvShowParts = parts,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return View("Details", viewModel);
        }
    }
}