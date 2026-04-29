using BusinessLogicLayer.Helpers;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Enums;
using DataAccessLayer.Models;
using DataAccessLayer.Repository.Interfaces;

namespace BusinessLogicLayer.Services
{
    public class ActorsService : IActorsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFilmsService _filmsService;
        private readonly ITvShowsService _tvShowsService;

        public ActorsService(IUnitOfWork unitOfWork, IFilmsService filmsService, ITvShowsService tvShowsService)
        {
            _unitOfWork = unitOfWork;
            _filmsService = filmsService;
            _tvShowsService = tvShowsService;
        }

        public async Task<Actor> GetActorByID(int id)
        {
            return await _unitOfWork.Actors.GetById(id);
        }

        public async Task<IEnumerable<Actor>> GetAllActors(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Actors.GetAll(pageNumber, pageSize);
        }

        public async Task<Result> AddActor(Actor actor)
        {
            if (actor.clientFile != null)
            {
                var stream = new MemoryStream();
                await actor.clientFile.CopyToAsync(stream);
                actor.dbImage = stream.ToArray();

                var result = await _unitOfWork.Actors.Add(actor);
                await _unitOfWork.SaveChanges();

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

        public async Task<Result> UpdateActor(Actor actor)
        {
            if (actor.clientFile != null)
            {
                var stream = new MemoryStream();
                await actor.clientFile.CopyToAsync(stream);
                actor.dbImage = stream.ToArray();

                var result = _unitOfWork.Actors.Update(actor);
                await _unitOfWork.SaveChanges();

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

        public Result DeleteActor(Actor actor)
        {
            var result = _unitOfWork.Actors.Delete(actor);

            return result is not null ? new Result() { Success = true } :
                     new Result()
                     {
                         Success = false,
                         Error = "An error ouccered while deleting."
                     };
        }

        public IEnumerable<Actor> GetActorsForSearch(string key)
        {
            var results = _unitOfWork.Actors.GetAllWithoutPagination().Result
                .Where(a => a.Name.ToLower().Contains(key.ToLower()) || a.AnotherLangName.ToLower().Contains(key.ToLower()));

            return results.Any() ? results : Enumerable.Empty<Actor>();
        }

        public async Task<PersonDetailsDTO<Actor>> GetActorDetails(int id)
        {
            var actor = await this.GetActorByID(id);

            if (actor is not null)
            {
                var ids1 = _unitOfWork.ActorFilms.GetAllWithoutPagination().Result
                    .Where(a => a.ActorId == actor.Id)
                    .Select(a => a.FilmId);

                List<Film> actorFilms = new List<Film>();

                foreach (var filmID in ids1)
                {
                    actorFilms.AddRange(_filmsService.GetFilteredFilmsWithKey(filmID, Keys.ID));
                }

                var ids2 = _unitOfWork.ActorTvShows.GetAllWithoutPagination().Result
                    .Where(a => a.ActorId == actor.Id)
                    .Select(a => a.TvShowId);

                List<TvShow> actorTvShows = new List<TvShow>();

                foreach (var tvShowID in ids2)
                {
                    actorTvShows.AddRange(_tvShowsService.GetFilteredTvShowsWithKey(tvShowID, Keys.ID));
                }

                var works = actorFilms.Select(f => new ItemDTO { Type = "Film", Item = f }).
                    Concat(actorTvShows.Select(t => new ItemDTO { Type = "TvShow", Item = t })).ToList();

                return new PersonDetailsDTO<Actor>()
                {
                    Person = actor,
                    Works = works
                };
            }

            throw new NullReferenceException();
        }
    }
}