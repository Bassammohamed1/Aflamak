using DataAccessLayer.Models;

namespace DataAccessLayer.Models.ViewModels
{
    public class TvShowVM
    {
        public IEnumerable<TvShow> TvShows { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool FromHome { get; set; }
        public bool Arabic { get; set; }
        public bool Ramadan { get; set; }
    }
}
