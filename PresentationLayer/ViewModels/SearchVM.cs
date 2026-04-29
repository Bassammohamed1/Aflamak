using DataAccessLayer.Models;

namespace PresentationLayer.ViewModels
{
    public class SearchVM
    {
        public IEnumerable<Film> Films { get; set; }
        public IEnumerable<TvShow> TvShows { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Producer> Producers { get; set; }
    }
}
