using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ActorsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ActorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            IEnumerable<Actor> result = _unitOfWork.Actors.GetAll(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Actor actor)
        {
            if (ModelState.IsValid)
            {
                if (actor.clientFile != null)
                {
                    var stream = new MemoryStream();
                    actor.clientFile.CopyTo(stream);
                    actor.dbImage = stream.ToArray();
                }
                _unitOfWork.Actors.Add(actor);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(actor);
            }
        }
        public IActionResult Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var actor = _unitOfWork.Actors.GetById(Id);
            if (actor == null)
                return NotFound();
            return View(actor);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Actor actor)
        {
            if (ModelState.IsValid)
            {
                if (actor.clientFile != null)
                {
                    var stream = new MemoryStream();
                    actor.clientFile.CopyTo(stream);
                    actor.dbImage = stream.ToArray();
                }
                _unitOfWork.Actors.Update(actor);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(actor);
            }
        }
        public IActionResult Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var actor = _unitOfWork.Actors.GetById(Id);
            if (actor == null)
                return NotFound();
            return View(actor);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Actor actor)
        {
            _unitOfWork.Actors.Delete(actor);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var actor = _unitOfWork.Actors.GetById(id);
            if (actor == null)
                return NotFound();

            var ids1 = _unitOfWork.ActorFilms.GetAllWithoutPagination().Where(a => a.ActorId == actor.Id).Select(a => a.FilmId);
            List<Film> actorFilms = new List<Film>();
            foreach (var item in ids1)
            {
                actorFilms.AddRange(_unitOfWork.Films.GetFilteredFilmsWithId(item, "ID"));
            }

            var ids2 = _unitOfWork.ActorTvShows.GetAllWithoutPagination().Where(a => a.ActorId == actor.Id).Select(a => a.TvShowId);
            List<TvShow> actorTvShows = new List<TvShow>();
            foreach (var item in ids2)
            {
                actorTvShows.AddRange(_unitOfWork.TvShows.GetFilteredTvShowsWithId(item, "ID"));
            }

            var works = actorFilms.Select(f => new ItemViewModel { Type = "Film", Item = f }).
                Concat(actorTvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t })).ToList();

            var viewModel = new PersonDetailsVM<Actor>()
            {
                Person = actor,
                Works = works
            };

            return View(viewModel);
        }
    }
}
