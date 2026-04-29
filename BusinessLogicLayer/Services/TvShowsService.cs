using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class TvShowsService : ITvShowsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TvShowsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TvShow> GetTvShowByID(int id)
        {
            return await _unitOfWork.TvShows.GetTvShowById(id);
        }

        public IEnumerable<TvShow> GetTvShows(Keys? key, Languages? language, ItemType? itemType, bool isRamadan)
        {
            if (key is not null && language is null && itemType == ItemType.مسلسل && !isRamadan)
                return _unitOfWork.TvShows.GetTvShows().AsQueryable()
                    .OrderByDescending(x => EF.Property<object>(x, key.ToString()))
                    .ThenByDescending(x => key == Keys.Year ? x.Month : x.Id) 
                    .Where(m => m.Type == 2 && !m.IsRamadan);

            else if (language is Languages.عربي && itemType == ItemType.مسلسل && !isRamadan)
                return _unitOfWork.TvShows.GetTvShows()
                    .Where(m => m.Type == 2 && m.Language == 1 && !m.IsRamadan);

            else if (key is null && language is Languages.عربي && itemType == ItemType.مسلسل && isRamadan)
            {
                var data = new List<TvShow>();

                var tvshows1 = _unitOfWork.TvShows.GetTvShows()
                    .Where(m => m.Type == 2 && m.IsRamadan && m.Year == DateTime.Today.Year && m.Month < 8)
                    .OrderBy(x => x.Year);

                data.AddRange(tvshows1);

                var tvshows2 = _unitOfWork.TvShows.GetTvShows()
                    .Where(m => m.Type == 2 && m.IsRamadan && (m.Year == DateTime.Today.Year || m.Year == DateTime.Today.Year - 1) && m.Month >= 8)
                    .OrderBy(x => x.Year);

                data.AddRange(tvshows2);

                if (data.Any())
                {
                    data = data.OrderByDescending(x => x.Year).ToList();

                    int year = data.First().Year;

                    data = data.Where(t => t.Year == year).ToList();

                    return data.AsQueryable();
                }
                else
                {
                    return Enumerable.Empty<TvShow>().AsQueryable();
                }
            }

            else
                return _unitOfWork.TvShows.GetTvShows();
        }

        public async Task<IEnumerable<TvShow>> GetAllTvShows(int pageNumber, int pageSize)
        {
            return await _unitOfWork.TvShows.GetAllTvShows(pageNumber, pageSize);
        }

        public IQueryable<TvShow> GetFilteredTvShowsWithKey(int id, Keys key)
        {
            if (key == Keys.ID)
                return _unitOfWork.TvShows.GetFilteredTvShowsWithID(id);

            else if (key == Keys.Producer)
                return _unitOfWork.TvShows.GetFilteredTvShowsWithProducerID(id);

            else
                return Enumerable.Empty<TvShow>().AsQueryable();
        }

        public async Task<IEnumerable<TvShow>> GetAllTvShowsOrderedByKey(int pageNumber, int pageSize, Keys key)
        {
            return await _unitOfWork.TvShows.GetAllTvShowsOrderedByKey(pageNumber, pageSize, key);
        }

        public IQueryable<TvShow> GetFilteredTvShows(string genre, string country, int? language, int? year, bool isArabic = false, bool isRamadan = false)
        {
            if (isArabic && !isRamadan)
            {
                var query = GetTvShows(null, Languages.عربي, ItemType.مسلسل, false).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var TvShows = query;

                return TvShows;
            }
            else if (isRamadan)
            {
                var query = GetTvShows(null, Languages.عربي, ItemType.مسلسل, true).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var TvShows = query;

                return TvShows;
            }
            else
            {
                var query = GetTvShows(null, null, ItemType.مسلسل, false).AsQueryable();

                if (!string.IsNullOrEmpty(genre))
                {
                    int genreId = int.Parse(genre);
                    query = query.Where(f => f.CategoryId == genreId);
                }

                if (!string.IsNullOrEmpty(country))
                {
                    int countryId = int.Parse(country);
                    query = query.Where(f => f.CountryId == countryId);
                }

                if (language.HasValue && language.Value != 0)
                {
                    query = query.Where(f => f.Language == language);
                }

                if (year.HasValue)
                {
                    query = query.Where(f => f.Year == year);
                }

                var TvShows = query;

                return TvShows;
            }
        }

        public IQueryable<TvShow> GetRelatedTvShowsByKey(Keys key)
        {
            List<TvShow> data = new List<TvShow>();

            if (key == Keys.Year)
            {
                var result1 = _unitOfWork.TvShows.GetTvShows()
                    .OrderBy(f => f.Name).Where(f => f.Month < 8 && f.Year == DateTime.Now.Year).ToList();

                if (result1 is not null)
                    data.AddRange(result1);

                var result2 = _unitOfWork.TvShows.GetTvShows()
                    .OrderBy(f => f.Name).Where(f => f.Month >= 8 && (f.Year == DateTime.Now.Year || f.Year == DateTime.Now.Year - 1)).ToList();

                if (result2 is not null)
                    data.AddRange(result2);

                if (data.Any())
                    return data.AsQueryable();
            }
            else if (key == Keys.NoOfLikes)
            {
                var result = _unitOfWork.TvShows.GetTvShows()
                    .OrderBy(f => f.Name).Where(f => f.NoOfLikes > 5000).ToList();

                if (result.Any())
                {
                    data.AddRange(result);

                    return data.AsQueryable();
                }
            }

            return Enumerable.Empty<TvShow>().AsQueryable();
        }

        public async Task<Result> AddTvShow(TvShowDTO data)
        {
            if (data.clientFile != null)
            {
                var stream = new MemoryStream();
                await data.clientFile.CopyToAsync(stream);
                data.dbImage = stream.ToArray();

                int languageId = (int)data.Language;
                int typeId = (int)data.Type;

                var tvShow = new TvShow()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    dbImage = data.dbImage,
                    IsSeries = data.IsSeries,
                    IsRamadan = data.IsRamadan,
                    PartsNo = data.PartsNo,
                    NoOfLikes = data.NoOfLikes,
                    Year = data.Year,
                    Month = data.Month,
                    Type = typeId,
                    Language = languageId,
                    CountryId = data.CountryId,
                    CategoryId = data.CategoryId,
                    ProducerId = data.ProducerId,
                };

                var result = await _unitOfWork.TvShows.AddTvShow(tvShow, data.ActorsId);

                return result is not null ? new Result() { Success = true } :
                    new Result()
                    {
                        Success = false,
                        Error = "An error ouccered while adding."
                    };
            }

            return new Result()
            {
                Success = false,
                Error = "clientFile is missing."
            };
        }

        public async Task<Result> UpdateTvShow(TvShowDTO data)
        {
            if (data.clientFile != null)
            {
                var stream = new MemoryStream();
                await data.clientFile.CopyToAsync(stream);
                data.dbImage = stream.ToArray();

                int languageId = (int)data.Language;
                int typeId = (int)data.Type;

                var tvShow = new TvShow()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    dbImage = data.dbImage,
                    IsSeries = data.IsSeries,
                    IsRamadan = data.IsRamadan,
                    PartsNo = data.PartsNo,
                    NoOfLikes = data.NoOfLikes,
                    Year = data.Year,
                    Month = data.Month,
                    Type = typeId,
                    Language = languageId,
                    CountryId = data.CountryId,
                    CategoryId = data.CategoryId,
                    ProducerId = data.ProducerId,
                };

                var result = await _unitOfWork.TvShows.UpdateTvShow(tvShow, data.ActorsId);

                return result is not null ? new Result() { Success = true } :
                    new Result()
                    {
                        Success = false,
                        Error = "An error ouccered while updating."
                    };
            }

            return new Result()
            {
                Success = false,
                Error = "clientFile is missing."
            };
        }

        public Result DeleteTvShow(TvShow TvShow)
        {
            var result = _unitOfWork.TvShows.Delete(TvShow);

            return result is not null ? new Result() { Success = true } :
                new Result()
                {
                    Success = false,
                    Error = "An error ouccered while deleting."
                };
        }

        public async Task<TvShowDetailsDTO> TvShowDetails(int tvShowID, bool isAuthenticated, string? userID)
        {
            if (isAuthenticated)
            {
                var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                    .FirstOrDefault(fi => fi.ItemId == tvShowID && fi.UserId == userID);

                var tvShow = await _unitOfWork.TvShows.GetTvShowById(tvShowID);

                if (tvShow == null)
                    throw new NullReferenceException();

                var tvShows = _unitOfWork.TvShows.GetTvShows().ToList();
                tvShows = tvShows.Where(t => t.Producer.Id == tvShow.ProducerId && t.Type == tvShow.Type || t.CategoryId == tvShow.CategoryId && t.Type == tvShow.Type && t.Language == tvShow.Language).Take(10).ToList();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvShow.Id).ToList();

                if (interaction is not null)
                {
                    var viewModel = new TvShowDetailsDTO()
                    {
                        TvShow = tvShow,
                        RelatedTvShows = tvShows,
                        TvShowParts = parts,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return viewModel;
                }
                else
                {
                    var viewModel = new TvShowDetailsDTO()
                    {
                        TvShow = tvShow,
                        RelatedTvShows = tvShows,
                        TvShowParts = parts,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return viewModel;
                }
            }
            else
            {
                var tvShow = await _unitOfWork.TvShows.GetTvShowById(tvShowID);

                if (tvShow == null)
                    throw new NullReferenceException();

                var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
                tvshows = tvshows.Where(t => t.Producer.Id == tvShow.ProducerId && t.Type == tvShow.Type || t.CategoryId == tvShow.CategoryId && t.Type == tvShow.Type && t.Language == tvShow.Language).Take(10).ToList();

                var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvShow.Id).ToList();

                var viewModel = new TvShowDetailsDTO()
                {
                    TvShow = tvShow,
                    RelatedTvShows = tvshows,
                    TvShowParts = parts,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return viewModel;
            }
        }

        public async Task<TvShowDetailsDTO> LikeTvShow(int tvShowID, string userID)
        {
            TvShow tvShow = new TvShow();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == tvShowID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = tvShowID,
                    IsLiked = true,
                    IsDisLiked = false
                };
                await _unitOfWork.Interactions.Add(interaction);

                tvShow = await _unitOfWork.TvShows.GetById(tvShowID);
                tvShow.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                tvShow = await _unitOfWork.TvShows.GetById(tvShowID);
                tvShow.NoOfLikes += 1;
                tvShow.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                tvShow = await _unitOfWork.TvShows.GetById(tvShowID);
                tvShow.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            await _unitOfWork.SaveChanges();

            tvShow = await _unitOfWork.TvShows.GetById(tvShowID);

            var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
            tvshows = tvshows.Where(t => t.Producer.Id == tvShow.ProducerId && t.Type == tvShow.Type || t.CategoryId == tvShow.CategoryId && t.Type == tvShow.Type && t.Language == tvShow.Language).Take(10).ToList();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvShow.Id).ToList();

            var viewModel = new TvShowDetailsDTO()
            {
                TvShow = tvShow,
                RelatedTvShows = tvshows,
                TvShowParts = parts,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return viewModel;
        }

        public async Task<TvShowDetailsDTO> DisLikeTvShow(int TvShowID, string userID)
        {
            TvShow tvShow = new TvShow();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
               .FirstOrDefault(fi => fi.ItemId == TvShowID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = TvShowID,
                    IsLiked = false,
                    IsDisLiked = true
                };
                await _unitOfWork.Interactions.Add(interaction);

                tvShow = await _unitOfWork.TvShows.GetById(TvShowID);
                tvShow.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                tvShow = await _unitOfWork.TvShows.GetById(TvShowID);
                tvShow.NoOfDisLikes += 1;
                tvShow.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                tvShow = await _unitOfWork.TvShows.GetById(TvShowID);
                tvShow.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            await _unitOfWork.SaveChanges();

            tvShow = await _unitOfWork.TvShows.GetById(TvShowID);

            var tvshows = _unitOfWork.TvShows.GetTvShows().ToList();
            tvshows = tvshows.Where(t => t.Producer.Id == tvShow.ProducerId && t.Type == tvShow.Type || t.CategoryId == tvShow.CategoryId && t.Type == tvShow.Type && t.Language == tvShow.Language).Take(10).ToList();

            var parts = _unitOfWork.Parts.GetFilteredPartsWithTvShowId(tvShow.Id).ToList();

            var viewModel = new TvShowDetailsDTO()
            {
                TvShow = tvShow,
                RelatedTvShows = tvshows,
                TvShowParts = parts,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return viewModel;
        }

        public IEnumerable<TvShow> GetTvShowsForSearch(string key)
        {
            var result = _unitOfWork.TvShows.GetTvShows().Where(a => a.Name.ToLower().Contains(key.ToLower()));

            return result.Any() ? result : Enumerable.Empty<TvShow>();
        }

        public async Task<TvShowDataForSelectListsDTO> GetTvShowDataForSelectLists()
        {
            return new TvShowDataForSelectListsDTO()
            {
                Actors = await _unitOfWork.Actors.GetAllWithoutPagination(),
                Producers = await _unitOfWork.Producers.GetAllWithoutPagination(),
                Categories = await _unitOfWork.Categories.GetAllWithoutPagination(),
                Countries = await _unitOfWork.Countries.GetAllWithoutPagination()
            };
        }
    }
}