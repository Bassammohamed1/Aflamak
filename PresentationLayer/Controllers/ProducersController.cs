using DataAccessLayer.Models;
using DataAccessLayer.Models.ViewModels;
using DataAccessLayer.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProducersController : Controller
    {
        private IUnitOfWork _unitOfWork;
        public ProducersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            IEnumerable<Producer> result = _unitOfWork.Producers.GetAll(pageNumber, pageSize);
            return View(result);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Add(Producer producer)
        {
            if (ModelState.IsValid)
            {
                if (producer.clientFile != null)
                {
                    var stream = new MemoryStream();
                    producer.clientFile.CopyTo(stream);
                    producer.dbImage = stream.ToArray();
                }
                _unitOfWork.Producers.Add(producer);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(producer);
            }
        }
        public IActionResult Update(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var producer = _unitOfWork.Producers.GetById(Id);
            if (producer == null)
                return NotFound();
            return View(producer);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Producer producer)
        {
            if (ModelState.IsValid)
            {
                if (producer.clientFile != null)
                {
                    var stream = new MemoryStream();
                    producer.clientFile.CopyTo(stream);
                    producer.dbImage = stream.ToArray();
                }
                _unitOfWork.Producers.Update(producer);
                _unitOfWork.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(producer);
            }
        }
        public IActionResult Delete(int Id)
        {
            if (Id == null || Id == 0)
                return NotFound();
            var producer = _unitOfWork.Producers.GetById(Id);
            if (producer == null)
                return NotFound();
            return View(producer);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(Producer producer)
        {
            _unitOfWork.Producers.Delete(producer);
            _unitOfWork.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            if (id == null)
                return NotFound();

            var producer = _unitOfWork.Producers.GetById(id);
            if (producer == null)
                return NotFound();

            var films = _unitOfWork.Films.GetFilteredFilmsWithId(producer.Id, "Producer").ToList();
            var tvShows = _unitOfWork.TvShows.GetFilteredTvShowsWithId(producer.Id, "Producer").ToList();

            var works = films.Select(f => new ItemViewModel { Type = "Film", Item = f }).
                Concat(tvShows.Select(t => new ItemViewModel { Type = "TvShow", Item = t })).ToList();

            var viewModel = new PersonDetailsVM<Producer>()
            {
                Person = producer,
                Works = works
            };

            return View(viewModel);
        }
    }
}
