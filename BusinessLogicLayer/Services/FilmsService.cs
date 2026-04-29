using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogicLayer.Services
{
    public class FilmsService : IFilmsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilmsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Film> GetFilmByID(int id)
        {
            return await _unitOfWork.Films.GetFilmById(id);
        }

        public IEnumerable<Film> GetFilms(Keys? key, Languages? language, ItemType? itemType)
        {
            if (language is null && itemType == ItemType.فيلم && key is not null)
                return _unitOfWork.Films.GetFilms().AsQueryable()
                    .OrderByDescending(x => EF.Property<object>(x, key.ToString()))
                    .ThenByDescending(x => key == Keys.Year ? x.Month : x.Id) 
                    .Where(m => m.Type == 1);

            else if (language is not null && itemType is null)
                return _unitOfWork.Films.GetFilms()
                 .Where(m => m.Type == 1 && m.Language == (int)language);

            else if (itemType is not null && language is null)
                return _unitOfWork.Films.GetFilms()
                    .OrderByDescending(x => x.NoOfLikes)
                    .Where(m => m.Type == 3);
            else
                return _unitOfWork.Films.GetFilms();
        }

        public async Task<IEnumerable<Film>> GetAllFilms(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Films.GetAllFilms(pageNumber, pageSize);
        }

        public IQueryable<Film> GetFilteredFilmsWithKey(int id, Keys key)
        {
            if (key == Keys.ID)
                return _unitOfWork.Films.GetFilteredFilmsWithID(id);

            else if (key == Keys.Producer)
                return _unitOfWork.Films.GetFilteredFilmsWithProducerID(id);

            else
                return Enumerable.Empty<Film>().AsQueryable();
        }

        public async Task<IEnumerable<Film>> GetAllFilmsOrderedByKey(int pageNumber, int pageSize, Keys key)
        {
            return await _unitOfWork.Films.GetAllFilmsOrderedByKey(pageNumber, pageSize, key);
        }

        public IQueryable<Film> GetFilteredFilms(string genre, string country, int? language, int? year, bool isArabic = false, bool isCartoon = false)
        {
            if (isArabic && !isCartoon)
            {
                var query = GetFilms(null, Languages.عربي, null).AsQueryable();

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

                var films = query;

                return films;
            }
            else if (isCartoon && !isArabic)
            {
                var query = GetFilms(null, null, ItemType.كرتون).AsQueryable();

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

                var films = query;

                return films;
            }
            else
            {
                var query = GetFilms(null, null, null).AsQueryable();

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

                var films = query;

                return films;
            }
        }

        public IQueryable<Film> GetRelatedFilmsByKey(Keys key)
        {
            List<Film> data = new List<Film>();

            if (key == Keys.NoOfLikes)
            {
                var result = _unitOfWork.Films.GetFilms()
                    .OrderBy(f => f.Name)
                    .Where(f => f.NoOfLikes > 5000).ToList();

                if (result.Any())
                {
                    data.AddRange(result);

                    return data.AsQueryable();
                }
            }
            else if (key == Keys.Year)
            {
                var result1 = _unitOfWork.Films.GetFilms()
                    .OrderBy(f => f.Name)
                    .Where(f => f.Month < 8 && f.Year == DateTime.Now.Year).ToList();

                if (result1 is not null)
                    data.AddRange(result1);

                var result2 = _unitOfWork.Films.GetFilms()
                    .OrderBy(f => f.Name)
                    .Where(f => f.Month >= 8 && (f.Year == DateTime.Now.Year || f.Year == DateTime.Now.Year - 1)).ToList();

                if (result2 is not null)
                    data.AddRange(result2);

                if (data.Any())
                    return data.AsQueryable();
            }

            return Enumerable.Empty<Film>().AsQueryable();
        }

        public async Task<Result> AddFilm(FilmDTO data)
        {
            if (data.clientFile != null)
            {
                var stream = new MemoryStream();
                await data.clientFile.CopyToAsync(stream);
                data.dbImage = stream.ToArray();


                int languageId = (int)data.Language;
                int typeId = (int)data.Type;

                var film = new Film()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    dbImage = data.dbImage,
                    IsSeries = data.IsSeries,
                    PartsNo = data.PartsNo,
                    Root = data.Root,
                    NoOfLikes = data.NoOfLikes,
                    NoOfDisLikes = data.NoOfDisLikes,
                    Year = data.Year,
                    Month = data.Month,
                    Type = typeId,
                    Language = languageId,
                    CountryId = data.CountryId,
                    CategoryId = data.CategoryId,
                    ProducerId = data.ProducerId
                };

                var result = await _unitOfWork.Films.AddFilm(film, data.ActorsId);

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

        public async Task<Result> UpdateFilm(FilmDTO data)
        {
            if (data.clientFile != null)
            {
                var stream = new MemoryStream();
                await data.clientFile.CopyToAsync(stream);
                data.dbImage = stream.ToArray();

                int languageId = (int)data.Language;
                int typeId = (int)data.Type;

                var film = new Film()
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    dbImage = data.dbImage,
                    IsSeries = data.IsSeries,
                    PartsNo = data.PartsNo,
                    Root = data.Root,
                    NoOfLikes = data.NoOfLikes,
                    NoOfDisLikes = data.NoOfDisLikes,
                    Year = data.Year,
                    Month = data.Month,
                    Type = typeId,
                    Language = languageId,
                    CountryId = data.CountryId,
                    CategoryId = data.CategoryId,
                    ProducerId = data.ProducerId
                };

                var result = await _unitOfWork.Films.UpdateFilm(film, data.ActorsId);

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

        public Result DeleteFilm(Film film)
        {
            var result = _unitOfWork.Films.Delete(film);

            return result is not null ? new Result() { Success = true } :
                new Result()
                {
                    Success = false,
                    Error = "An error ouccered while deleting."
                };
        }

        public async Task<FilmDetailsDTO> FilmDetails(int filmID, bool isAuthenticated, string? userID)
        {
            if (isAuthenticated)
            {
                var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                    .FirstOrDefault(fi => fi.ItemId == filmID && fi.UserId == userID);

                var film = await _unitOfWork.Films.GetFilmById(filmID);

                if (film == null)
                    throw new NullReferenceException();

                var films = _unitOfWork.Films.GetFilms().ToList();
                films = films
                    .Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

                if (interaction is not null)
                {
                    var viewModel = new FilmDetailsDTO()
                    {
                        Film = film,
                        RelatedFilms = films,
                        HasUserLiked = interaction.IsLiked,
                        HasUserDisliked = interaction.IsDisLiked
                    };

                    return viewModel;
                }
                else
                {
                    var viewModel = new FilmDetailsDTO()
                    {
                        Film = film,
                        RelatedFilms = films,
                        HasUserLiked = false,
                        HasUserDisliked = false
                    };

                    return viewModel;
                }
            }
            else
            {
                var film = await _unitOfWork.Films.GetFilmById(filmID);

                if (film == null)
                    throw new NullReferenceException();

                var films = _unitOfWork.Films.GetFilms().ToList();
                films = films
                    .Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type).Take(10).ToList();

                var viewModel = new FilmDetailsDTO()
                {
                    Film = film,
                    RelatedFilms = films,
                    HasUserLiked = false,
                    HasUserDisliked = false
                };

                return viewModel;
            }
        }

        public async Task<FilmDetailsDTO> LikeFilm(int filmID, string userID)
        {
            Film film = new Film();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == filmID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = filmID,
                    IsLiked = true,
                    IsDisLiked = false
                };
                await _unitOfWork.Interactions.Add(interaction);

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfLikes += 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsLiked = true;
                interaction.IsDisLiked = false;

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfLikes += 1;
                film.NoOfDisLikes -= 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }
            await _unitOfWork.SaveChanges();

            var films = _unitOfWork.Films.GetFilms().ToList();

            films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type)
                .Take(10).ToList();

            var viewModel = new FilmDetailsDTO()
            {
                Film = film,
                RelatedFilms = films,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return viewModel;
        }

        public async Task<FilmDetailsDTO> DisLikeFilm(int filmID, string userID)
        {
            Film film = new Film();

            var interaction = _unitOfWork.Interactions.GetAllWithoutPagination().Result
                .FirstOrDefault(fi => fi.ItemId == filmID && fi.UserId == userID);

            if (interaction == null)
            {
                interaction = new Interaction
                {
                    UserId = userID,
                    ItemId = filmID,
                    IsLiked = false,
                    IsDisLiked = true
                };
                await _unitOfWork.Interactions.Add(interaction);

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfDisLikes += 1;
            }
            else if (interaction.IsLiked)
            {
                interaction.IsLiked = false;
                interaction.IsDisLiked = true;

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfDisLikes += 1;
                film.NoOfLikes -= 1;
            }
            else if (interaction.IsDisLiked)
            {
                interaction.IsDisLiked = false;

                film = await _unitOfWork.Films.GetById(filmID);
                film.NoOfDisLikes -= 1;

                _unitOfWork.Interactions.Delete(interaction);
            }

            await _unitOfWork.SaveChanges();

            var films = _unitOfWork.Films.GetFilms().ToList();

            films = films.Where(f => f.Root != null && f.Root == film.Root || f.Producer.Id == film.ProducerId && f.Type == film.Type || f.CategoryId == film.CategoryId && f.Type == film.Type)
                .Take(10).ToList();

            var viewModel = new FilmDetailsDTO()
            {
                Film = film,
                RelatedFilms = films,
                HasUserLiked = interaction.IsLiked,
                HasUserDisliked = interaction.IsDisLiked
            };

            return viewModel;
        }

        public IEnumerable<Film> GetFilmsForSearch(string key)
        {
            var result = _unitOfWork.Films.GetFilms().Where(a => a.Name.ToLower().Contains(key.ToLower()));

            return result.Any() ? result : Enumerable.Empty<Film>();
        }

        public async Task<FilmDataForSelectListsDTO> GetFilmDataForSelectLists()
        {
            return new FilmDataForSelectListsDTO()
            {
                Actors = await _unitOfWork.Actors.GetAllWithoutPagination(),
                Producers = await _unitOfWork.Producers.GetAllWithoutPagination(),
                Categories = await _unitOfWork.Categories.GetAllWithoutPagination(),
                Countries = await _unitOfWork.Countries.GetAllWithoutPagination()
            };
        }
    }
}